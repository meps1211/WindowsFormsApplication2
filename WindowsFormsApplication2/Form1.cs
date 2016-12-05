using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using Ozeki.VoIP;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Program.InitSF();
            //MP3ToSpeaker();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string ToDail = textDail();
            textBox2Write(ToDail);
            Program.StartCall(ToDail);
            //Program.StartCall("039295559");
        }

        public void WriteMsg(string Msg)
        {
            textBox2Write(Msg);
        }

     
    }
}
