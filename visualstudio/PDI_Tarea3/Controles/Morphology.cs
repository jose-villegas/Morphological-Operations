using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDI_Tarea3
{
    public partial class Morphology : Form
    {
        Bitmap original;
        public Morphology()
        {
            InitializeComponent();
            original = Cache.GetCurrentBitmap();
            Cache.SetMainformPictureBox(Colors.ToGrayScale(original));
            MorphologyFilters.LoadDefaultKernel();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cache.ToPreviousBitmapData();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap res = Cache.GetCurrentBitmap();

            if (radioButton1.Checked)
            {
                res = MorphologyFilters.ApplyCurrentKernel(true, res);
            }

            else if (radioButton2.Checked)
            {
                res = MorphologyFilters.ApplyCurrentKernel(false, res);
            }

            else if (radioButton3.Checked)
            {
                res = MorphologyFilters.ApplyCurrentKernel(false, res);
                res = MorphologyFilters.ApplyCurrentKernel(true, res);
            }
            else if (radioButton4.Checked)
            {
                res = MorphologyFilters.ApplyCurrentKernel(true, res);
                res = MorphologyFilters.ApplyCurrentKernel(false, res);
            }

            if (res != null)
            {
                Cache.SetMainformPictureBox(res);
                Cache.StoreCurrentBitmapData();
            }

            this.Close();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            Cache.SetMainformPictureBox(Colors.ToGrayScale(original));
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            Cache.SetMainformPictureBox(Colors.OtsuThreshold(Colors.ToGrayScale(original), 127));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PDI_Tarea3.Controles.MorphologyStructure ms = new PDI_Tarea3.Controles.MorphologyStructure();
            ms.Owner = this;
            ms.Show();
        }
    }
}
