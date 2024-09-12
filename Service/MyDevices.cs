using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;


namespace SpCoreMiner.Serices
{
    public class MyDevices
    {
        public static string GetLocalIPAddress()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "/bin/bash",
                Arguments = "-c \"hostname -I\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    return output;
                }
                else
                {
                    throw new Exception("0.0.0.0");
                }
            }


            //var host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (var ip in host.AddressList)
            //{
            //    if (ip.AddressFamily == AddressFamily.InterNetwork)
            //    {
            //        return ip.ToString();
            //    }
            //}
            //throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public static string GetMacAddr()
        {
            string mod = GetMACAddresses();
            //string mod = DeviceInfo.Manufacturer;
            mod = mod.Replace("'", "");
            mod = mod.Replace(";", "");
            return mod;
        }
        public static string GetDeviceName()
        {
            string mod = GetMACAddresses();
            //string mod = DeviceInfo.Name;
            mod = mod.Replace("'", "");
            mod = mod.Replace(";", "");
            return mod;
        }

        static string GetMACAddresses()
        {
            string mc = null;
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var nic in nics)
            {
                var macAddress = nic.GetPhysicalAddress();
                if (!string.IsNullOrEmpty(macAddress.ToString()))
                {
                    mc = macAddress.ToString();
                    Console.WriteLine($"Interface: {nic.Name}, MAC Address: {macAddress}");
                }
            }
            return mc;
        }


        static string GetProcessorInfo()
        {
            var response = "";
            var searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (var obj in searcher.Get())
            {
                Console.WriteLine("Processor Name: " + obj["Name"]);
                Console.WriteLine("Number Of Cores: " + obj["NumberOfCores"]);
                Console.WriteLine("Processor ID: " + obj["ProcessorId"]);
                response = obj["Name"].ToString();
            }
            return response;
        }

        static string GetMemoryInfo()
        {
            var response = "";
            var searcher = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
            foreach (var obj in searcher.Get())
            {
                response = (Convert.ToDouble(obj["TotalPhysicalMemory"]) / 1048576).ToString();

            }
            return response;
        }
    }
}