using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LockSwitch
{
    public partial class Form1 : Form
    {
        private Boolean isCLocked;
        private Boolean isNLocked;
        private Boolean isSLocked;
        private Dictionary<String, Point> defLocations;
        private Size defB1Size;
        private Size defB2Size;
        private Size defB3Size;
        private Size defL1Size;
        private Size defL2Size;
        private Size defL3Size;
        private Rectangle resolution;
        private Boolean inTopRight;
        private Boolean inBottomRight;
        private Boolean inDefault;
        private Boolean inBottomLeft;
        private RegistryKey regKey;
        private int dpi;
        

        //Constructs the program through checking the initial state of the lock keys
        //, implementing the UI, and starting the system timer.
        public Form1()
        {
            InitializeComponent();
            this.notifyIcon1.Icon = SystemIcons.Application;
            resolution = Screen.PrimaryScreen.Bounds;
            dpi = getDPI();
            isCLocked = Control.IsKeyLocked(Keys.CapsLock);
            isNLocked = Control.IsKeyLocked(Keys.NumLock);
            isSLocked = Control.IsKeyLocked(Keys.Scroll);
            this.BackColor = Color.Magenta;
            this.TransparencyKey = this.BackColor;

            WindowState = FormWindowState.Normal;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            defLocations = new Dictionary<String, Point>();
            defLocations.Add("label1", label1.Location);
            defLocations.Add("label2", label2.Location);
            defLocations.Add("label3", label3.Location);
            defLocations.Add("button1", button1.Location);
            defLocations.Add("button2", button2.Location);
            defLocations.Add("button3", button3.Location);
            defL1Size = new Size(1 , 0.3);
            defL2Size = new Size(1, 0.3);
            defL3Size = new Size(1, 0.3);
            defB1Size = new Size(119 * 1.2, 56 * 1.2);
            defB2Size = new Size(61 * 1.2, 61 * 1.2);
            defB3Size = new Size(80 * 1.2 , 50 * 1.2);
            

            inDefault = true;
            inBottomLeft = false;
            inTopRight = false;
            inBottomRight = false;
            
            

            button1.Text = "CAPS LOCK";
            button2.Text = "NUM LOCK";
            button3.Text = "SCROLL LOCK";
            notifyIcon1.BalloonTipTitle = "MSI";
            regKey = Registry.CurrentUser.OpenSubKey(
                    "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (regKey.GetValue(Application.ProductName) != null){ 
                ((ToolStripMenuItem)contextMenuStrip1.Items[4]).Checked = true; //Turns on the check mark for "Run on startup"
            }
            else
            {
                ((ToolStripMenuItem)contextMenuStrip1.Items[4]).Checked = false;//Turns off the check mark for "Run on startup"
            }
            timer1.Interval = 10;
            timer1.Start();
        }

        //Checks the state of Num_Lock, Scroll_Lock, and Caps_Lock and notifies
        //the user when a change has been made. The check occurs everytime the timer
        //ticks.
        private void timer1_Tick(object sender, EventArgs e)
        { 

            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                if (!isCLocked)
                { 
                    notifyIcon1.BalloonTipText = "Caps-Lock is on";
                    notifyIcon1.ShowBalloonTip(100);
                }
                ((ToolStripMenuItem)contextMenuStrip1.Items[0]).Checked = true;
                button1.BackColor = Color.Green;
                label1.Text = "Caps-Lock On";
                isCLocked = true;
            }
            else if (!Control.IsKeyLocked(Keys.CapsLock))
            { 
                if (isCLocked)
                { 
                    notifyIcon1.BalloonTipText = "Caps-Lock is off";
                    notifyIcon1.ShowBalloonTip(100);
                }
                ((ToolStripMenuItem)contextMenuStrip1.Items[0]).Checked = false;
                button1.BackColor = Color.White;
                label1.Text = "Caps-Lock Off";
                isCLocked = false;
            }
            if (Control.IsKeyLocked(Keys.NumLock))
            {
                if (!isNLocked)
                {
                    notifyIcon1.BalloonTipText = "Num-Lock is on";
                    notifyIcon1.ShowBalloonTip(100);
                }
                ((ToolStripMenuItem)contextMenuStrip1.Items[1]).Checked = true;
                button2.BackColor = Color.Green;
                label2.Text = "Num-Lock On";
                isNLocked = true;
            }
            else if (!Control.IsKeyLocked(Keys.NumLock))
            {
                if (isNLocked)
                {
                    notifyIcon1.BalloonTipText = "Num-Lock is off";
                    notifyIcon1.ShowBalloonTip(100);
                }
                ((ToolStripMenuItem)contextMenuStrip1.Items[1]).Checked = false;
                button2.BackColor = Color.White;
                label2.Text = "Num-Lock Off";
                isNLocked = false;
            }

            if (Control.IsKeyLocked(Keys.Scroll))
            {
                if (!isSLocked)
                {
                    notifyIcon1.BalloonTipText = "Scroll-Lock is on";
                    notifyIcon1.ShowBalloonTip(100);
                }
                ((ToolStripMenuItem)contextMenuStrip1.Items[2]).Checked = true;
                button3.BackColor = Color.Green;
                label3.Text = "Scroll-Lock On";
                isSLocked = true;
            }
            else if (!Control.IsKeyLocked(Keys.Scroll))
            {
                if (isSLocked)
                {
                    notifyIcon1.BalloonTipText = "Scroll-Lock is off";
                    notifyIcon1.ShowBalloonTip(100);
                }
                ((ToolStripMenuItem)contextMenuStrip1.Items[2]).Checked = false;
                button3.BackColor = Color.White;
                label3.Text = "Scroll-Lock Off";
                isSLocked = false;
            }

            TopMost = true;
            if ((!inTopRight || resolution != Screen.PrimaryScreen.Bounds || getDPI() != dpi) && true)
            {
                dpi = getDPI();
                resolution = Screen.PrimaryScreen.Bounds;
                newDefaultLoc();
                moveTopRight();
                inTopRight = true;
            }

        } 

        //Programmatically presses the Caps_Lock key when button1(Caps_Lock) is clicked
        private void button1_Click(object sender, EventArgs e)
        {
            switchMode(0x14);
        }

        //Programmatically presses the Num_Lock key when button2(Num_Lock) is clicked
        private void button2_Click(object sender, EventArgs e)
        {
            switchMode(0x90);
        }

        //Programmatically pressed the Scroll_Lock key when button3(Scroll_Lock) is clicked
        private void button3_Click(object sender, EventArgs e)
        {
            switchMode(0x91);
        }

        //Allows the user to press a key programmatically through inputing the desired 
        //key in byte form
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
            notifyIcon1.Icon = Icon;
        }



        public void moveTopRight()
        {
            defaultLocation();
            resize();
            double widthScale = resolution.Width / 1920.0;
            double heightScale = resolution.Height / 1080.0;
            double moveRightAmount = (resolution.Width - (button2.Location.X + button2.Width)) * 0.99999999;
            double moveUpAmount = (0 - button2.Location.Y);
            moveLocation((int)moveRightAmount, (int)(moveUpAmount));
        }

        public void resize()
        {
            double widthScale = resolution.Width / 1920.0;
            double heightScale = resolution.Height / 1080.0;
            
            button1.Width = (int)(((double)defB1Size.x * heightScale));    //The following height and weight of the controls are scaled to the 
            button1.Height = (int)(((double)defB1Size.y * heightScale));  //the Y-axis resolution change because most screens scale down their
            button2.Width = (int)(((double)defB2Size.x * heightScale));  //X-axis resolution through adding black bars on the right and leftsides.
            button2.Height = (int)(((double)defB2Size.y * heightScale));   //The Y-axis are immune to to these blackbars. Overall, elimnating  the visual
            button3.Width = (int)(((double)defB3Size.x * heightScale));    //size change of the controls from resolution change is the primary goal of resizing.
            button3.Height = (int)(((double)defB3Size.y * heightScale));   //The Y-axis' independence from black bars gives us an accurate dipiction of the visual
                                                                                         //size change and enables us to adjusts our object's size accordingly.
            button1.Font = newFont(heightScale, button1);
            button2.Font = newFont(heightScale, button2);
            button3.Font = newFont(heightScale, button3);
            label1.Font = newFont(heightScale, label1);
            label2.Font = newFont(heightScale, label2);
            label3.Font = newFont(heightScale, label3);
            int b2Offset = (button1.Location.X + button1.Width) - (button2.Location.X + button2.Width);
            int b3Offset = (button1.Location.X + button1.Width) - (button3.Location.X + button3.Width);
            if(b2Offset != 0 || b3Offset != 0)
            {
                button2.Location = new Point(button2.Location.X + b2Offset, button2.Location.Y);
                button3.Location = new Point(button3.Location.X + b3Offset, button3.Location.Y);
            }
            int b1tob2Space = (button1.Location.Y) - (button2.Height + button2.Location.Y);
            button1.Location = new Point(button1.Location.X, button1.Location.Y + (15 - b1tob2Space));
            int b1tob3Space = (button3.Location.Y) - (button1.Height + button1.Location.Y);
            button3.Location = new Point(button3.Location.X, button3.Location.Y + (15 - b1tob3Space));
            label1.Location = new Point(button1.Location.X - (50 + label1.Width), button1.Location.Y + (button1.Height / 2) - (label1.Height / 2));
            label2.Location = new Point(button2.Location.X - (50 + label2.Width), button2.Location.Y + (button2.Height / 2) - (label2.Height / 2));
            label3.Location = new Point(button3.Location.X - (50 + label3.Width), button3.Location.Y + (button3.Height / 2) - (label3.Height / 2));
        }

        private int inchToPixels(double desiredInch)
        {
            double pixels = 0;
            while (pixels /dpi <= desiredInch - 0.01|| pixels /dpi >= desiredInch + 0.01)
            {
                pixels += 1.0;
            }
            return (int)pixels;
        }

        private Font newFont(double heightScale, Control control)
        {
            double scale = dpi / 100.0;
            float fontSize = (float)heightScale *(float) (9.5/scale);
            return new Font(control.Font.Name, fontSize, control.Font.Style, control.Font.Unit);
        }
        public void moveLocation(int x, int y)
        {
            label1.Location = new Point(label1.Location.X + x, label1.Location.Y + y);
            label2.Location = new Point(label2.Location.X + x, label2.Location.Y + y);
            label3.Location = new Point(label3.Location.X + x, label3.Location.Y + y);
            button1.Location = new Point(button1.Location.X + x, button1.Location.Y + y);
            button2.Location = new Point(button2.Location.X + x, button2.Location.Y + y);
            button3.Location = new Point(button3.Location.X + x, button3.Location.Y + y);
        }

        public void defaultLocation()
        {
            label1.Location = defLocations["label1"];
            label2.Location = defLocations["label2"];
            label3.Location = defLocations["label3"];
            button1.Location = defLocations["button1"];
            button2.Location = defLocations["button2"];
            button3.Location = defLocations["button3"];
        }
       
        private void newDefaultLoc()
        {
            double widthScale = resolution.Width / 1920.0;
            double heightScale = resolution.Height / 1080.0;
            defLocations["label1"] = new Point((int)(defLocations["label1"].X * widthScale)
                ,(int)(defLocations["label1"].Y * heightScale));
            defLocations["label2"] = new Point((int)(defLocations["label2"].X * widthScale)
                ,(int)(defLocations["label2"].Y * heightScale));
            defLocations["label3"] = new Point((int)(defLocations["label3"].X * widthScale)
                ,(int)(defLocations["label3"].Y * heightScale));
            defLocations["button1"] = new Point((int)(defLocations["button1"].X * widthScale)
                ,(int)(defLocations["button1"].Y * heightScale));
            defLocations["button2"] = new Point((int)(defLocations["button2"].X * widthScale)
                ,(int)(defLocations["button2"].Y * heightScale));
            defLocations["button3"] = new Point((int)(defLocations["button3"].X * widthScale)
                ,(int)(defLocations["button3"].Y * heightScale));
        }
        private void capsLockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switchMode(0x14);

        }

        private void scrollLockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switchMode(0x91);
        }

        private void numLockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switchMode(0x90);
        }

       

        private void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            timer1.Start();

        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        
        private void runOnStartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (regKey.GetValue(Application.ProductName) == null)
            {
                regKey.SetValue(Application.ProductName, Application.ExecutablePath);
            } else
            {
                regKey.DeleteValue(Application.ProductName);
            }
            ((ToolStripMenuItem)contextMenuStrip1.Items[4]).Checked = !((ToolStripMenuItem)contextMenuStrip1.Items[4]).Checked;

        }

        private int getDPI()
        {
            return (int)(Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop\\WindowMetrics").GetValue("AppliedDPI"));
        }




        static void SystemEvents_PowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        {
            if (e.Mode == Microsoft.Win32.PowerModes.StatusChange)
            {
                // Check what the status is and act accordingly
            }
        }
    }
}
