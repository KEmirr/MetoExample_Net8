using NModbus;
using System.Net.Sockets;

namespace PLC_V2.PLC_WriteFunction
{
    public class ModbusTCPWrite
    {
        private readonly string _ipAddress;
        private readonly int _port;
        private TcpClient _tcpClient;
        private IModbusMaster _modbusMaster;
        private readonly int _reconnectInterval;
        
        public ModbusTCPWrite (string ipaddtess, int port, int reconnectInterval)
        {
            _ipAddress = ipaddtess;
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
        public async Task<bool> WriteCoilAsync(ushort startAddress, bool coilStatus)
        {
            const int maxRetryAttempts = 3;
            const int delayBetweenRetries = 1000; // milliseconds
            int attempt = 0;
            while (true)
            {
                try
                {
                    if (_tcpClient == null || !_tcpClient.Connected)
                    {
                        Console.WriteLine("Not connected to PLC. Trying to reconnect...");
                        await ConnectAsync();
                    }

                    if (_tcpClient != null && _tcpClient.Connected)
                    {
                        await _modbusMaster.WriteSingleCoilAsync(1, startAddress, coilStatus);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SocketException on attempt {attempt + 1}: {ex.Message}");
                    attempt++;
                    if (attempt >= maxRetryAttempts)
                        throw; // Eğer maksimum deneme sayısına ulaşıldıysa hatayı yeniden fırlat
                    await Task.Delay(delayBetweenRetries);

                }
            }
            return false;
        }
        public async Task<bool> WriteRegisterAsync(ushort startAddress, ushort numberOfPoints)
        {
            const int maxRetryAttempts = 3;
            const int delayBetweenRetries = 1000; // milliseconds
            int attempt = 0;
            while (attempt < maxRetryAttempts)
            {
                try
                {
                    if (_tcpClient == null || !_tcpClient.Connected)
                    {
                        Console.WriteLine("Not connected to PLC. Trying to reconnect...");
                        await ConnectAsync();
                    }

                    if (_tcpClient != null && _tcpClient.Connected)
                    {
                       await _modbusMaster.WriteSingleRegisterAsync(1, startAddress, numberOfPoints);
                        return true;
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"SocketException on attempt {attempt + 1}: {ex.Message}");
                    attempt++;
                    if (attempt >= maxRetryAttempts)
                        throw; // Eğer maksimum deneme sayısına ulaşıldıysa hatayı yeniden fırlat
                    await Task.Delay(delayBetweenRetries);
                }
            }
            return false; // Eğer tüm denemeler başarısız olursa null döndür
        }
    }
}
