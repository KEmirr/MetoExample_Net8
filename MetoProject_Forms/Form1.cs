using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using MetoFirstExample_v4_WepAPI;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering.Helpers;
using System.Diagnostics.Eventing.Reader;
using System.Collections.Generic;
using DevExpress.XtraPrinting.Native.LayoutAdjustment;

namespace MetoProject_Forms
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        private static readonly HttpClient client = new HttpClient();
        private FileSystemWatcher fileWatcher;
        private string watchPath = @"C:\Users\USER55\Desktop\Captured_Images2";
        private PLCHelper plcHelper;

        private FileSystemWatcher _fileWatcher;

        public Form1()
        {
            InitializeComponent();
        }

        private async void button_fileOn_Click(object sender, EventArgs e)
        {
            if (await CheckPLCConnectAsync())
            {
                await WriteToPlcAsync();

                var value = await ReadToPlcAsync();
                if (value != null)
                {
                    textEdit_Counter.Text = value.Value.ToString();
                }
            }
            await StartFileWatcher();
        }

        private async void button_fileOff_Click(object sender, EventArgs e)
        {
            await StopFileWatcherAsync();
        }

        private async Task StopFileWatcherAsync()
        {
            await Task.Run(() =>
            {
                if (fileWatcher != null)
                {
                    fileWatcher.EnableRaisingEvents = false;
                    fileWatcher.Dispose();
                    fileWatcher = null;
                }
            });

            memoEditLogs.AppendText("Klasör İzleme Durduruldu.\n");
        }

        private async Task StartFileWatcher()
        {

            await Task.Run(() =>
            {
                if (fileWatcher != null)
                {
                    fileWatcher.Dispose();
                }

                fileWatcher = new FileSystemWatcher(watchPath);
                fileWatcher.Filter = "*.jpg";
                fileWatcher.Created += async (sender, e) => await OnFileCreatedAsync(sender, e);
                fileWatcher.EnableRaisingEvents = true;
            });

            memoEditLogs.AppendText("Klasör İzleme Başlatıldı.\n");
        }

        private async Task OnFileCreatedAsync(object sender, FileSystemEventArgs e)
        {
            string filePath = e.FullPath;
            string logMessage = $"File created: {filePath}";

            try
            {
                byte[] fileData = File.ReadAllBytes(filePath);
                await SaveFileToDatabaseAsync(filePath);

                memoEditLogs.Invoke(new Action(() =>
                {
                    memoEditLogs.AppendText($"File saved: {filePath}\n");
                }));

                File.Delete(filePath); // Dosyayı klasörden sil

                // Dosya kaydedildikten sonra başarılı log mesajı
                await LogToApiAsync($"File saved and deleted: {filePath}", "Information", "FileWatcher");
                //PLC yazma işlemi bu kısımda yapılmaktadır.
                await plcHelper.WriteToPlcAsync();
                ushort plcValue = await plcHelper.ReadSingleRegisterAsync();
                memoEditLogs.Invoke(new Action(() =>
                {
                    memoEditLogs.AppendText($"PLC Value: {plcValue}\n");
                }));
            }
            catch (Exception ex)
            {
                memoEditLogs.Invoke(new Action(() =>
                {
                    memoEditLogs.AppendText($"Error: {ex.Message}\n");
                }));

                // Hata mesajını logla
                await LogToApiAsync($"Error saving file: {filePath}, Exception: {ex.Message}", "Error", "FileWatcher");

            }
        }

        private async Task SaveFileToDatabaseAsync(string filePath)
        {

            var image = new
            {
                ImagePath = filePath,
                CreatedAt = DateTime.Now
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(image);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("http://localhost:9192/api/image", content);

                if (response.IsSuccessStatusCode)
                {
                    memoEditLogs.Invoke(new Action(() =>
                    {
                        memoEditLogs.AppendText($"API Response:{response.StatusCode} \n");
                    }));
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    memoEditLogs.Invoke(new Action(() =>
                    {
                        memoEditLogs.AppendText($"Request error:{response.StatusCode} \n Response:{responseBody} \n");
                    }));
                }
            }
            catch (HttpRequestException ex)
            {
                memoEditLogs.Invoke(new Action(() =>
                {
                    memoEditLogs.AppendText($"Request error: {ex.Message} \n");
                }));
            }
            catch (Exception ex)
            {
                memoEditLogs.Invoke(new Action(() =>
                {
                    memoEditLogs.AppendText($"Unexpected error: {ex.Message} \n");
                }));
            }

        }

        private async Task LogToApiAsync(string logtype, string message, string source)
        {
            var log = new
            {
                logtype = logtype,
                message = message,
                Source = source,
                CreatedAt = DateTime.Now
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(log);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("http://localhost:9192/api/log", content);
                if (response.IsSuccessStatusCode)
                {
                    memoEditLogs.Invoke(new Action(() =>
                    {
                        memoEditLogs.AppendText($"API Response: {response:StatusCode}\n");
                    }));
                }

                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    memoEditLogs.Invoke(new Action(() =>
                    {
                        memoEditLogs.AppendText($"Request error: {{response.StatusCode}} \n Response:{{responseBody}}\n");
                    }));
                }

            }
            catch (HttpRequestException ex)
            {
                memoEditLogs.AppendText($"Response Error: {ex.Message}");

            }
            catch (Exception ex)
            {
                memoEditLogs.Invoke(new Action(() =>
                {
                    memoEditLogs.AppendText($"Unexpected Error:{ex.Message}");
                }));

            }
        }

        private async Task InitializePLCHelper()
        {
            var response = await client.GetAsync("http://localhost:9192/api/PLCSettings");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);



            string ipAddress = settings["PLC_IP"];
            int port = int.Parse(settings["plcPort"]);
            int writeRegisterAddress = int.Parse(settings["plc_FotoKaydet"]);
            int readRegisterAddress = int.Parse(settings["plc_sayac"]);

            plcHelper = new PLCHelper(ipAddress, port, writeRegisterAddress, readRegisterAddress);
        }

        private async Task<bool> CheckPLCConnectAsync()
        {
            try
            {
                HttpResponseMessage Response = await client.GetAsync("http://localhost:9192/api/plc/check-connection");
                Response.EnsureSuccessStatusCode();
                string responseBody = await Response.Content.ReadAsStringAsync();
                memoEditLogs.AppendText($"Request Error PLCConnection:{responseBody} \n");
                return true;

            }
            catch (HttpRequestException ex)
            {
                memoEditLogs.AppendText($"Request error PCLCon: {ex.Message}\n");
                return false;

            }
        }

        private async Task WriteToPlcAsync()
        {
            try
            {
                HttpResponseMessage Response = await client.GetAsync("http://localhost:9192/api/plc/write");
                Response.EnsureSuccessStatusCode();
                string responseBody = await Response.Content.ReadAsStringAsync();
                memoEditLogs.AppendText($"Write To PLC: {responseBody} \n");

            }
            catch (HttpRequestException ex)
            {

                memoEditLogs.AppendText($"Request error: {ex.Message}");
            }
        }

        private async Task<ushort?> ReadToPlcAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:9192/api/plc/read");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                memoEditLogs.AppendText($"Read from PLC: {responseBody} \n");

                return JsonConvert.DeserializeObject<ushort>(responseBody);
            }
            catch (HttpRequestException ex)
            {
                memoEditLogs.AppendText($"Request error: {ex.Message}\n");
                return null;
            }
        }

    }
}
