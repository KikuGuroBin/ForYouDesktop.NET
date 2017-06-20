using System;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace WindowsFormsApp1
{
    /* ホストからの接続可否 */
    public class Communication
    {
        /* メインスレッドのマニュアル制御用 */
        private ManualResetEvent receiveDone = new ManualResetEvent(false);
        /* ホストから受信したデータ格納用 
         * 初期値"NONE"*/
        private string response = "NONE";

        /* データ受信の流れをまとめたもの */
        public string ConnectWait(Socket client)
        {
            Receive(client);
            receiveDone.WaitOne();

            return response;
        }

        /* ホストからのデータ受信*/
        private void Receive(Socket client)
        {
            try
            {
                /* 構造体生成 */
                State state = new State();
                state.workSocket = client;

                /* ホストからのデータを非同期で受信 */
                client.BeginReceive(state.buffer, 0, State.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        /* ホストからのデータ受信時のコールバック*/
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                State state = (State)ar.AsyncState;
                Socket client = state.workSocket;

                /* ホストからのデータを取得 */
                int byteRead = client.EndReceive(ar);

                if (byteRead > 0)
                {
                    /* StateのStringBilderに取得したデータを格納 */
                    state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, byteRead));
                    /* 残りのデータを取得 */
                    client.BeginReceive(state.buffer, 0, State.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                        Debug.WriteLine(response);
                    }
                    /* メインスレッド再開 */
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }
    }
}

