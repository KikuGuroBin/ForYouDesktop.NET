using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections;

namespace WindowsFormsApp1
{
    /* クライアントからの受信待ちする */
    public class ReceiveTask
    {
        private AsyncTcpListener Listener;
        Hashtable ht = new Hashtable();
        /* コンストラクタ */
        public ReceiveTask(AsyncTcpListener Listener)
        {
            ht.Add("<ENT>", "{ENTER}");
            ht.Add("<DEL>", "{BACKSPACE}");
            ht.Add("<CON>", "<CONV>");
            ht.Add("<COP>", "^c");
            ht.Add("<CUT>", "^x");
            ht.Add("<PAS>", "^v");
            ht.Add("<END>", "< ENDCONNECTION >");

            /*
            ht.Add("{BACKSPACE}", "{BACKSPACE}");

            
            ht.Add("{UP}", "{UP}");
            ht.Add("{DOWN}", "{DOWN}");
            ht.Add("{RIGHT}", "{RIGHT}");
            ht.Add("{LETF}", "{LEFT}");
            ht.Add("{TUB}", "{TAB}");
           
            ht.Add("{SAVEADD}", "{BACKSPACE}");
            ht.Add("{SAVE}", "{BACKSPACE}");
            ht.Add("{SEARCH}", "{BACKSPACE}");
            ht.Add("{BACK}", "{BACKSPACE}");
            ht.Add("{ALT}", "{BACKSPACE}");
            ht.Add("{RENEW}", "{BACKSPACE}");
            ht.Add("{RENEW}", "{BACKSPACE}");
            ht.Add("{RENEW}", "{BACKSPACE}");
            ht.Add("{RENEW}", "{BACKSPACE}");
            ht.Add("{RENEW}", "{BACKSPACE}");
            ht.Add("{RENEW}", "{BACKSPACE}");
             
             
             */


            this.Listener = Listener;
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
                    string data = Listener.Receive();
                    /*タグが含まれていた場合*/
                    if (ht.ContainsKey(data))
                    {
                        /* クライアント側が切断要求をした場合 */
                        if (((string)ht[data]).IndexOf("<ENDCONNECTION>") > -1)
                        {
                            /* 
                             * 切断処理 
                             * ↓
                             * QRの表示
                             */

                            break;
                        }else if (((string)ht[data]).Equals("<CONV>"))
                        {

                        }
                        else
                        {
                            System.Windows.Forms.SendKeys.SendWait((string)ht[data]);
                        }

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
                         * 
                         * アクティブウィンドウに文字を送る
                         */
                        // Microsoft.VisualBasic.Interaction.AppActivate("TeraPad");
                        

                        System.Windows.Forms.SendKeys.SendWait(data);
                        /* デバッグ用 */
                        Debug.WriteLine("--------------------受信したデータ--------------------");
                        Debug.WriteLine(data);
                    }
                }
            });
            Debug.WriteLine("あああああああああああああああああああああああああ");
            Listener.ClientDisconnect();

        }
    }
}