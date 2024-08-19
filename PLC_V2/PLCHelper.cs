using NModbus;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;

namespace PLC_V2
{
    public class PLCHelper
    {
        private readonly string _ipAddress;
        private readonly int _port;
        private readonly int _writeRegisterAddress;
        private readonly int _readRegisterAddress;

        public PLCHelper(IConfiguration configuration)
        {
            _ipAddress = configuration["PLCSettings:IpAddress"];
            _port = int.Parse(configuration["PLCSettings:Port"]);
            _writeRegisterAddress = int.Parse(configuration["PLCSettings:WriteRegisterAddress"]);
            _readRegisterAddress = int.Parse(configuration["PLCSettings:ReadRegisterAddress"]);
        }

        public async Task WriteToPlcAsync()
        {
            try
            {
                using (var client = new TcpClient(_ipAddress, _port))
                {
                    var factory = new ModbusFactory();
                    var master = factory.CreateMaster(client);

                    ushort valueToWrite = 20; // Register değerini 20 olarak ayarla
                    await master.WriteSingleRegisterAsync(0, (ushort)_writeRegisterAddress, valueToWrite);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"PLC'ye yazma hatası: {ex.Message}");
            }
        }

        public async Task<ushort> ReadSingleRegisterAsync()
        {
            try
            {
                using (var client = new TcpClient(_ipAddress, _port))
                {
                    var factory = new ModbusFactory();
                    var master = factory.CreateMaster(client);
                    var registers = await master.ReadHoldingRegistersAsync(0, (ushort)_readRegisterAddress, 1);
                    return registers[0];
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"PLC'den okuma hatası: {ex.Message}");
            }
        }
    }
}
