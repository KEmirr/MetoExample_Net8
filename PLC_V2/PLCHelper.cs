using NModbus;
using System.Net.Sockets;

namespace PLC_V2
{
    public class PLCHelper
    {
        private readonly string _ipAddress;
        private readonly int _port;
        private readonly int _writeRegisterAddress;
        private readonly int _readRegisterAddress;

        public PLCHelper(string ipAddress, int port, int writeRegisterAddress, int readRegisterAddress)
        {
            _ipAddress = ipAddress;
            _port = port;
            _writeRegisterAddress = writeRegisterAddress;
            _readRegisterAddress = readRegisterAddress;
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
