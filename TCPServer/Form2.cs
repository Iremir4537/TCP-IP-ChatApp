﻿using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
       
        SimpleTcpServer server; 
        private void btnStart_Click(object sender, EventArgs e)
        {
            

            server.Start();
            txtInfo.Text += $"Starting...{Environment.NewLine}";
            btnStart.Enabled = false;
            btnSend.Enabled = true;
            btnStop.Enabled = true;    
            
              
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            
            
            listClientIP.Items.Clear();
            txtInfo.Text += $"Ending session...{Environment.NewLine}";
            btnStop.Enabled = false;
            btnStart.Enabled = true;
            btnSend.Enabled = false;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false; 
            server = new SimpleTcpServer(txtIP.Text);
            server.Events.ClientConnected += Events_ClientConnected;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            server.Events.DataReceived += Events_DataReceived;
              
            
            
        }



        private void Events_DataReceived(object? sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort}:  {Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";
            });
        }
        private void Events_ClientDisconnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} disconnected.{e.Reason}{Environment.NewLine}";
                listClientIP.Items.Remove(e.IpPort);
            });
        }

        private void Events_ClientConnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {

                txtInfo.Text += $"{e.IpPort} connected.{Environment.NewLine}";
                listClientIP.Items.Add(e.IpPort);
                

            }); 
              
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (server.IsListening)
            {
                //check messages and select client from listbox
                if(!string.IsNullOrEmpty(txtIP.Text) && listClientIP.SelectedItem != null)
                {
                    server.Send(listClientIP.SelectedItem.ToString(), txtMessage.Text);
                    txtInfo.Text += $"Server : {txtMessage.Text}{Environment.NewLine}";
                    txtMessage.Text = string.Empty;

                }
            }
        }

        
    }
}
