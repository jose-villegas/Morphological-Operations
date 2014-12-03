using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDI_Tarea3
{
    public partial class EdgeDetection : Form
    {
        public EdgeDetection()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex == 1)
            {
                this.radioButton1.Enabled = true;
                this.radioButton2.Enabled = false;
                this.radioButton3.Enabled = false;
                this.radioButton4.Enabled = false;
            }

            else if (this.comboBox1.SelectedIndex == 2)
            {
                this.radioButton1.Enabled = true;
                this.radioButton2.Enabled = true;
                this.radioButton3.Enabled = true;
                this.radioButton4.Enabled = false;
            }

            else
            {
                this.radioButton1.Enabled = true;
                this.radioButton2.Enabled = true;
                this.radioButton3.Enabled = true;
                this.radioButton4.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConvolutionFilters.EdgeDetectionMode();
            Bitmap res = Colors.ToGrayScale(Cache.GetCurrentBitmap());
            int size = 0;
            size = radioButton1.Checked ? 3 : size;
            size = radioButton2.Checked ? 5 : size;
            size = radioButton3.Checked ? 7 : size;
            size = radioButton4.Checked ? 9 : size;

            if (this.comboBox1.SelectedIndex == 0)
            {
                res = ConvolutionFilters.ApplySobelFilter(size, res);
            }

            else if (this.comboBox1.SelectedIndex == 1)
            {
                res = ConvolutionFilters.ApplyRobertsFilter(size, res);
            }

            else if (this.comboBox1.SelectedIndex == 2)
            {
                res = ConvolutionFilters.ApplyPrewittFilter(size, res);
            }

            else if (this.comboBox1.SelectedIndex == 3)
            {
                res = ConvolutionFilters.ApplyLaplacianOfGaussianFilter(size, res);
            }

            if (size != 0 && res != null)
            {
                Cache.SetMainformPictureBox(res);
                Cache.StoreCurrentBitmapData();
            }

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
