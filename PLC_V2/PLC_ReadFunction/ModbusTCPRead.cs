using System;
using System.Threading.Tasks;
using NModbus;
using System.Net.Sockets;

namespace PLC_V2.PLC_ReadFunction
{
    public class ModbusTCPRead
    {
        private readonly string _ipAddress;
        private readonly int _port;
        private TcpClient _tcpClient;
        private IModbusMaster _modbusMaster;
        private readonly int _reconnectInterval;

        public ModbusTCPRead(string ipAddress, int port, int reconnectInterval)
        {
            _ipAddress = ipAddress;
            _port = port;
            _reconnectInterval = reconnectInterval;
        }
        private async Task<bool> ConnectAsync()
        {
            while (_tcpClient == null || !_tcpClient.Connected)
            {
                try
                {
                    _tcpClient = new TcpClient();
                    await _tcpClient.ConnectAsync(_ipAddress, _port);

                    var factory = new ModbusFactory();
                    _modbusMaster = factory.CreateMaster(_tcpClient);
                    Console.WriteLine("Connected to PLC.");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Connection failed: {ex.Message}. Retrying in {_reconnectInterval / 1000} seconds...");
                    await Task.Delay(_reconnectInterval);
                }
            }

            return false;
        }

        public async Task<bool[]> ReadCoilAsync(ushort startAddress,ushort numberOfPoints)
        {
            if (_tcpClient == null || !_tcpClient.Connected)
            {
                Console.WriteLine("Not connected to PLC. Trying to reconnect...");
                await ConnectAsync();
            }

            if (_tcpClient != null && _tcpClient.Connected)
            {
                return _modbusMaster.ReadCoils(1, startAddress, numberOfPoints);
            }
            else
            {
                Console.WriteLine("Failed to connect to PLC.");
                return null;
            }
        }
        public async Task<ushort[]> ReadHoldingRegistersAsync(ushort startAddress, ushort numberOfPoints)
        {
            if (_tcpClient == null || !_tcpClient.Connected)
            {
                Console.WriteLine("Not connected to PLC. Trying to reconnect...");
                await ConnectAsync();
            }

            if (_tcpClient != null && _tcpClient.Connected)
            {
                return _modbusMaster.ReadHoldingRegisters(1, startAddress, numberOfPoints);
            }
            else
            {
                Console.WriteLine("Failed to connect to PLC.");
                return null;
            }
        }

        public void Disconnect()
        {
            if (_tcpClient != null)
            {
                _tcpClient.Close();
                _tcpClient = null;
                Console.WriteLine("Disconnected from PLC.");
            }
        }

    }
}
