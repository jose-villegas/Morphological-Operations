using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDI_Tarea3.Algoritmos
{
    public static class CropAlgorithm
    {
        public static Bitmap CropBitmap(Bitmap src, int x, int y, int width, int height)
        {
            if (src != null)
            {
                Bitmap dst = new Bitmap(width, height);
                BitmapData bmSrcData = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadWrite, src.PixelFormat);
                BitmapData bmDstData = dst.LockBits(new Rectangle(0, 0, dst.Width, dst.Height), ImageLockMode.ReadWrite, src.PixelFormat);

                try
                {
                    int bytesPerPixel = Bitmap.GetPixelFormatSize(src.PixelFormat) / 8;
                    int srcStride = bmSrcData.Stride;
                    int dstStride = bmDstData.Stride;
                    IntPtr srcPtr = bmSrcData.Scan0;
                    IntPtr dstPtr = bmDstData.Scan0;
                    int srcBytes = Math.Abs(srcStride) * src.Height;
                    int dstBytes = Math.Abs(dstStride) * dst.Height;
                    byte[] srcData = new byte[srcBytes];
                    byte[] dstData = new byte[dstBytes];
                    // Copiamos los valores RGB en el arreglo.
                    System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcData, 0, srcBytes);
                    // Posiciones Iniciales
                    int dstPos = 0;
                    int dstIndex = 0;
                    int srcPos = 0;
                    int pixel = 0;

                    // Por cada linea
                    for (int i = y; i < height + y; i++, dstIndex++)
                    {
                        dstPos = dstStride * dstIndex;
                        srcPos = srcStride * i;

                        // Por cada pixel
                        for (int j = x; j < width + x; j++)
                        {
                            pixel = srcPos + bytesPerPixel * j;

                            for (int pos = 0; pos < bytesPerPixel; pos++, pixel++, dstPos++)
                            {
                                dstData[dstPos] = srcData[pixel];
                            }
                        }
                    }

                    // Colocamos los valores calculados en el nuevo bitmap
                    System.Runtime.InteropServices.Marshal.Copy(dstData, 0, dstPtr, dstBytes);
                }

                finally
                {
                    src.UnlockBits(bmSrcData);
                    dst.UnlockBits(bmDstData);
                }

                // Retornamos el bitmap escalado
                return dst;
            }

            return null;
        }
    }
}
