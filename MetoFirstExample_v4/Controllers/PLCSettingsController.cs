﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using MetoFirstExample_v4_WepAPI.Helpers;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Sockets;
using System.Data.SqlClient;

namespace MetoFirstExample_v4_WepAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //plc yapılanması kontrol edilecektir. 
    public class PLCSettingsController : ControllerBase
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly PLCHelper _plcHelper;
        public PLCSettingsController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper; 
        }
        public PLCSettingsController(PLCHelper plcHelper)
        {
            _plcHelper = plcHelper;
        }
        [HttpGet("check-connection")]
        public async Task<IActionResult> CheckPLCConnection()
        {
            try
            {
                var settings = await _databaseHelper.GetPLCSettingAsync();
                string ipAddress = settings["plcIP"];
                if (!int.TryParse(settings["port"], out int port))
                {
                    await LogErrorAsync("Invalid port number.");
                    return StatusCode(400, "Invalid port number.");
                }

                bool isConnection = await CheckConnectionAsync(ipAddress, port);
                if (isConnection)
                {
                    return Ok("PLC bağlantısı Başarılı \n");
                }
                else
                {
                    await LogErrorAsync("PLC Bağlantısı Başarısız ConToPLC-1 \n");
                    return StatusCode(501, "PLC Connection Failed ConToPLC-2 \n");
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync($"Error in Checking PLC Connection: {ex.Message}");
                return StatusCode(500, $"Internal server error PLC Connection: {ex.Message}");
            }
        }
        
        [HttpPost("write")]
        public async Task<IActionResult> WriteToPlc()
        {
            try
            {
                var settings = await _databaseHelper.GetPLCSettingAsync();
            string ipAddress = settings["plcIP"];
            int port = int.Parse(settings["port"]);
            
            bool isConnected=await CheckConnectionAsync(ipAddress, port);
                if (isConnected)
                {
                    var plcHelper = new PLCHelper(
                        ipAddress,
                        port,
                        int.Parse(settings["plc_FotoKaydet"]),
                        int.Parse(settings["plc_sayac"])
                    );
                    ushort value = await plcHelper.ReadSingleRegisterAsync();
                    return Ok(value);
                }
                else
                {
                    await LogErrorAsync("PLC Bağlantısı Başarısız WriteToPLC \n");
                    return StatusCode(503, "PLC Connection Başarısız WriteToPLC \n");
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync($"Error in Checking WriteToPLC... Looking Please Ethernet Cable or PLC IP Address Checking {ex.Message}");
                return StatusCode(504, $"Internel server Error WriteToPLC: {ex.Message}");
            }
            
         }
        [HttpPost("read")]
        public async Task<IActionResult> ReadFromPLC()
        {
            try
            {
                var settings = await _databaseHelper.GetPLCSettingAsync();
                string ipAddress = settings["plcIP"];
                int port = int.Parse(settings["plcPort"]);

                bool isConnected = await CheckConnectionAsync(ipAddress, port);
                if (isConnected)
                {
                    var plcHelper = new PLCHelper(
                        ipAddress,
                        port,
                        int.Parse(settings["plc_FotoKaydet"]),
                        int.Parse(settings["plc_sayac"])
                    );

                    ushort value = await plcHelper.ReadSingleRegisterAsync();
                    return Ok(value);
                }
                else
                {
                    await LogErrorAsync("PLC Bağlantısı Başarısız ReadFromPLC \n.");
                    return StatusCode(505, "PLC Connection Başarısız ReadFromPLC \n");
                }

            }
            catch (Exception ex)
            {
                await LogErrorAsync($"Error in Checking ReadFromPLC... Looking Please Ethernet Cable or PLC IP Address Checking{ex.Message} \n");
                return StatusCode(506, $"Internel server Error ReadFromPLC: {ex.Message}");

            }
        }
        private async Task<bool> CheckConnectionAsync(string ipAddress, int port) 
        {
            try
            {
                using (var client=new TcpClient())
                {
                    await client.ConnectAsync(ipAddress, port);
                    return client.Connected;
                }
            }
            catch (SocketException ex)
            {
                await LogErrorAsync($"SocketException: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                await LogErrorAsync($"Exception: {ex.Message}");
                return false;
            }
        }
        public interface IDatabaseHelper
        {
            Task<Dictionary<string, string>> GetPLCSettingAsync();
        }

        private async Task LogErrorAsync(string message) 
        {
            var parametres = new SqlParameter[]
            {
                new SqlParameter("@Message",message),
                new SqlParameter("@CreatedAt",DateTime.Now)
            };
            await _databaseHelper.ExecuteStoredProcedureAsync("sp_InsertLog", parametres);
        }
    }
}
