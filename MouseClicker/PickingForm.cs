using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MouseClicker
{
    public partial class PickingForm : Form
    {
        private MainForm father = null;
        public PickingForm(MainForm father)
        {
            this.father = father;
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.MouseClick += Form_Transparent_MouseClick;
            this.Cursor = Cursors.Cross;
            this.Opacity = 0.5;
        }

        private void Form_Transparent_MouseClick(object sender, MouseEventArgs e)
        {
            father.tmpLocationX = e.X;
            father.tmpLocationY = e.Y;
            father.LocationPicked();
            this.Close();
        }
    }
}
