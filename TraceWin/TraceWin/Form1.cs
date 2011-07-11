using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            XmlUtils xmlUtil = new XmlUtils();
            Config config = xmlUtil.readConfig(this.xmlPath);
            Coordinator worker = new Coordinator(config);
            Thread jobThread = new Thread(new ThreadStart(worker.TraceJob));
            jobThread.Start();

            button1.Text = "running in loop...";
            button1.Enabled = false;
            button2.Enabled = true;
            pictureBox1.Image = TraceWin.Properties.Resources.progress2;

            
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
                MessageBox.Show(sr.ReadToEnd());
                sr.Close();
                this.xmlPath = openFileDialog1.FileName;
                button1.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        
    }
}
