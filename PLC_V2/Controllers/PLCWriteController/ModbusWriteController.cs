using Microsoft.AspNetCore.Mvc;
using PLC_V2.PLC_WriteFunction;

namespace PLC_V2.Controllers.PLCWriteController
{
    public class ModbusWriteController : ControllerBase
    {
        private readonly ModbusTCPWrite modbusTCPWrite;

        public ModbusWriteController(ModbusTCPWrite modbusTCPWrite)
        {
            this.modbusTCPWrite = modbusTCPWrite;
        }
    }
}
