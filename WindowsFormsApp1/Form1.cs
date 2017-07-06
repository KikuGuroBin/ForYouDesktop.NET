using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        AsyncTcpListener Listener;
        public Form1()
        {
            InitializeComponent();
            /* AsyncSocketListener.StartListening(); */
            var po = new Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 200, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 200);
            //this.Location = po;
            Debug.WriteLine(po);
            Debug.WriteLine(this.Location);
            this.pictureBox1.Image = (new QR_Creater()).CrGet(((new IP_get_class()).Ipget()).ToString());
            Listener = new AsyncTcpListener();
            Listener.form = this;
            Listener.Listen();
        }
        
    }
}
