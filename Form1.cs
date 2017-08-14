﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace LockSwitch
{
    public partial class Form1 : Form
    {
        private Boolean isCLocked;
        private Boolean isNLocked;
        private Boolean isSLocked;


        public Form1()
        {
            InitializeComponent();
            notifyIcon1.Icon = SystemIcons.Application;
            isCLocked = Control.IsKeyLocked(Keys.CapsLock);
            isNLocked = Control.IsKeyLocked(Keys.NumLock);
            isSLocked = Control.IsKeyLocked(Keys.Scroll);
            this.BackColor = Color.YellowGreen;
            button1.Text = "CAPS LOCK";
            button2.Text = "NUM LOCK";
            button3.Text = "SCROLL LOCK";
            timer1.Interval = 10;
            timer1.Start();
            


        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                if (!isCLocked) { 
                    notifyIcon1.Visible = true;
                    notifyIcon1.BalloonTipTitle = "MSI";
                    notifyIcon1.BalloonTipText = "Caps-Lock is on";
                    notifyIcon1.ShowBalloonTip(1);
                }
                button1.BackColor = Color.Green;
                label1.Text = "Caps-Lock On";
                isCLocked = true;
            }
            else if (!Control.IsKeyLocked(Keys.CapsLock))
            { 
                if (isCLocked) { 
                    notifyIcon1.Visible = true;
                    notifyIcon1.BalloonTipTitle = "MSI";
                    notifyIcon1.BalloonTipText = "Caps-Lock is off";
                    notifyIcon1.ShowBalloonTip(1);
                }
                button1.BackColor = Color.White;
                label1.Text = "Caps-Lock Off";
                isCLocked = false;
            }
            if (Control.IsKeyLocked(Keys.NumLock))
            {
                if (!isNLocked)
                {
                    notifyIcon1.Visible = true;
                    notifyIcon1.BalloonTipTitle = "MSI";
                    notifyIcon1.BalloonTipText = "Num-Lock is on";
                    notifyIcon1.ShowBalloonTip(1);
                }
                button2.BackColor = Color.Green;
                label2.Text = "Num-Lock On";
                isNLocked = true;
            }
            else if (!Control.IsKeyLocked(Keys.NumLock))
            {
                if (isNLocked)
                {
                    notifyIcon1.Visible = true;
                    notifyIcon1.BalloonTipTitle = "MSI";
                    notifyIcon1.BalloonTipText = "Num-Lock is off";
                    notifyIcon1.ShowBalloonTip(1);
                }
                button2.BackColor = Color.White;
                label2.Text = "Num-Lock Off";
                isNLocked = false;
            }

            if (Control.IsKeyLocked(Keys.Scroll))
            {
                if (!isSLocked)
                {
                    notifyIcon1.Visible = true;
                    notifyIcon1.BalloonTipTitle = "MSI";
                    notifyIcon1.BalloonTipText = "Scroll-Lock is on";
                    notifyIcon1.ShowBalloonTip(1);
                }
                button3.BackColor = Color.Green;
                label3.Text = "Scroll-Lock On";
                isSLocked = true;
            }
            else if (!Control.IsKeyLocked(Keys.Scroll))
            {
                if (isSLocked)
                {
                    notifyIcon1.Visible = true;
                    notifyIcon1.BalloonTipTitle = "MSI";
                    notifyIcon1.BalloonTipText = "Scroll-Lock is off";
                    notifyIcon1.ShowBalloonTip(1);
                }
                button3.BackColor = Color.White;
                label3.Text = "Scroll-Lock Off";
                isSLocked = false;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            switchMode(0x14);
        }

        private void switchMode(byte key)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;
            keybd_event(key, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            keybd_event(key, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP,
            (UIntPtr)0);

        }

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags,
            UIntPtr dwExtraInfo);

        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            switchMode(0x90);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            switchMode(0x91);
        }
    }
}