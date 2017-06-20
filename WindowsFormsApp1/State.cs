using System.Text;
using System.Net.Sockets;

namespace WindowsFormsApp1
{
    /* Recive用構造体 */
    public class State
    {
        /* クライアント側ソケット格納用 */
        public Socket workSocket    = null;
        /* 受信データのサイズの定数*/
        public const int BufferSize = 1024;
        /* 受信データバッファー格納用*/
        public byte[] buffer        = new byte[BufferSize];
        /* 受信データ格納用 */
        public StringBuilder sb     = new StringBuilder();
    }
}
