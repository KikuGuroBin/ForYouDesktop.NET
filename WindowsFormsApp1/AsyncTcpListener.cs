using System.IO;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApp1
{
    public class AsyncTcpListener
    {
        
        /* Listenするポート番号定数 */
        private const int PORT = 55555;

        private TcpListener listener;
        private TcpClient client;

        /* ソケット通信におけるListen用 */
        public async void Listen()
        {
            IPAddress ipAdd = new IP_get_class().Ipget();

            Debug.WriteLine(ipAdd.ToString());
            //ホスト名からIPアドレスを取得する時は、次のようにする
            //string host = "localhost";
            //System.Net.IPAddress ipAdd =
            //    System.Net.Dns.GetHostEntry(host).AddressList[0];
            //.NET Framework 1.1以前では、以下のようにする
            //System.Net.IPAddress ipAdd =
            //    System.Net.Dns.Resolve(host).AddressList[0];
            
            /* TcpListenerオブジェクトを作成する */
            listener = new TcpListener(ipAdd, PORT);

            /* Listenを開始する */
            listener.Start();
            Debug.WriteLine("Listenを開始しました({0}:{1})。",
                ((IPEndPoint)listener.LocalEndpoint).Address,
                ((IPEndPoint)listener.LocalEndpoint).Port);

            /* 接続要求があったら受け入れる */
            client = await listener.AcceptTcpClientAsync();
            Debug.WriteLine("クライアント({0}:{1})と接続しました。",
                ((IPEndPoint)client.Client.RemoteEndPoint).Address,
                ((IPEndPoint)client.Client.RemoteEndPoint).Port);
            
            /* Receive(); */

            /* Send("OK"); */

            Debug.WriteLine("----------------接続OK----------------");

            /* クライアントの受信待ち状態に移行 */
            new ReceiveTask(this).Receiver();
        }

        /* 
         * 送信用
         * 接続時とホスト側切断時にしか呼ばれない 
         */
        public void Send(string data)
        {
            /* NetworkStreamを取得 */
            NetworkStream ns = client.GetStream();
            /* タイムアウト設定 */
            ns.WriteTimeout = 30000;

            /* 文字列をByte型配列に変換 */
            byte[] sendBytes = Encoding.UTF8.GetBytes(data);
            /* データを送信する */
            ns.Write(sendBytes, 0, sendBytes.Length);
            
            ns.Close();
        }

        /* 受信用 */
        public string Receive()
        {
            /* NetworkStreamを取得 */
            NetworkStream ns = client.GetStream();
            /* タイムアウト設定 */
            ns.ReadTimeout = 30000;

            /* 受信データの一時格納用 */
            MemoryStream ms = new MemoryStream();

            byte[] resBytes = new byte[256];

            /* データの一部を受信する */
            int resSize = 0;

            /* 戻り値にする受信データ格納用 */
            string resMsg = "NONE";
            
            /* 読み取り可能なデータ、または読み取れるデータがある場合は受信を続ける */
            while(ns.DataAvailable)
            {
                /* 再びデータを受信する */
                resSize = ns.Read(resBytes, 0, resBytes.Length);

                /* 受信したデータを蓄積する */
                ms.Write(resBytes, 0, resSize);
            }

            /* 受信したデータを文字列に変換し格納 */
            if (resSize > 0)
            {
                resMsg = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
            }

            ms.Close();

            return resMsg;
        }

        public void HostDisConnect()
        {
            /* クライアント側に接続終了のタグを送る */
            Send("<ENDCONNECTION>");
        }

        public void ClientDisconnect()
        {
            /* 閉じる */
            client.Close();

            /* 
             * QRコード表示
             */
        }
    }
}
