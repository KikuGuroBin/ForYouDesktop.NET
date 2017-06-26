using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace WindowsFormsApp1
{
    class IP_get_class
    {
        public  IP_get_class()
        {
        }
        public IPAddress Ipget()
        {
            //ListenするIPアドレス
            string ipString = "10.4.1.131";
            IPAddress ipAdd = IPAddress.Parse(ipString);
            IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in addresses)
            {
                // IPv4 のみを追加する
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipAdd = address;
                }
            }
            return ipAdd;
        }
    }
}
