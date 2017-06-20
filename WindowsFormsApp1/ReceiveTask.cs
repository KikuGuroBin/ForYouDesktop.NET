using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    /* クライアントからの受信待ちする */
    public class ReceiveTask
    {
        private AsyncTcpListener Listener;

        /* コンストラクタ */
        public ReceiveTask(AsyncTcpListener Listener)
        {
            this.Listener = Listener;
        }

        /* 非同期で受信待ちし、受信したらそれに応じた処理を行う */
        public async void Receive()
        {
            /* 人工的な無限ループのスレッド */
            await Task.Run(() =>
            {
                while (true)
                {
                    /* クライアントからの受信データを取得 */
                    string data = Listener.Receive();

                    /* クライアント側が切断要求をした場合 */
                    if (data.IndexOf("<ENDCONNECTION>") > -1)
                    {
                        /* 
                         * 切断処理 
                         * ↓
                         * QRの表示
                         */
                    }
                    /* 受信したデータがない場合 */
                    else if (data.Equals("NONE"))
                    {

                    }
                    else
                    {
                        /* 
                         * エスケープ処理
                         * ↓
                         * アクティブウィンドウに文字を送る
                         */
                    }
                }
            });
        }
    }
}
