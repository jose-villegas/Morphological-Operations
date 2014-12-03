using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDI_Tarea3
{
    public static class ScalingAlgorithms
    {

        public static Bitmap NearestNeighbor(Bitmap src, int new_width, int new_height)
        {
            if (src != null)
            {
                Bitmap dst = new Bitmap(new_width, new_height);
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
                    double ratioX = (float)(src.Width) / (dst.Width);
                    double ratioY = (float)(src.Height) / (dst.Height);
                    // Posiciones Iniciales
                    int dstPos = 0;
                    int srcPos = 0;
                    int pixel = 0;

                    // Por cada linea
                    for (int y = 0; y < dst.Height; y++)
                    {
                        dstPos = dstStride * y;
                        srcPos = srcStride * ((int)(y * ratioY));

                        // Por cada pixel
                        for (int x = 0; x < dst.Width; x++)
                        {
                            pixel = srcPos + bytesPerPixel * ((int)(x * ratioX));

                            for (int i = 0; i < bytesPerPixel; i++, pixel++, dstPos++)
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

        public static Bitmap Bilinear(Bitmap src, int new_width, int new_height)
        {
            if (src != null)
            {
                Bitmap dst = new Bitmap(new_width, new_height);
                BitmapData bmSrcData = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadWrite, src.PixelFormat);
                BitmapData bmDstData = dst.LockBits(new Rectangle(0, 0, dst.Width, dst.Height), ImageLockMode.ReadWrite, src.PixelFormat);

                try
                {
                    int bytesPerPixel = Bitmap.GetPixelFormatSize(src.PixelFormat) / 8;
                    int srcStride = bmSrcData.Stride;
                    int dstStride = bmDstData.Stride;
                    int offset = dstStride - bytesPerPixel * dst.Width;
                    IntPtr srcPtr = bmSrcData.Scan0;
                    IntPtr dstPtr = bmDstData.Scan0;
                    int srcBytes = Math.Abs(srcStride) * src.Height;
                    int dstBytes = Math.Abs(dstStride) * dst.Height;
                    byte[] srcData = new byte[srcBytes];
                    byte[] dstData = new byte[dstBytes];
                    // Copiamos los valores RGB en el arreglo.
                    System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcData, 0, srcBytes);
                    double ratioX = (float)(src.Width) / (dst.Width);
                    double ratioY = (float)(src.Height) / (dst.Height);
                    // Coordenadas Interpolacion Y
                    int src_y1, src_y2;
                    double dst_y1, dst_y2;
                    // Coordenadas Interpolacion X
                    int src_x1, src_x2;
                    double dst_x1, dst_x2;
                    // Variables para los 4 puntos de interpolacion
                    int P1 = 0, P2 = 0, P3 = 0, P4 = 0;
                    // Posiciones temporales
                    int tmp1 = 0;
                    int tmp2 = 0;
                    // Posiciones inicial
                    int dstPos = 0;

                    // Por cada linea
                    for (int y = 0; y < dst.Height; y++)
                    {
                        // Coordenadas en Y
                        src_y1 = (int)((double)y * ratioY);
                        src_y2 = (src_y1 == src.Height - 1) ? src_y1 : src_y1 + 1;
                        dst_y1 = (double)y * ratioY - (double)src_y1;
                        dst_y2 = 1.0 - dst_y1;
                        // Posiciones temporales
                        tmp1 = src_y1 * srcStride;
                        tmp2 = src_y2 * srcStride;

                        // Por cada pixel
                        for (int x = 0; x < dst.Width; x++)
                        {
                            // Coordenadas en X
                            src_x1 = (int)((double)x * ratioX);
                            src_x2 = (src_x1 == src.Width - 1) ? src_x1 : src_x1 + 1;
                            dst_x1 = (double)x * ratioX - (double)src_x1;
                            dst_x2 = 1.0 - dst_x1;
                            // Calculamos los 4 puntos para la interpolacion
                            P1 = tmp1 + src_x1 * bytesPerPixel;
                            P2 = tmp1 + src_x2 * bytesPerPixel;
                            P3 = tmp2 + src_x1 * bytesPerPixel;
                            P4 = tmp2 + src_x2 * bytesPerPixel;

                            for (int i = 0; i < bytesPerPixel; i++, dstPos++, P1++, P2++, P3++, P4++)
                            {
                                dstData[dstPos] = (byte)( dst_y2 * (dst_x2 * srcData[P1] + dst_x1 * srcData[P2]) + dst_y1 * (dst_x2 * srcData[P3] + dst_x1 * srcData[P4]));
                            }
                        }

                        dstPos += offset;
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
