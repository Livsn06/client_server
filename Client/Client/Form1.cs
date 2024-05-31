using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        private SimpleTcpClient _client;
        int port = 8910;


        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            btnDisconnect.Enabled = false;
            btnSend.Enabled = false;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                _client = new SimpleTcpClient().Connect("127.0.0.1", port);
                _client.Delimiter = 0x13; // Enter key
                _client.StringEncoder = Encoding.UTF8;
                _client.DataReceived += Client_DataReceived;
                btnDisconnect.Enabled = true;
                btnSend.Enabled = true;
                btnDisconnect.Enabled = true;
                btnConnect.Enabled = false;
                Log("Connected to server.");
            }
            catch (Exception ex)
            {
                Log("Server is not open.");
            }

        }

        private void Client_DataReceived(object sender, SimpleTCP.Message e)
        {
            Log("Server: " + e.MessageString);
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

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (_client != null && _client.TcpClient.Connected)
            {
                string message = text.Text;
                _client.WriteLineAndGetReply(message, TimeSpan.FromSeconds(3));
                Log("Message sent to server.");
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            _client.Disconnect();
            Log("Disconnected to server.");
        }
    }
}
