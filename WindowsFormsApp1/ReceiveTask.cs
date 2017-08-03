using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    /* クライアントからの受信待ちする */
    public class ReceiveTask
    {
        private AsyncTcpListener Listener;
        Hashtable ht = new Hashtable();
        Boolean ShiftOnOff=false;
        /* コンストラクタ */
        public ReceiveTask(AsyncTcpListener Listener)
        {
            ht.Add("<ENT>", "{ENTER}");
            ht.Add("<DEL>", "{DELETE}");
            ht.Add("<BAC>", "{BACKSPACE}");
            ht.Add("<CON>", "<CON>");
            ht.Add("<UPP>", "{UP}");
            ht.Add("<DOW>", "{DOWN}");
            ht.Add("<RIG>", "{RIGHT}");
            ht.Add("<LEF>", "{LEFT}");
            ht.Add("<COP>", "^c");
            ht.Add("<ALL>", "^a");
            ht.Add("<CUT>", "^x");
            ht.Add("<PAS>", "^v");
            ht.Add("<SAV>", "^s");
            ht.Add("<OPN>", "^o");
            ht.Add("<BEF>", "^z");
            ht.Add("<AFT>", "^y");
            ht.Add("<SEA>", "^f");
            ht.Add("<NEW>", "^n");
            ht.Add("<END>", "<END>");
            ht.Add("<TAB>", "%");
            ht.Add("<CTA>", "^%{TAB}");
            ht.Add("<SHI>", "+");
            ht.Add("+", "{+}");
            ht.Add("^", "{^}");
            ht.Add("%", "{%}");

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
            await Task.Run(async () =>
            {
                while (true)
                {
                    /* クライアントからの受信データを取得 */
                    string data = await Listener.Receive();

                    /* 受信したデータがない場合 */
                    if (data.Equals("<NONE>"))
                    {
                        continue;
                    }
                    /* タグが含まれていた場合 */
                    else if (data.StartsWith("<") && ht.ContainsKey(data.Substring(0, 5)))
                    {
                        /* クライアント側が切断要求をした場合 */
                        if (((string)ht[data]) == ("<END>"))
                        {
                            break;
                        }
                        /* クライアントが文字変換をした場合 */
                        else if (data.StartsWith("<") && data.Substring(0, 5) == "<CON>")
                        {
                            /* 受信データ分割 */
                            string[] work = Microsoft.VisualBasic.Strings.Split(data, "<>");

                            /* カーソル移動用 */
                            int rCount = int.Parse(work[1]);
                            /* 文字削除用 */
                            int bCount = int.Parse(work[2]);
                            /* カーソルを戻す用 */
                            int mCount = 0;

                            /* 変換対象の文字の最後尾までカーソルを移動 */
                            for (int i = 0; i < rCount; i++)
                            {
                                SendKeys.SendWait("{LEFT}");
                                mCount++;
                            }

                            /* 変換対象の文字を削除 */
                            for (int i = 0; i < bCount; i++)
                            {
                                SendKeys.SendWait("{BACKSPACE}");
                            }

                            /* 変換後の文字を挿入 */
                            SendKeys.SendWait(Escape(work[3]));
                            
                            /* カーソルをもとの位置まで戻す */
                            for (int i = 0; i < mCount; i++)
                            {
                                SendKeys.SendWait("{RIGHT}");
                            }
                        }
                        /* クライアントがバックスペースをした場合 */
                        else if (data.StartsWith("<") && data.Substring(0, 5) == "<BAC>")
                        {
                            /* 受信データを分割 */
                            string key = (string)ht[data.Substring(0, 5)];

                            /* バックスペースを押す回数 */
                            int count = int.Parse(Microsoft.VisualBasic.Strings.Split(data, "<>")[1]);

                            /* 受信した回数分だけバックスペースをさせる */
                            for (int a = 0; a < count; a++)
                            {
                                SendKeys.SendWait(key);
                            }
                        }
                        else if (data.StartsWith("<") && data.Substring(0, 5) == "<SHI>")
                        {
                            if (ShiftOnOff == false)
                            {
                                ShiftOnOff = true;
                            }
                            else
                            {
                                ShiftOnOff = false;
                            }

                        }
                        else if (data.StartsWith("<") && (data.Substring(0, 5) == "<UPP>" ||
                                                            data.Substring(0, 5) == "<DOW>" ||
                                                            data.Substring(0, 5) == "<RIG>" ||
                                                            data.Substring(0, 5) == "<LEF>" ||
                                                            data.Substring(0, 5) == "<TAB>"))
                        {
                            if (ShiftOnOff == false)
                            {
                                SendKeys.SendWait(((string)ht[data]));
                            }
                            else
                            {
                                SendKeys.SendWait(("+" + (string)ht[data]));

                            }
                        }
                        /* その他のコマンド(ショートカットなど)が送られてきた場合 */
                        else
                        {
                            SendKeys.SendWait(((string)ht[data]));
                        }
                    }
                    else
                    {
                        /* 
                         * エスケープ処理*/
                        if (data == "+" || data == "^"|| data == "%") {
                            SendKeys.SendWait(((string)ht[data]));
                        }

                        /*
                         * アクティブウィンドウに文字を送る
                         */
                        else
                        {
                            SendKeys.SendWait(Escape(data));
                        }
                    }
                }
            });
            Listener.ClientDisconnect();

        }
        public String Escape(String data)
        {
            try
            {
                data.Replace("+", "{+}");
                data.Replace("^", "{^}");
                data.Replace("(", "{(}");
                data.Replace(")", "{)}");
            }
            catch(Exception)
            {

            }

            return data;

        }
    }
}