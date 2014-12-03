using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDI_Tarea3
{
    public static class MorphologyFilters
    {
        private static double[,] kernel;
        private static int KernelMiddlePoint;
        private static bool mode;

        public static void LoadDefaultKernel()
        {
            kernel = new double[,]
            {
                { 1, 1, 1 },
                { 1, 1, 1 },
                { 1, 1, 1 }
            };
            KernelMiddlePoint = 1;
        }

        public static void LoadKernel(double[,] krnl)
        {
            kernel = krnl;
            KernelMiddlePoint = kernel.GetLength(0) / 2;
        }

        public static Bitmap ApplyCurrentKernel(bool md, Bitmap bmp) // true = erosion, false = dilatacion
        {
            mode = md;
            return LoadedMatrixBitmapConvolution(bmp);
        }

        private static Bitmap LoadedMatrixBitmapConvolution(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                Bitmap res = new Bitmap(bitmap.Width, bitmap.Height);
                BitmapData bmData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                BitmapData dstBmData = res.LockBits(new Rectangle(0, 0, res.Width, res.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

                try
                {
                    int height = bitmap.Height;
                    int width = bitmap.Width;
                    int stride = bmData.Stride;
                    int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
                    int totalLength = Math.Abs(stride) * bitmap.Height;
                    IntPtr ptr = bmData.Scan0;
                    IntPtr dstPtr = dstBmData.Scan0;
                    // Declaramos un arreglo para guardar toda la data.
                    byte[] rgbValues = new byte[totalLength];
                    byte[] rgbValuesOrig = new byte[totalLength];
                    // Copiamos los valores RGB en el arreglo.
                    System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValuesOrig, 0, totalLength);
                    System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, totalLength);

                    for (int y = KernelMiddlePoint; y < height - KernelMiddlePoint; y++)
                    {
                        for (int x = KernelMiddlePoint; x < width - KernelMiddlePoint; x++)
                        {
                            ApplyMatrixToPixel(rgbValuesOrig, rgbValues, x, y, width, bytesPerPixel);
                        }
                    }

                    // Copy the RGB values back to the bitmap
                    System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, dstPtr, totalLength);
                }

                finally
                {
                    res.UnlockBits(dstBmData);
                    bitmap.UnlockBits(bmData);
                }

                return res;
            }

            return null;
        }

        private static void ApplyMatrixToPixel(byte[] src, byte[] dst, int x, int y, int width, int bytesPerPixel)
        {
            double finalX;
            int pos = 0;
            finalX = mode == true ? 0 : 255;

            for (int i = 0; i < kernel.GetLength(0); i++)
            {
                for (int j = 0; j < kernel.GetLength(1); j++)
                {
                    int I = y + i - KernelMiddlePoint;
                    int J = x + j - KernelMiddlePoint;
                    pos = (I * width + J) * bytesPerPixel;

                    if (kernel[i, j] > 0)
                    {
                        if (mode == true)
                        {
                            finalX = src[pos] > finalX ? src[pos] : finalX;
                        }

                        else
                        {
                            finalX = src[pos] < finalX ? src[pos] : finalX;
                        }
                    }
                }
            }

            pos = ((y * width) + x) * bytesPerPixel;
            dst[pos] = dst[pos + 1] = dst[pos + 2] = (byte)finalX;
        }

    }
}
