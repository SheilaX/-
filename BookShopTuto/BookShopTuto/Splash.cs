﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Automation;

namespace BookShopTuto
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
        int startpos = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            startpos += 10;
            Myprogress.Value = startpos;
            PercentageLbl.Text = startpos + "%";
            if(Myprogress.Value==100)//进入登陆界面
            {
                Myprogress.Value = 0;
                timer1.Stop();
                Login log = new Login();
                log.Show();
                this.Hide();
            }
        }
    }
}
