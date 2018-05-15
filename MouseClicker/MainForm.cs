using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MouseClicker
{
    public partial class MainForm : Form
    {
        private bool isPickingMode = false;
        private bool allowToClick = false;
        public int tmpLocationX = 0;
        public int tmpLocationY = 0;
        private Timer timer = new Timer();


        public MainForm()
        {
            InitializeComponent();
            this.button_choose_location.Click += Button_choose_location_Click;
            this.button_choose_location.BackColor = Color.Green;
            this.TopMost = true;
            this.timer.Interval = 5000;
            this.label_ClickStatus.Text = "未点击";
            this.label_ClickStatus.BackColor = Color.Red;
            this.timer.Tick += Timer_Tick;
            Win32Api.RegisterHotKey(Handle, 100, Win32Api.KeyModifiers.Ctrl|Win32Api.KeyModifiers.Alt, Keys.Q);
            this.FormClosing += MainForm_FormClosing;

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Win32Api.UnregisterHotKey(Handle, 100);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            
            if(this.allowToClick)
            {
                this.timer.Stop();
                //MessageBox.Show("模拟点击");
                Win32Api.MouseLeftClick(this.tmpLocationX, this.tmpLocationY, 0);
                this.timer.Start();
            }
            
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if(this.isPickingMode)
            {
                this.isPickingMode = false;
                this.Cursor = Cursors.Default;
                this.button_choose_location.Text = "选择点击位置:";
                this.button_choose_location.BackColor = Color.Green;
                this.tmpLocationX = e.Location.X;
                this.tmpLocationY = e.Location.Y;
            }
        }

        private void Button_choose_location_Click(object sender, EventArgs e)
        {
            if(!this.isPickingMode)
            {
                this.isPickingMode = true;
                //this.Cursor = Cursors.Cross;
                Form pickingForm = new PickingForm(this);
                pickingForm.Show();
                this.button_choose_location.Text = "选取中。。。";
                this.button_choose_location.BackColor = Color.Red;
            }

        }

        public void LocationPicked()
        {
            this.Cursor = Cursors.Default;
            this.isPickingMode = false;
            this.button_choose_location.BackColor = Color.Green;
            this.button_choose_location.Text = "选择点击位置:";
            this.label_locationX.Text = this.tmpLocationX.ToString();
            this.label_LocationY.Text = this.tmpLocationY.ToString();


        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:
                            //MessageBox.Show("点击了快捷键！");
                            this.HotKeyPressed();
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void HotKeyPressed()
        {
            int tmpInterval = Convert.ToInt32(this.numericUpDown_interval.Value);
            if(tmpInterval == 0)
            {
                MessageBox.Show("定时器间隔应大于0！！！");
            }
            else
            {
                if (allowToClick)
                {
                    this.allowToClick = false;
                    this.label_ClickStatus.Text = "未点击";
                    this.label_ClickStatus.BackColor = Color.Red;
                }
                else
                {
                    this.allowToClick = true;
                    this.label_ClickStatus.Text = "点击中";
                    this.label_ClickStatus.BackColor = Color.Green;
                    this.timer.Interval = 1000 * tmpInterval;
                    this.timer.Start();
                }
            }
            
        }

    }
}
