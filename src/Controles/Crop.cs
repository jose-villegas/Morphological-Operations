using PDI_Tarea3.Algoritmos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDI_Tarea3.Controles
{
    public partial class Crop : Form
    {
        public Crop()
        {
            InitializeComponent();
            this.numericUpDown1.DataBindings.Add("Value", this.hScrollBar1, "Value", false, DataSourceUpdateMode.OnPropertyChanged);
            this.numericUpDown2.DataBindings.Add("Value", this.hScrollBar2, "Value", false, DataSourceUpdateMode.OnPropertyChanged);
            this.numericUpDown3.DataBindings.Add("Value", this.hScrollBar3, "Value", false, DataSourceUpdateMode.OnPropertyChanged);
            this.numericUpDown4.DataBindings.Add("Value", this.hScrollBar4, "Value", false, DataSourceUpdateMode.OnPropertyChanged);
            // Limitando los controles con respecto a la resolucion
            this.numericUpDown1.Maximum = Cache.GetCurrentBitmap().Width;
            this.numericUpDown2.Maximum = Cache.GetCurrentBitmap().Height;
            this.numericUpDown3.Maximum = Cache.GetCurrentBitmap().Width;
            this.numericUpDown4.Maximum = Cache.GetCurrentBitmap().Height;
            this.hScrollBar1.Maximum = Cache.GetCurrentBitmap().Width;
            this.hScrollBar2.Maximum = Cache.GetCurrentBitmap().Height;
            this.hScrollBar3.Maximum = Cache.GetCurrentBitmap().Width;
            this.hScrollBar4.Maximum = Cache.GetCurrentBitmap().Height;
            // Copiando
            this.pictureBox1.Image = Cache.GetCurrentBitmap();
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = CropAlgorithm.CropBitmap(Cache.GetCurrentBitmap(), hScrollBar1.Value, hScrollBar2.Value, hScrollBar3.Value, hScrollBar4.Value);
            ImagePanel.AutoScroll = false;
            ImagePanel.AutoScroll = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cache.SetMainformPictureBox((Bitmap)pictureBox1.Image);
            Cache.StoreCurrentBitmapData();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
