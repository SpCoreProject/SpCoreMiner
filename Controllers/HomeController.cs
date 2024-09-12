using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using SpCoreMiner.Data;
using SpCoreMiner.Models;
using SpCoreMiner.Serices;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System;
using System.Device.Gpio;
using System.Device.Spi;
using Iot.Device.Mfrc522;
using System.Text.Json;
using SpCoreMiner.Services;
using static System.Net.Mime.MediaTypeNames;
using SPBHashing;
using System.Security.Cryptography;
using System.Management;
using System.Text;

namespace SpCoreMiner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly RfidService _rfidService;

        private readonly RfidService _rfidService;


        public HomeController(ILogger<HomeController> logger,
            RfidService rfidService)
        {
            _rfidService = rfidService;
            _logger = logger;
        }

        //[HttpGet("read-uid")]
        [HttpPost]
        public async Task<JsonResult> ReadUid()
        {
            //string scriptPath = "/var/www/html/Main/read-Rfid.py";
            //string output = RunPythonScript(scriptPath);

            //if (output.StartsWith("Error"))
            //{
            //    return Json(new { success = false, message = "No card detected." });
            //}
            try
            {

                var result = _rfidService.ReadCard();
                var parts = result.Split(',');
                //var parts = output.Split(',');
                if (parts.Length >= 3)
                {
                    var uid = parts[0];
                    var wallet = parts[1];
                    var amount = parts[2];
                    return Json(new { success = true, uid = uid, wallet = wallet, amount = amount });

                }

                return Json(new { success = false, uid = "uid", wallet = parts, amount = "00000" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, uid = "uid", wallet = ex.Message, amount = "0000" });

            }
            //var uid = _rfidService.ReadCardUid();
            //if (uid == null)
            //    return Json("No card detected.");
            //return Json(result);
            //return Json( BitConverter.ToString(uid));
        }
        //private string RunPythonScript(string scriptPath)
        //{
        //    string pythonPath = "/usr/bin/python3"; // مسیر به مفسر پایتون
        //    string args = scriptPath;

        //    var psi = new ProcessStartInfo
        //    {
        //        FileName = pythonPath,
        //        Arguments = args,
        //        RedirectStandardOutput = true,
        //        RedirectStandardError = true,
        //        UseShellExecute = false,
        //        CreateNoWindow = true
        //    };

        //    var process = new Process { StartInfo = psi };
        //    process.Start();

        //    string output = process.StandardOutput.ReadToEnd();
        //    string error = process.StandardError.ReadToEnd();

        //    process.WaitForExit();

        //    if (!string.IsNullOrEmpty(error))
        //    {
        //        return $"Error: {error}";
        //    }

        //    return output.Trim();
        //}
        //[HttpPost("write-card")]
        [HttpPost]
        public async Task<JsonResult> WriteCard()
        {
            string text = "";

            //var result = _rfidService.ReadCard();
            //var parts = result.Split(',');
            //var parts = output.Split(',');

            Random random = new Random();

            int randomNumber1 = random.Next(1, 115);
            int randomNumber2 = random.Next(1, 12);
            string temp = randomNumber1.ToString() + MyDevices.GetMacAddr() + Guid.NewGuid().ToString() + randomNumber2.ToString();
            var spWallet = SPB128.Hash(temp);

            //randomNumber1 = random.Next(1, 115);
            //randomNumber2 = random.Next(1, 12);
            //temp = randomNumber1.ToString() + MyDevices.GetMacAddr() + Guid.NewGuid().ToString() + randomNumber2.ToString();
            //var sppass = SPB128.Hash(temp);
            string uid = "";
            //if (parts.Length >= 2)
            //{
            //    uid = parts[0];
            //}
            text = spWallet + "," + "0.00000000";

            _rfidService.WriteCard(text);
            //var result = _rfidService.WriteCard(text) ;

            //return Json(result);
            return Json(new { success = true, uid = uid, wallet = spWallet, amount = "0.00000000" });
            //if (!json.TryGetProperty("data", out var dataElement) || dataElement.ValueKind != JsonValueKind.String)
            //{
            //    return Json(BadRequest(new { success = false, message = "Invalid data format. Expected a string." }));
            //}

            //var dataString = dataElement.GetString();
            //var dataBytes = Convert.FromBase64String(dataString);

            //var success = _rfidService.WriteToCard(dataBytes);
            //if (!success)
            //    return Json(BadRequest(new { success = false, message = "Failed to write data." }));

            //return Json(new { success = true, message = "Data written successfully." });
        }

        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return Redirect(Request.Headers["Referer"].ToString());
        }
        [HttpPost]
        public async Task<JsonResult> CheckDatabase(string Pin)
        {
            var Setingdb = await SettingDB.Instance;
            var seting = await Setingdb.CheckItemAsync();
            return Json(seting);
        }
        public async Task<JsonResult> CheckMinerSetting(string Pin)
        {
            var Setingdb = await MinerSettingDB.Instance;
            var seting = await Setingdb.CheckItemAsync(Pin);
            return Json(seting);
        }

        public async Task<JsonResult> CheckStaticWallet(string Pin)
        {
            var Setingdb = await WalletStaticDB.Instance;
            var seting = await Setingdb.CheckItemAsync(Pin);
            return Json(seting);
        }
        public class WifiModel
        {
            public string Name { get; set; }
            public string Password { get; set; }
            public bool IsLock { get; set; }
        }
        public async Task<JsonResult> GetWifiList()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"sudo iwlist wlan0 scan | grep 'ESSID' | awk -F ':' '{print $2}' | sed 's/\\\"//g' | sort | uniq\"",
                    //Arguments = "-c \"sudo iwlist wlan0 scan\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                    string result = process.StandardOutput.ReadToEnd();

                    // پردازش خروجی برای تشخیص اینکه آیا شبکه‌ها دارای پسورد هستند یا خیر
                    var networks = result.Split('\n')
                                         .Skip(1) // رد کردن هدر جدول
                                         .Where(line => !string.IsNullOrWhiteSpace(line))
                                         .Select(line => new
                                         {
                                             SSID = line.Substring(8, 30).Trim(),
                                             Security = line.Substring(73).Trim()
                                         })
                                         .Select(n => new
                                         {
                                             n.SSID,
                                             HasPassword = !n.Security.Contains("--") && !n.Security.Contains("NONE")
                                         });

                    return Json(result.Split('\n').Where(ssid => !string.IsNullOrWhiteSpace(ssid)).ToList());
                    //return Json(result);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<JsonResult> ConnectToWifi(string ssid, string password)
        {
            try
            {
                string hashedPsk = GeneratePsk(ssid, password);
                // ایجاد فایل پیکربندی wpa_supplicant
                string configPath = "/etc/wpa_supplicant/wpa_supplicant.conf";
                System.IO.File.WriteAllText(configPath, $"ctrl_interface=DIR=/var/run/wpa_supplicant GROUP=netdev\nupdate_config=1\ncountry=GB\n\nnetwork={{\n    ssid=\"{ssid}\"\n    psk=\"{password}\"\n}}");

                // اجرای wpa_supplicant برای اتصال به شبکه
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"sudo wpa_supplicant -B -i wlan0 -c {configPath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        return Json("Failed to start wpa_supplicant");
                    }
                }

                // دریافت آدرس IP با dhclient
                startInfo = new ProcessStartInfo()
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"sudo dhclient wlan0\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                    if (process.ExitCode == 0)
                    {
                        return Json($"Connected to {ssid}");
                    }
                    else
                    {
                        return Json("Failed to obtain IP address");
                    }
                }
            }
            catch (Exception ex)
            {
                return Json($"Error: {ex.Message}");
            }
        }
        private string GeneratePsk(string ssid, string password)
        {
            try
            {
                // اجرای دستور wpa_passphrase برای تولید PSK
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"wpa_passphrase '{ssid}' '{password}' | grep psk= | grep -v '#psk'\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    string output = process.StandardOutput.ReadToEnd().Trim();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        // استخراج PSK از خروجی دستور
                        return output.Split('=')[1];
                    }
                }
            }
            catch
            {
                // در صورت بروز خطا، مقدار null برگردانده می‌شود
            }

            return null;
        }
        public async Task<JsonResult> TurnOffDevice()
        {
            try
            {

                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"sudo /sbin/shutdown -h now\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                }


                //Process.Start("sudo shutdown", "now");
                return Json(true);

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        public async Task<JsonResult> RestartDevice()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"sudo /sbin/reboot\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                }

                //Process.Start("sudo reboot");
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<JsonResult> CheckPin(string Pin)
        {
            var Setingdb = await SettingDB.Instance;
            var seting = await Setingdb.CheckItemAsync();
            if (seting.pin.ToString() == Pin)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        public async Task<JsonResult> ChangePin(string NewPin, int OldPin = 0)
        {
            var Setingdb = await SettingDB.Instance;
            var seting = await Setingdb.CheckItemAsync();
            if (seting.pin == OldPin)
            {
                seting.pin = Convert.ToInt32(NewPin);
                await Setingdb.SaveItemAsync(seting);
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        public async Task<JsonResult> GetLocalIp(string Pin)
        {
            return Json(MyDevices.GetLocalIPAddress());
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [HttpPost]
        public JsonResult ReconnectWifi()
        {
            var result = ExecutePythonScript("/var/www/html/Main/wwwroot/connect_wifi.py");
            return Json(result);
        }

        private string ExecutePythonScript(string scriptPath)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "/usr/bin/python3",  // مسیر به پایتون
                    Arguments = scriptPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    process.WaitForExit();
                    string result = process.StandardOutput.ReadToEnd();
                    string errors = process.StandardError.ReadToEnd();

                    if (!string.IsNullOrEmpty(errors))
                    {
                        throw new Exception(errors);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }







        [HttpPost]
        public async Task<JsonResult> SetRotation([FromBody] RotationModel model)
        {
            string configPath = "/boot/config.txt";
            string rotationValue = GetRotationValue(model.Rotation);

            if (rotationValue != null)
            {
                var lines = await System.IO.File.ReadAllLinesAsync(configPath);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("display_rotate") || lines[i].StartsWith("lcd_rotate"))
                    {
                        lines[i] = "lcd_rotate=" + rotationValue;
                    }
                }

                await System.IO.File.WriteAllLinesAsync(configPath, lines);

                // ری‌استارت رزبری پای
                RebootSystem();

                return Json(new { message = "Rotation applied successfully! The system is rebooting." });
            }

            return Json(new { message = "Invalid rotation value." });
        }

        [HttpPost]
        public async Task<JsonResult> SetLcdSize([FromBody] LcdModel model)
        {
            string configPath = "/boot/config.txt";
            string lcdSettings = GetLcdSettings(model.LcdSize);

            if (lcdSettings != null)
            {
                var lines = await System.IO.File.ReadAllLinesAsync(configPath);
                // Add or modify LCD size-specific settings
                lines = AddOrUpdateLcdSettings(lines, lcdSettings);

                await System.IO.File.WriteAllLinesAsync(configPath, lines);

                // ری‌استارت رزبری پای
                RebootSystem();

                return Json(new { message = "LCD size applied successfully! The system is rebooting." });
            }

            return Json(new { message = "Invalid LCD size." });
        }

        private string GetRotationValue(string rotation)
        {
            return rotation switch
            {
                "0" => "0",
                "1" => "1",
                "2" => "2",
                "3" => "3",
                _ => null
            };
        }

        private string GetLcdSettings(string lcdSize)
        {
            return lcdSize switch
            {
                "240" => "hdmi_cvt=320 240 60 1 0 0 0",
                "280" => "hdmi_cvt=640 480 60 1 0 0 0",
                "330" => "hdmi_cvt=800 480 60 1 0 0 0",
                "350" => "hdmi_cvt=800 480 60 1 0 0 0",
                "400" => "hdmi_cvt=854 480 60 1 0 0 0",
                "500" => "hdmi_cvt=800 480 60 1 0 0 0",
                "700" => "hdmi_cvt=1024 600 60 1 0 0 0",
                "1000" => "hdmi_cvt=1280 800 60 1 0 0 0",
                _ => null
            };
        }

        private string[] AddOrUpdateLcdSettings(string[] lines, string lcdSettings)
        {
            bool found = false;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("hdmi_cvt"))
                {
                    lines[i] = lcdSettings;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                var newList = new List<string>(lines) { lcdSettings };
                return newList.ToArray();
            }

            return lines;
        }

        private void RebootSystem()
        {
            var psi = new ProcessStartInfo("sudo", "reboot")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = psi };
            process.Start();
        }








        private async Task<string> RunPythonScriptAsync(string arguments)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "python3",
                Arguments = $"/path/to/your_script.py {arguments}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                using (var reader = process.StandardOutput)
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }



        [HttpPost]
        //[HttpPost("control")]
        public async Task<IActionResult> LedControll(int pin, string status)
        {
            string command = $"python3 /var/www/html/Main/ledmanager.py {pin} {status}";
            ProcessStartInfo processStartInfo = new ProcessStartInfo("bash", $"-c \"{command}\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(processStartInfo))
            {
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return Content(result);
            }
        }


        [HttpPost]
        //[HttpPost("control")]
        public async Task<JsonResult> ControlDevice([FromBody] DeviceControlRequest request)
        {
            if (request == null)
                return Json(BadRequest("Invalid request"));

            string arguments = $"{request.Option} {request.Status} {request.FanNumber} {request.FanSpeed}";
            var result = await RunPythonScriptAsync(arguments);

            return Json(result);
        }


        // متدی برای دریافت اطلاعات دمای CPU، استفاده از CPU و RAM
        //[HttpGet("get-system-info")]

        [HttpPost]
        public async Task<JsonResult> GetSystemInfo()
        {
            var systemInfo = new
            {
                CpuUsage = GetCpuUsageLinux(),
                //CpuUsage = GetCpuUsage(),
                RamUsage = GetRamUsageLinux(),
                //RamUsage = GetRamUsage(),
                Temperature = GetCpuTemperatureLinux()
                //Temperature = GetCpuTemperature()
            };

            return Json(systemInfo);
        }

        // تابعی برای دریافت دمای CPU
        private string GetCpuTemperature()
        {
            try
            {
                string temperature = "";
                using (var searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        double temp = Convert.ToDouble(obj["CurrentTemperature"].ToString());
                        temperature = ((temp - 2732) / 10.0).ToString();
                    }
                }
                return temperature ;
            }
            catch
            {
                return "0";
            }
        }
        private int GetCpuTemperatureLinux()
        {
            try
            {
                // خواندن فایل دمای پردازنده
                string tempString = System.IO.File.ReadAllText("/sys/class/thermal/thermal_zone0/temp");

                // تبدیل به عدد صحیح
                int temp = int.Parse(tempString.Trim()) / 1000; // تبدیل میلی‌درجه به سانتیگراد

                return temp;
            }
            catch
            {
                return 0; // در صورت بروز خطا، مقدار 0 برگردانده می‌شود
            }
        }

        // تابعی برای دریافت میزان استفاده از CPU
        private string GetCpuUsage()
        {
            try
            {

                var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                cpuCounter.NextValue();
                System.Threading.Thread.Sleep(1000); // باید یک تاخیر برای اندازه‌گیری دقیق گذاشته شود
                string input = cpuCounter.NextValue().ToString();
                string[] parts = input.Split('٫');  // تقسیم رشته بر اساس کاما
                int result = int.Parse(parts[0]);    // تبدیل قسمت اول به int

                return result.ToString();
                //return cpuCounter.NextValue() + " %";
            }
            catch
            {
                return "0";
            }
        }

        private double GetCpuUsageLinux()
        {
            try
            {
                string[] cpuInfoLines = System.IO.File.ReadAllLines("/proc/stat");
                foreach (string line in cpuInfoLines)
                {
                    if (line.StartsWith("cpu "))
                    {
                        return ParseCpuUsage(line);
                    }
                }
                return 0.0;
            }
            catch
            {
                return 0.0;
            }
        }

        private double ParseCpuUsage(string cpuLine)
        {
            string[] parts = cpuLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 5)
            {
                return 0.0;
            }

            // پردازش مقادیر زمان‌های مختلف CPU
            long user = long.Parse(parts[1]);
            long nice = long.Parse(parts[2]);
            long system = long.Parse(parts[3]);
            long idle = long.Parse(parts[4]);

            long totalIdleTime = idle;
            long totalCpuTime = user + nice + system + idle;

            // ذخیره‌ی مقادیر قبلی برای محاسبه تفاوت
            long prevTotalCpuTime = 0;
            long prevTotalIdleTime = 0;

            long totalCpuDiff = totalCpuTime - prevTotalCpuTime;
            long idleCpuDiff = totalIdleTime - prevTotalIdleTime;

            prevTotalCpuTime = totalCpuTime;
            prevTotalIdleTime = totalIdleTime;

            if (totalCpuDiff == 0)
            {
                return 0.0;
            }

            // محاسبه درصد استفاده از CPU
            double usagePercentage = (1.0 - ((double)idleCpuDiff / totalCpuDiff)) * 100.0;
            return usagePercentage;
        }
        // تابعی برای دریافت میزان استفاده از RAM
        //private string GetRamUsage()
        //{
        //    var ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        //    var ramCom = new PerformanceCounter("Memory", "Committed MBytes");
        //    var result = (int)ramCounter.NextValue() / (int)ramCom.NextValue();
        //    return (ramCounter.NextValue()).ToString();
        //    //return (ramCounter.NextValue()).ToString() + " MB available";
        //} 
        private int GetRamUsage()
        {
            try
            {

                var ramAvailableCounter = new PerformanceCounter("Memory", "Available MBytes");  // رم قابل استفاده
                var ramTotalCounter = new PerformanceCounter("Memory", "Committed Bytes");       // کل رم اختصاص داده شده

                // گرفتن مقدار رم در دسترس و کل رم
                float availableRam = ramAvailableCounter.NextValue(); // رم در دسترس بر حسب مگابایت
                float totalRam = ramTotalCounter.NextValue() / (1024 * 1024); // کل رم بر حسب مگابایت (بایت به مگابایت)

                // محاسبه درصد رم مصرف شده
                float usedRamPercentage = ((totalRam - availableRam) / totalRam) * 100;

                // تبدیل به عدد صحیح
                return (int)usedRamPercentage;
            }
            catch
            {
                return 0;
            }
        }

        private int GetRamUsageLinux()
        {
            try
            {
                string[] memInfoLines = System.IO.File.ReadAllLines("/proc/meminfo");

                long totalRam = 0;
                long freeRam = 0;
                long availableRam = 0;

                foreach (string line in memInfoLines)
                {
                    if (line.StartsWith("MemTotal:"))
                    {
                        totalRam = ParseMemInfoLine(line);
                    }
                    else if (line.StartsWith("MemFree:"))
                    {
                        freeRam = ParseMemInfoLine(line);
                    }
                    else if (line.StartsWith("MemAvailable:"))
                    {
                        availableRam = ParseMemInfoLine(line);
                    }

                    if (totalRam > 0 && availableRam > 0)
                    {
                        break;
                    }
                }

                if (totalRam > 0 && availableRam > 0)
                {
                    // محاسبه درصد رم مصرف شده
                    float usedRamPercentage = ((float)(totalRam - availableRam) / totalRam) * 100;

                    // تبدیل به عدد صحیح
                    return (int)usedRamPercentage;
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private long ParseMemInfoLine(string line)
        {
            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2 && long.TryParse(parts[1], out long value))
            {
                return value;
            }
            return 0;
        }



        [HttpPost]
        public async Task<JsonResult> ControlLED([FromBody] LedRequest request)
        {
            try
            {
                RunLEDScript(request.Option, request.Status);
                return Json(new { message = "LED status updated successfully" });
            }
            catch (Exception ex)
            {
                return Json(BadRequest(new { error = ex.Message }));
            }
        }

        static void RunLEDScript(string option, string status)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "/usr/bin/python3",
                Arguments = $"/var/www/html/Main/wwwroot/led_control.py {option} {status}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                process.WaitForExit();
            }
        }

        public class LedRequest
        {
            public string Option { get; set; }
            public string Status { get; set; }
        }
















        public IActionResult StartService()
        {
            return ExecuteSystemctlCommand("start rgb-dance.service");
        }

        public IActionResult StopService()
        {
            return ExecuteSystemctlCommand("stop rgb-dance.service");
        }

        // متد عمومی برای اجرای دستورات systemctl
        private IActionResult ExecuteSystemctlCommand(string command)
        {
            try
            {
                // ساخت یک پروسه برای اجرای دستورات
                Process process = new Process();
                process.StartInfo.FileName = "/bin/bash";
                process.StartInfo.Arguments = $"-c \"sudo systemctl {command}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();

                return Ok($"Service {command} executed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }








        private static readonly HttpClient fanclient = new HttpClient();

        [HttpPost]
        public async Task SetFanMode(string mode)
        {
            //var url = "http://localhost:4000/fan";
            //var jsonContent = new StringContent($"{{\"mode\":\"{mode}\"}}", Encoding.UTF8, "application/json");

            //var response = await fanclient.PostAsync(url, jsonContent);
            //if (response.IsSuccessStatusCode)
            //{
            //    Console.WriteLine($"Fan set to {mode} mode.");
            //}
            //else
            //{
            //    Console.WriteLine($"Failed to set fan mode: {response.StatusCode}");
            //}
        }









    }

    public class DeviceControlRequest
    {
        public string Option { get; set; }
        public string Status { get; set; }
        public int FanNumber { get; set; }
        public int FanSpeed { get; set; }
    }
    public class RotationModel
    {
        public string Rotation { get; set; }
    }

    public class LcdModel
    {
        public string LcdSize { get; set; }
    }

}
