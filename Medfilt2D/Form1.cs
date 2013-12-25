using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Medfilt2D.Classes;

namespace Medfilt2D
{
    public partial class Form1 : Form
    {
        public Bitmap Pic { get; set; }
        delegate void invokeDelegate();

        public Form1()
        {
            InitializeComponent();
        }

        public void StartProcessMedianFilt(int size)
        {
            this.statusLabel.Text = "Начата медианная фильтрация, пожалуйста, подождите.";
            Task processing = Task.Factory.StartNew(() =>
            {
                DateTime start = DateTime.Now;
                Bitmap img = MedianFilt2D.Medfilt2D(Pic, size);

                this.BeginInvoke(new invokeDelegate(() =>
                    {
                        Pic = img;
                        this.pictureBox1.Image = Pic;
                        this.pictureBox1.Invalidate();
                    }));

                this.Invoke(new invokeDelegate(() =>
                {
                    this.statusLabel.Text = "Фильтрация завершена, наслаждайтесь изображением.";
                    MessageBox.Show(string.Format("Время обработки: {0}", (DateTime.Now - start)),
                        "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }));
            });
        }

        public void StartProcessColorEdit(float brightness, float gamma, float red, float green, float blue, float alpha)
        {
            DateTime start = DateTime.Now;
            Pic = ColorProcessing.AdjustImage(
                Pic, brightness, gamma, red, green, blue, alpha);
            this.pictureBox1.Image = Pic;
            MessageBox.Show(string.Format("Время обработки: {0}", (DateTime.Now - start)),
                        "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG",
                FilterIndex = 1,
                Multiselect = false
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (ofd.FileName != null)
                    {
                        Pic = new Bitmap(ofd.FileName);
                        this.pictureBox1.Image = Pic;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка открытия файла. \n" + ex.Message);
                }
            }

            this.фильтрыToolStripMenuItem.Enabled = true;
        }

        private void medianFiltering_Click(object sender, EventArgs e)
        {
            MedianFilteringForm mff = new MedianFilteringForm(this);
            mff.ShowDialog();
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Image Files(*.BMP)|*.BMP",
                FilterIndex = 1,
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (sfd.FileName != null)
                    {
                        this.pictureBox1.Image.Save(sfd.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения файла. \n" + ex.Message);
                }
            }
        }

        private void brightnessAndContrastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorForm bf = new ColorForm(this);
            bf.ShowDialog();
        }

        private void сбросToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Pic = null;
            this.pictureBox1.Image = null;
        }
    }
}
