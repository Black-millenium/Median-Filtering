using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Medfilt2D.Classes;

namespace Medfilt2D
{
    public partial class ColorForm : Form
    {
        Form1 parent;

        public ColorForm(Form1 form1)
        {
            this.parent = form1;
            InitializeComponent();

            this.pictureBox1.Image = ColorProcessing.GetPreviev(parent.Pic, this.pictureBox1.Width,
                this.pictureBox1.Height, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            this.pictureBox1.Image = ColorProcessing.GetPreviev(
                parent.Pic,
                this.pictureBox1.Width,
                this.pictureBox1.Height,
                this.brightnessTrackBar.Value * 0.1f,
                this.gammaTrackBar.Value * 0.1f,
                this.redTrackBar.Value * 0.1f,
                this.greenTrackBar.Value * 0.1f,
                this.blueTrackBar.Value * 0.1f,
                this.alphaTrackBar.Value * 0.1f);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parent.StartProcessColorEdit(this.brightnessTrackBar.Value * 0.1f,
                this.gammaTrackBar.Value * 0.1f,
                this.redTrackBar.Value * 0.1f,
                this.greenTrackBar.Value * 0.1f,
                this.blueTrackBar.Value * 0.1f,
                this.alphaTrackBar.Value * 0.1f);
            this.Close();
        }
    }
}
