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

namespace PDI_Tarea3.Controles
{
    public partial class MorphologyStructure : Form
    {
        private int MAX_SIZE = 15;
        public MorphologyStructure()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double [,] kernel = new double[tableLayoutPanel3.RowCount, tableLayoutPanel3.ColumnCount];

            for (int i = 0; i < tableLayoutPanel3.RowCount; i++)
            {
                for (int j = 0; j < tableLayoutPanel3.ColumnCount; j++)
                {
                    if (((CheckBox)tableLayoutPanel3.GetControlFromPosition(i, j)).Checked)
                    {
                        kernel[i, j] = 1;
                    }

                    else
                    {
                        kernel[i, j] = 0;
                    }
                }
            }
            MorphologyFilters.LoadKernel(kernel);
            this.Close();
        }

        private void Expand()
        {
            if (tableLayoutPanel3.ColumnCount < MAX_SIZE)
            {
                tableLayoutPanel3.ColumnCount++;
                tableLayoutPanel3.RowCount++;

                for (int i = 0; i < tableLayoutPanel3.RowCount; i++)
                {
                    CheckBox checkb = new CheckBox();
                    checkb.Checked = true;
                    checkb.Anchor = Anchor = System.Windows.Forms.AnchorStyles.None;
                    tableLayoutPanel3.Controls.Add(checkb, tableLayoutPanel3.RowCount - 1, i);

                    if (i != tableLayoutPanel3.RowCount - 1)
                    {
                        CheckBox chec = new CheckBox();
                        chec.Checked = true;
                        chec.Anchor = Anchor = System.Windows.Forms.AnchorStyles.None;
                        tableLayoutPanel3.Controls.Add(chec, i, tableLayoutPanel3.RowCount - 1);
                    }
                }

                tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
                tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Se agregan dos filas y dos columnas ya que el kernel siempre es de tamaño 2n+1
            Expand();
            Expand();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
