using image_designer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxssauLibraries
{
    public class classWBCorrection
    {
        private classLogger Logger;

        public classWBCorrection(classLogger logger)
        {
            Logger = logger;
        }

        private classColorConversion ColorConverter=new classColorConversion();

        private double[] ApplyCAT02Adaptation(double[] xyz, double[] sourceWhite, double[] targetWhite)
        {
            // Матрица CAT02 (из CIE CAM02)
            double[,] cat02Matrix =
            {
                { 0.7328, 0.4296, -0.1624 },
                { -0.7036, 1.6975, 0.0061 },
                { 0.0030, 0.0136, 0.9834 }
            };

            // Обратная матрица CAT02
            double[,] invCat02Matrix =
            {
                { 1.096124, -0.278869, 0.182745 },
                { 0.454369, 0.473533, 0.072098 },
                { -0.009628, -0.005698, 1.015326 }
            };

            // Конвертируем XYZ в RGB (CAT02)
            double[] sourceRGB = MultiplyMatrixVector(cat02Matrix, xyz);
            double[] refWhiteRGB = MultiplyMatrixVector(cat02Matrix, sourceWhite);
            double[] targetWhiteRGB = MultiplyMatrixVector(cat02Matrix, targetWhite);

            // Масштабируем компоненты
            double[] scaledRGB =
            {
                sourceRGB[0] * (targetWhiteRGB[0] / refWhiteRGB[0]),
                sourceRGB[1] * (targetWhiteRGB[1] / refWhiteRGB[1]),
                sourceRGB[2] * (targetWhiteRGB[2] / refWhiteRGB[2])
            };

            // Возвращаемся в XYZ
            double[] adaptedXYZ = MultiplyMatrixVector(invCat02Matrix, scaledRGB);

            return adaptedXYZ;
        }

        /// <summary>
        /// Умножает матрицу 3x3 на вектор 3x1.
        /// </summary>
        private double[] MultiplyMatrixVector(double[,] matrix, double[] vector)
        {
            double[] result = new double[3];
            for (int i = 0; i < 3; i++)
            {
                result[i] = matrix[i, 0] * vector[0] + matrix[i, 1] * vector[1] + matrix[i, 2] * vector[2];
            }
            return result;
        }

        double[] AdjustColorTemperature(double[] xyz, double temperatureK)
        {
            // Получаем xy для целевой температуры (Planckian Locus + точные формулы CIE)
            double[] targetXY = GetChromaticityFromTemperature(temperatureK);

            // Преобразуем xyY в XYZ (Y = 1 для нормализации)
            double targetX = targetXY[0] / targetXY[1];
            double targetY = 1.0;
            double targetZ = (1 - targetXY[0] - targetXY[1]) / targetXY[1];
            double[] targetWhite = { targetX, targetY, targetZ };

            // Исходная белая точка (D65)
            double[] sourceWhite = { 0.95047, 1.0, 1.08883 };

            // Применяем CAT02 (Chromatic Adaptation Transform)
            double[] adaptedXYZ = ApplyCAT02Adaptation(xyz, sourceWhite, targetWhite);

            return adaptedXYZ;
        }

        /// <summary>
        /// Вычисляет координаты xy по цветовой температуре (точный метод, включая 2000K).
        /// </summary>
        private double[] GetChromaticityFromTemperature(double tempK)
        {
            // Используем формулы из CIE и статьи "Precise Color Communication" (OSRAM)
            double x, y;

            if (tempK < 1667 || tempK > 25000)
                throw new ArgumentOutOfRangeException("tempK", "Диапазон: 1667K–25000K");

            double t = tempK / 1000.0;

            if (tempK <= 4000)
            {
                x = -0.2661239 * (1e9 / (t * t * t)) - 0.2343589 * (1e6 / (t * t)) + 0.8776956 * (1e3 / t) + 0.179910;
            }
            else
            {
                x = -3.0258469 * (1e9 / (t * t * t)) + 2.1070379 * (1e6 / (t * t)) + 0.2226347 * (1e3 / t) + 0.240390;
            }

            // Вычисляем y по формуле Ким-Шаша (Kim-Shah, 2020)
            if (tempK <= 2222)
                y = -1.1063814 * x * x * x - 1.34811020 * x * x + 2.18555832 * x - 0.20219683;
            else if (tempK <= 4000)
                y = -0.9549476 * x * x * x - 1.37418593 * x * x + 2.09137015 * x - 0.16748867;
            else
                y = 3.0817580 * x * x * x - 5.87338670 * x * x + 3.75112997 * x - 0.37001483;

            return new double[] { x, y };
        }

        public OperationStatus Calculate(ref XYZ_Pixel xyz_pixel, int color_temperature)
        {
            try
            {
                double[] xyz = { xyz_pixel.X, xyz_pixel.Y, xyz_pixel.Z};

                double[] correctedXYZ = AdjustColorTemperature(xyz, color_temperature);

                xyz_pixel.X = correctedXYZ[0];
                xyz_pixel.Y = correctedXYZ[1];
                xyz_pixel.Z = correctedXYZ[2];

                return OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    if (Logger.status == classLogger.STATUS.OPEN)
                    {
                        Logger.add_to_log("WB Calculator", ex.Message);
                        if (ex.StackTrace != null)
                        {
                            Logger.add_to_log("WB Calculator", ex.StackTrace);
                        }
                    }
                }
                return OperationStatus.STATUS_FAIL;
            }
        }
    }
}
