using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Main
    {
        private AsyncTcpListener Listener;

        public void Start()
        {
            Listener = new AsyncTcpListener();
            Listener.Listen();
        }
    }
}
