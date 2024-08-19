using Microsoft.AspNetCore.Mvc;
using PLC_V2.PLC_ReadFunction;
using NModbus;
using System.Threading.Tasks;

namespace PLC_V2.Controllers.PLCReadController
{
    [ApiController]
    [Route("api/[controller]")]

    public class ModbusReadController : ControllerBase
    {
        private readonly ModbusTCPRead _modbusTCPRead;

        public ModbusReadController(ModbusTCPRead modbusTCPRead)
        {
            _modbusTCPRead = modbusTCPRead;
        }
        [HttpGet("read-coils")]
        public async Task<IActionResult> ReadCoils([FromBody] ushort startAddress, [FromBody] ushort numberOfPoints)
        {
            var result = await _modbusTCPRead.ReadCoilAsync(startAddress, numberOfPoints);
            return Ok(result);
        }

        [HttpGet("read-registers")]
        public async Task<IActionResult> ReadRegister([FromBody] ushort startAddress, [FromBody] ushort numberOfPoints)
        {
            var result = await _modbusTCPRead.ReadHoldingRegistersAsync(startAddress, numberOfPoints);
            return Ok(result);
        }

    }
}
