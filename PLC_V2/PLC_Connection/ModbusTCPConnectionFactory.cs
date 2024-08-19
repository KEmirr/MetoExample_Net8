using System;
using System.Threading.Tasks;
using NModbus;
using System.Net.Sockets;

public class ModbusTCPConnectionFactory
{
    private readonly string _ipAddress;
    private readonly int _port;
    private TcpClient _tcpClient;
    private IModbusMaster _modbusMaster;
    private readonly int _reconnectInterval;

    public ModbusTCPConnectionFactory(string ipAddress, int port, int reconnectInterval = 5000)
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

    public async Task WriteSingleCoilAsync(ushort coilAddress, bool value)
    {
        if (_tcpClient == null || !_tcpClient.Connected)
        {
            Console.WriteLine("Not connected to PLC. Trying to reconnect...");
            await ConnectAsync();
        }

        if (_tcpClient != null && _tcpClient.Connected)
        {
            _modbusMaster.WriteSingleCoil(1, coilAddress, value);
            Console.WriteLine($"Coil at address {coilAddress} set to {value}.");
        }
        else
        {
            Console.WriteLine("Failed to connect to PLC.");
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
