//using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SpCoreMiner.Services
{
    public class PingIp
    {
        public async Task<string> server()
        {
            //var isReachable = await CrossConnectivity.Current.IsReachable("google.com", 5000);

            string ttls = "";
            try
            {
                System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingReply rep = await p.SendPingAsync("google.com");
                await Task.Delay(5000);
                if (rep.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    ttls = rep.Address.ToString();
                    Debug.WriteLine("Ping result: " + rep.Address);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ping result: " + ex.Message);
            }



            return ttls;
        }
    }
}
