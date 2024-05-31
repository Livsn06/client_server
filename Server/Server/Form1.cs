using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Server
{
    public partial class Form1 : Form
    {

        int port = 8910;
        SimpleTcpServer _server;
        private SimpleTCP.Message _lastClient;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnSend.Enabled = false;
        }

        /* START SERVER */
        private void btnStart_Click(object sender, EventArgs e)
        {
            _server = new SimpleTcpServer().Start(port);
            _server.Delimiter = 0x13; // Enter key
            _server.StringEncoder = Encoding.UTF8;
            _server.DataReceived += Server_DataReceived;

            Log("Server started.");

            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnSend.Enabled = true;
        }



        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            _lastClient = e;
            Log($"Received from client: {e.MessageString}");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }



        private void btnStop_Click(object sender, EventArgs e)
        {
            _server.Stop();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            btnSend.Enabled = false;
            Log("Server stopped.");
        }

        private void Log(string message)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action<string>(Log), message);
            }
            else
            {
                richTextBox1.AppendText($"{message}{Environment.NewLine}");
            }
        }


        private void btnSend_Click_1(object sender, EventArgs e)
        {
            if (_lastClient != null)
            {
                string message = text.Text;
                _lastClient.ReplyLine(message);
                Log($"Sent to client: {message}");
            }
            else
            {
                Log("No client connected.");
            }
        }
    }
}
