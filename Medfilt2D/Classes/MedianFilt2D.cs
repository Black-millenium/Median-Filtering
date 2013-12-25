using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;

namespace Medfilt2D.Classes
{
    public class MedianFilt2D
    {
        /// <summary>
        /// Медианная фильтрация
        /// </summary>
        /// <param name="inputBitmap">Изображение</param>
        /// <param name="size">Размер скользящей апертуры (она квадратная)</param>
        /// <returns>Отфильтрованное изображение</returns>
        public static unsafe Bitmap Medfilt2D(Bitmap inputBitmap, int size)
        {
            int bitHeight = inputBitmap.Height,
                bitWidth = inputBitmap.Width,
                edgex = size / 2,
                edgey = size / 2;

            uint[,]
                inputPixelValue = PicToIntArray(inputBitmap),
                outputPix = new uint[bitWidth, bitHeight];

            outputPix = (uint[,])inputPixelValue.Clone();

            DateTime start = DateTime.Now;
            Parallel.For(edgex, bitWidth - edgex, (x) =>
            {
                for (int y = edgey; y < bitHeight - edgey; y++)
                {
                    uint[] window = new uint[size * size];

                    for (int fx = 0; fx < size; fx++)
                        for (int fy = 0; fy < size; fy++)
                            window[fx * size + fy] = inputPixelValue[x + fx - edgex, y + fy - edgey];

                    Sort(window);
                    outputPix[x, y] = window[(window.Length) / 2];
                }
            });
            Debug.WriteLine("Medfilt2D: {0}", (DateTime.Now - start));
            return IntArrayToBitmap(outputPix);
        }

        /// <summary>
        /// Изображение предварительного просмотра обработки
        /// </summary>
        /// <param name="bitmap">Изображение</param>
        /// <param name="x">Конечная длина</param>
        /// <param name="y">Конечная ширина</param>
        /// <param name="size">Радиус фильтрации</param>
        /// <returns>Отфильтрованная уменьшенная копия изображения</returns>
        public static unsafe Bitmap GetPreviev(Bitmap bitmap, int x, int y, int size)
        {
            return Medfilt2D(new Bitmap(bitmap, new Size(x, y)), size);
        }

        /// <summary>
        /// Получить медиану для апертуры
        /// </summary>
        /// <param name="array">Массив поиска медианы</param>
        static void Sort(uint[] array)
        {
            Array.Sort(array);
        }

        /// <summary>
        /// Метод конвертации Bitmap изображения в массив типа int
        /// </summary>
        /// <param name="bmp">Bitmap изображения</param>
        /// <returns>Массив int, каждый элемент
        /// массива представляет цвет, закодированый по системе ARGB</returns>
        static uint[,] PicToIntArray(Bitmap bmp)
        {
            DateTime start = DateTime.Now;
            int height = bmp.Height,
                width = bmp.Width;

            uint[,] result = new uint[width, height];

            Parallel.For(0, width, (int i) =>
                {
                    for (int j = 0; j < height; j++)
                        lock (bmp)
                        {
                            result[i, j] = (uint)bmp.GetPixel(i, j).ToArgb();
                        }
                });

            Debug.WriteLine("BmpToIntArray: {0}", (DateTime.Now - start));
            return result;
        }

        /// <summary>
        /// Метод конвертации массива int в Bitmap-изображение
        /// </summary>
        /// <param name="array">Двухмерный массив типа int</param>
        /// <returns>Bitmap-изображение</returns>
        static Bitmap IntArrayToBitmap(uint[,] array)
        {
            DateTime start = DateTime.Now;
            int arrDim0 = array.GetUpperBound(0),
                arrDim1 = array.GetUpperBound(1);

            Bitmap result = new Bitmap(arrDim0 + 1, arrDim1 + 1);

            for (int i = 0; i <= arrDim0; i++)
                for (int j = 0; j <= arrDim1; j++)
                    result.SetPixel(i, j, Color.FromArgb((int)array[i, j]));

            Debug.WriteLine("IntArrayToBitmap: {0}", (DateTime.Now - start));

            return result;
        }
    }
}