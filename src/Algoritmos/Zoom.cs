using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDI_Tarea3.Algoritmos
{
    public static class Zoom
    {
        private static double[] zoomlist = new  double[] { 0.05, 0.10, 0.15, 0.25, 0.50, 0.75, 0.90, 1, 1.10, 1.20, 1.50, 1.75, 2.50, 4.50, 6.00 };
        private static int index = 7;
        private static int neutralIndex = 7;

        public static void Reset()
        {
            index = neutralIndex;
        }

        public static Bitmap ZoomUp(Bitmap bmp)
        {
            if (index + 1 < zoomlist.GetLength(0))
            {
                int finalWidth;
                int finalHeight;
                index++;

                if (Index() != 0)
                {
                    finalWidth = (int)(zoomlist[index] * bmp.Width);
                    finalHeight = (int)(zoomlist[index] * bmp.Height);
                    return ScalingAlgorithms.NearestNeighbor(bmp, finalWidth, finalHeight);
                }

                else
                {
                    return bmp;
                }
            }

            return null;
        }

        public static Bitmap ZoomDown(Bitmap bmp)
        {
            if (index > 0)
            {
                int finalWidth;
                int finalHeight;
                index--;

                if (Index() != 0)
                {
                    finalWidth = (int)(zoomlist[index] * bmp.Width);
                    finalHeight = (int)(zoomlist[index] * bmp.Height);
                    return ScalingAlgorithms.NearestNeighbor(bmp, finalWidth, finalHeight);
                }

                else
                {
                    return bmp;
                }
            }

            return null;
        }

        public static Bitmap CurrentZoom(Bitmap bmp)
        {
            if (Index() != 0)
            {
                int finalWidth;
                int finalHeight;
                finalWidth = (int)(zoomlist[index] * bmp.Width);
                finalHeight = (int)(zoomlist[index] * bmp.Height);
                return ScalingAlgorithms.NearestNeighbor(bmp, finalWidth, finalHeight);
            }

            else
            {
                return bmp;
            }
        }

        public static int CurrentZoomValue()
        {
            return (int)(100 * zoomlist[index]);
        }
        public static int Index()
        {
            return index - neutralIndex;
        }
    }
}
