using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Medfilt2D.Classes
{
    class ColorProcessing
    {
        /// <summary>
        /// Метод изменения яркости и контрастности изображения
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <param name="brightness">Яркость</param>
        /// <param name="gamma">Гамма</param>
        /// <param name="cRed">Контраст красного</param>
        /// <param name="cGreen">Контраст зеленого</param>
        /// <param name="cBlue">Контраст синего</param>
        /// <param name="cAlpha">Прозрачность</param>
        /// <returns>Новое изображение</returns>
        public static Bitmap AdjustImage(Image image, float brightness = 1.0f, float gamma = 1.0f, float cRed = 1.0f, float cGreen = 1.0f, float cBlue = 1.0f, float cAlpha = 1.0f)
        {
            brightness -= 1.0f; // Значение яркости указывается в виде дельты, то есть 0 - нет изменений, 0.5 - увеличить на 50%, -0.5 - уменьшить на 50% и т.д.
            float[][] matrix = 
            {
                new float[] { cRed, 0, 0, 0, 0 },   // Контраст красного
                new float[] { 0, cGreen, 0, 0, 0 }, // Контраст зеленого
                new float[] { 0, 0, cBlue, 0, 0 },  // Контраст синего
                new float[] { 0, 0, 0, cAlpha, 0 }, // Контраст альфы
                new float[] { brightness, brightness, brightness, 0, 1 } // Яркость
            };

            ImageAttributes atts = new ImageAttributes();
            atts.SetColorMatrix(new ColorMatrix(matrix), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            atts.SetGamma(gamma, ColorAdjustType.Bitmap);

            Bitmap newImage = new Bitmap(image.Width, image.Height);
            using (var gfx = Graphics.FromImage(newImage))
                gfx.DrawImage(image, new Rectangle(0, 0, newImage.Width, newImage.Height), 0, 0, newImage.Width, newImage.Height, GraphicsUnit.Pixel, atts);
            return newImage;
        }

        /// <summary>
        /// Изображение предварительного просмотра обработки
        /// </summary>
        /// <param name="bitmap">Изображение</param>
        /// <param name="x">Конечная длина</param>
        /// <param name="y">Конечная ширина</param>
        /// <param name="size">Радиус фильтрации</param>
        /// <returns>Отфильтрованная уменьшенная копия изображения</returns>
        public static unsafe Bitmap GetPreviev(Bitmap bitmap, int x, int y, float brightness = 1.0f, float gamma = 1.0f, float cRed = 1.0f, float cGreen = 1.0f, float cBlue = 1.0f, float cAlpha = 1.0f)
        {
            return AdjustImage(new Bitmap(bitmap, new Size(x, y)), brightness, gamma, cRed, cGreen, cBlue, cAlpha);
        }
    }
}
