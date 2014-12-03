using PDI_Tarea3.Algoritmos;
using PDI_Tarea3.Controles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDI_Tarea3
{
    public partial class Form1 : Form
    {
        Bitmap currentImage;
        public Form1()
        {
            InitializeComponent();
            // Habilitamos double buffer en el contenedor de bitmaps para mejorar el performance
            Form1.SetDoubleBuffered(this.pictureBox1);
            // Asignamos la ventana principal como dueña de todas las subventanas de edicion
            Cache.SetOwner(this, 10);
            // Configuracion de variables
        }

        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
            {
                return;
            }

            System.Reflection.PropertyInfo aProp =
                typeof(System.Windows.Forms.Control).GetProperty(
                    "DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }

        private Bitmap convertoTo24(Bitmap orig)
        {
            Bitmap res = new Bitmap(orig.Width, orig.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (Graphics gr = Graphics.FromImage(res))
            {
                gr.DrawImage(orig, new Rectangle(0, 0, res.Width, res.Height));
            }
            return res;
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentImage = (Bitmap)Image.FromFile(openFileDialog1.FileName);

                if (currentImage.PixelFormat != PixelFormat.Format24bppRgb)
                {
                    if (currentImage.PixelFormat != PixelFormat.Format32bppArgb)
                    {
                        currentImage = convertoTo24((Bitmap)Image.FromFile(openFileDialog1.FileName));
                    }
                }

                pictureBox1.Image = currentImage;
                pictureBox1.Visible = true;
                Form1_Resize(null, null);
                Zoom.Reset();
                this.toolStripLabel5.Text = "100%";
                Cache.Start(currentImage);
                EnableImageControls();
            }
        }

        private void EnableImageControls()
        {
            this.cargarOriginalToolStripMenuItem.Enabled = true;
            this.escalarToolStripMenuItem.Enabled = true;
            this.informacionToolStripMenuItem.Enabled = true;
            this.histogramaRGBToolStripMenuItem.Enabled = true;
            this.brilloYContrasteToolStripMenuItem.Enabled = true;
            this.negativoToolStripMenuItem.Enabled = true;
            this.umbralToolStripMenuItem.Enabled = true;
            this.detectarBordesToolStripMenuItem.Enabled = true;
            this.reduccionDeRuidoToolStripMenuItem.Enabled = true;
            this.girarToolStripMenuItem.Enabled = true;
            this.voltearHorizontalmenteToolStripMenuItem.Enabled = true;
            this.voltearVerticalmenteToolStripMenuItem.Enabled = true;
            this.kernelPersonalToolStripMenuItem.Enabled = true;
            this.operacionesMorfologicasToolStripMenuItem.Enabled = true;
            this.recortarToolStripMenuItem.Enabled = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            ImagePanel.AutoScroll = false;
            ImagePanel.AutoScroll = true;
            this.toolStrip2.Left = this.toolStripContainer1.Width - this.toolStrip2.Width;
            RefreshPictureBox();
        }

        private void negativoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cache.SetMainformPictureBox(Colors.Negativo(this.currentImage));
            Cache.StoreCurrentBitmapData();
        }

        private void informacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentImage != null)
            {
                Bitmap bitmap = (Bitmap)Image.FromFile(openFileDialog1.FileName);
                FileInfo fi = new FileInfo(openFileDialog1.FileName);
                string text = "";
                text += "Tamaño en pixeles\t:\t" + currentImage.Width + "x" + currentImage.Height + " pixeles\n";
                text += "Numero de pixeles\t:\t" + currentImage.Width * currentImage.Height + " pixeles\n\n";
                text += "Formato de color\t:\t" + bitmap.PixelFormat.ToString() + "\n";
                text += "Profundidad de pixel\t:\t" + System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat) + "\n";
                text += "Cantidad de colores\t:\t" + Math.Pow(2, System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat)) + "\n\n";
                text += "Tipo de Archivo\t:\tImagen " + Path.GetExtension(openFileDialog1.FileName) + "\n";
                text += "Tamaño de Archivo\t:\t" + fi.Length / 1024.0f + " KB\n";
                MessageBox.Show(text, fi.Name);
            }
        }

        private void histogramaRGBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Histogram ht = new Histogram();
            ht.Owner = this;
            ht.Show();
        }

        private void brilloYContrasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Brightness_Contrast bc = new Brightness_Contrast();
            bc.Owner = this;
            bc.Show();
        }

        public Bitmap getCurrentBitmap()
        {
            if (this.pictureBox1.Image != null)
            {
                return currentImage;
            }

            else
            {
                return null;
            }
        }

        public void setPictureBoxBitmap(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                currentImage = bitmap;
                
                if (Zoom.Index() == 0)
                {
                    pictureBox1.Image = currentImage;
                }

                else
                {
                    UpdateZoomPictureBox();
                }
                Form1_Resize(null, null);
            }
        }

        private void umbralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Threshold th = new Threshold();
            th.Owner = this;
            th.Show();
        }

        private void cargarOriginalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cache.RestoreToOriginalBitmap();
            Zoom.Reset();
            pictureBox1.Visible = true;
        }

        private void deshacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cache.UndoBitmap();
        }

        private void rehacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cache.RedoBitmap();
        }

        public void SetRedo(bool value)
        {
            this.rehacerToolStripMenuItem.Enabled = value;
        }

        public void SetUndo(bool value)
        {
            this.deshacerToolStripMenuItem.Enabled = value;
        }

        private void detectarBordesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.currentImage != null)
            {
                EdgeDetection ed = new EdgeDetection();
                ed.Owner = this;
                ed.Show();
            }
        }

        private void promedioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.currentImage != null)
            {
                ConvolutionFilters.RGBMode();
                Cache.SetMainformPictureBox(ConvolutionFilters.ApplyMeanFilter(Cache.GetCurrentBitmap()));
                Cache.StoreCurrentBitmapData();
            }
        }

        private void medianaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.currentImage != null)
            {
                ConvolutionFilters.RGBMode();
                Cache.SetMainformPictureBox(ConvolutionFilters.ApplyMedianFilter(Cache.GetCurrentBitmap()));
                Cache.StoreCurrentBitmapData();
            }
        }

        private void escalarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Scaling scalingui = new Scaling();
            scalingui.Owner = this;
            scalingui.Show();
        }

        private void zoomPlusButton_Click(object sender, EventArgs e)
        {
            if (this.currentImage != null)
            {
                Bitmap bmp;

                if ((bmp = Zoom.ZoomUp(currentImage)) != null)
                {
                    this.pictureBox1.Image = bmp;

                    // Tamaño del Picture Box
                    if (Zoom.Index() != 0)
                    {
                        this.pictureBox1.Image = bmp;
                    }

                    else
                    {
                        this.pictureBox1.Image = currentImage;
                    }

                    Form1_Resize(null, null);
                    this.toolStripLabel5.Text = Zoom.CurrentZoomValue().ToString() + "%";
                }
            }
        }

        private void zoomMinusButton_Click(object sender, EventArgs e)
        {
            if (this.currentImage != null)
            {
                Bitmap bmp;

                if ((bmp = Zoom.ZoomDown(currentImage)) != null)
                {
                    this.pictureBox1.Image = bmp;

                    //Tamaño del Picture Box
                    if (Zoom.Index() != 0)
                    {
                        this.pictureBox1.Image = bmp;
                    }

                    else
                    {
                        this.pictureBox1.Image = currentImage;
                    }

                    Form1_Resize(null, null);
                    this.toolStripLabel5.Text = Zoom.CurrentZoomValue().ToString() + "%";
                }
            }
        }

        private void UpdateZoomPictureBox()
        {
            this.pictureBox1.Image = Zoom.CurrentZoom(currentImage);
        }

        public void RefreshPictureBox()
        {
            if (this.currentImage != null)
            {
                pictureBox1.Refresh();
            }
        }

        private void voltearHorizontalmenteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cache.SetMainformPictureBox(Mirror.MirrorHorizontal(Cache.GetCurrentBitmap()));
            Cache.StoreCurrentBitmapData();
        }

        private void voltearVerticalmenteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cache.SetMainformPictureBox(Mirror.MirrorVertical(Cache.GetCurrentBitmap()));
            Cache.StoreCurrentBitmapData();
        }

        private void girarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rotation rt = new Rotation();
            rt.Owner = this;
            rt.Show();
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentImage != null)
            {
                currentImage.Save(Path.GetDirectoryName(openFileDialog1.FileName) + "\\(1)" + Path.GetFileName(openFileDialog1.FileName));
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void kernelPersonalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.currentImage != null)
            {
                ConvolutionFilters.RGBMode();
                Cache.SetMainformPictureBox(ConvolutionFilters.ApplyPersonalKernel(Cache.GetCurrentBitmap()));
                Cache.StoreCurrentBitmapData();
            }
        }

        private void operacionesMorfologicasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Morphology mp = new Morphology();
            mp.Owner = this;
            mp.Show();
        }

        private void recortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Crop cr = new Crop();
            cr.Owner = this;
            cr.Show();
        }
    }

}
