using System;
using System.Windows.Forms;
using Medfilt2D.Classes;

namespace Medfilt2D
{
    public partial class MedianFilteringForm : Form
    {
        Form1 parent;
        public MedianFilteringForm(Form1 parent)
        {
            this.parent = parent;
            InitializeComponent();
            this.pictureBox1.Image = MedianFilt2D.GetPreviev(parent.Pic, this.pictureBox1.Width,
                this.pictureBox1.Height, 1);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.pictureBox1.Image = MedianFilt2D.GetPreviev(parent.Pic, this.pictureBox1.Width,
                this.pictureBox1.Height, ((TrackBar)sender).Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            parent.StartProcessMedianFilt(this.trackBar1.Value);
            this.Close();
        }
    }
}
