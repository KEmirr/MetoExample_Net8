using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PLC_V2.Controllers.PLCConnection
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModbusController : ControllerBase
    {
        private readonly ModbusTCPConnectionFactory _modbusConnectionFactory;

        public ModbusController(ModbusTCPConnectionFactory modbusConnectionFactory)
        {
            _modbusConnectionFactory = modbusConnectionFactory;
        }

        [HttpPost("connection-check")]
        public async Task<IActionResult> WriteConnectionCoil([FromBody] WriteCoilRequest request)
        {
            await _modbusConnectionFactory.WriteSingleCoilAsync(request.CoilAddress, request.Value);
            return Ok($"Coil at address {request.CoilAddress} set to {request.Value}.");
        }
    }

    public class WriteCoilRequest
    {
        public ushort CoilAddress { get; set; }
        public bool Value { get; set; }
    }
}
