using System.Net;

namespace WindowsFormsApp1
{
    class IP_get_class
    {
        public IPAddress Ipget()
        {
            //ListenするIPアドレス
            IPAddress ipAdd = null;
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
