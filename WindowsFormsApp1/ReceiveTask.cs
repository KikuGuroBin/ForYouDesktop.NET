using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    /* クライアントからの受信待ちする */
    public class ReceiveTask
    {
        private AsyncTcpListener listener;

        /* コンストラクタ */
        public ReceiveTask(AsyncTcpListener listener)
        {
            this.listener = listener;
        }

        /* 非同期で受信待ちし、受信したらそれに応じた処理を行う */
        public async void Receiver()
        {
            /* 人工的な無限ループのスレッド */
            await Task.Run(() =>
            {
                while (true)
                {
                    /* クライアントからの受信データを取得 */
                    string data = listener.Receive();

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
                        continue;
                    }
                    else
                    {
                        /* 
                         * エスケープ処理
                         * ↓
                         * アクティブウィンドウに文字を送る
                         */

                        /* デバッグ用 */
                        Debug.WriteLine("--------------------受信したデータ--------------------");
                        Debug.WriteLine(data);
                    }
                }
            });
        }
    }
}