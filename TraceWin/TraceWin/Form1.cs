﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Threading;
using System.Windows.Forms;
using TraceWinResources;
using System.Security.Principal;

namespace TraceWin
{
    public partial class Form1 : Form
    {

        private string xmlPath = null;

        public Form1()
        {
            InitializeComponent();
            
            button1.Enabled = false;
            button2.Enabled = false;
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            bool IsAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

            if(!IsAdmin) MessageBox.Show("For a complete result you MUST run this application as Administrator!");

            
        }

        

        

        private void button1_Click_1(object sender, EventArgs e)
        {
            String emsg = "Invalid xml config file";
            XmlUtils xmlUtil = new XmlUtils();

            Config config = null;
            
            try
            {
                config = xmlUtil.readConfig(this.xmlPath);
            }catch(Exception e1){
                FileHelper.WriteLog("ERROR = " + emsg + "[" + e1.Message + "]");
            }
            
            if (config != null)
            {
                try
                {
                    Coordinator worker = new Coordinator(config);
                    Thread jobThread = new Thread(new ThreadStart(worker.TraceJob));
                    jobThread.Start();
                }
                catch (Exception e2)
                {
                    FileHelper.WriteLog("ERROR = " + emsg + "[" + e2.Message + "]");
                }
                button1.Text = "running in loop...";
                button1.Enabled = false;
                button2.Enabled = true;
                pictureBox1.Image = TraceWin.Properties.Resources.progress2;
            }
            else
            {
                
                FileHelper.WriteLog("ERROR = " + emsg);
                MessageBox.Show(emsg);

            }
            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                MessageBox.Show(sr.ReadToEnd(), "TraceWin Config file");
                sr.Close();
                this.xmlPath = openFileDialog1.FileName;
                button1.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = Constants.APP_NAME + " " + Constants.APP_VERSION;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://infoserver.com.br/oss"); 
        }

        
    }
}
