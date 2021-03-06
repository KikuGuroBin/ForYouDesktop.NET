﻿using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class AsyncTcpListener
    {
        public Form1 form;

        /* Listenするポート番号定数 */
        private const int PORT = 55555;
        private TcpListener Listener;
        private TcpClient Client;

        private Queue ReceiveDatas;

        /* ソケット通信におけるListen用 */
        public async void Listen()
        {
            IPAddress ipAdd = new IP_get_class().Ipget();
           
            Debug.WriteLine(ipAdd.ToString());
            Listener = new TcpListener(ipAdd, PORT);

            /* Listenを開始する */
            Listener.Start();
            Debug.WriteLine("Listenを開始しました({0}:{1})。",
                ((IPEndPoint)Listener.LocalEndpoint).Address,
                ((IPEndPoint)Listener.LocalEndpoint).Port);

            /* 接続要求があったら受け入れる */
            Client = await Listener.AcceptTcpClientAsync();
            Debug.WriteLine("クライアント({0}:{1})と接続しました。",
                ((IPEndPoint)Client.Client.RemoteEndPoint).Address,
                ((IPEndPoint)Client.Client.RemoteEndPoint).Port);

            /* Receive(); */

            /* Send("OK"); */

            Debug.WriteLine("----------------接続OK----------------");
            form.notifyIcon1.BalloonTipText = "接続に成功しました";
            form.notifyIcon1.ShowBalloonTip(500);

            form.notifyIcon1.BalloonTipText = "★接続中です★\n①いつも通りスマホのキーボードで入力が可能です。\n②終了の際は右下ツールバーにアイコンを右クリックして終了を押してください。";
            
            form.ShowInTaskbar = false;
            form.Hide();

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
            NetworkStream ns = Client.GetStream();
            /* タイムアウト設定 */
            ns.WriteTimeout = 30000;

            /* 文字列をByte型配列に変換 */
            byte[] sendBytes = Encoding.UTF8.GetBytes(data);
            /* データを送信する */
            ns.Write(sendBytes, 0, sendBytes.Length);

            //ns.Close();
        }

        /* 受信用 */
        public async Task<string> Receive()
        {
            /* NetworkStreamを取得 */
            NetworkStream ns = Client.GetStream();
            /* タイムアウト設定 */
            ns.ReadTimeout = 30000;

            /* 受信データの一時格納用 */
            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte[1024];
            int bytes = 0;

            /* 戻り値にする受信データ格納用 */
            string read = "<NONE>";

            /* 読み取り可能なデータ、または読み取れるデータがある場合は受信を続ける 
            while (ns.DataAvailable)
            {
                /* データ受信 
                bytes = await ns.ReadAsync(buffer, 0, buffer.Length);

                /* 受信したデータを蓄積する
                ms.Write(buffer, 0, bytes);
            }
            */

            /* 読み取り可能なデータが確認できるまで待つ */
            await CheckStream(ns);

            /* データ受信 */
            bytes = await ns.ReadAsync(buffer, 0, buffer.Length);

            /* 受信したデータを蓄積する */
            ms.Write(buffer, 0, bytes);

            /* 受信したデータを文字列に変換し格納 */
            if (bytes > 0)
            {
                read = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
            }

            ms.Close();

            return read;
        }

        private async Task<bool> CheckStream(NetworkStream ns)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    /* 読み取り可能なデータが確認できたらループを抜ける */
                    if (ns.DataAvailable)
                    {
                        break;
                    }
                }
            });

            return false;
        }

        private async void ChargeReceiveData()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    string data = await Receive();

                    if (data.Contains("</>"))
                    {

                    }
                    else
                    {

                    }
                }
            });
        }

        public void HostDisconnect()
        {
            /* クライアント側に接続終了のタグを送る */
            Send("<END>");
        }

        public void ClientDisconnect()
        {
            /* 閉じる */
            Client.Close();
            Listener.Stop();
            Listener = null;
            form.notifyIcon1.BalloonTipText = "切断されました";
            form.notifyIcon1.ShowBalloonTip(500);
            form.ShowInTaskbar = true;
            form.WindowState = System.Windows.Forms.FormWindowState.Normal;
            form.Show();
            Listen();
            //form.Visible = true;
            //form.pictureBox1.Image = (new QR_Creater()).CrGet(((new IP_get_class()).Ipget()).ToString());


            /* 
             * QRコード表示
             */
        }
    }
}