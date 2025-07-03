using BitMiracle.LibTiff.Classic;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using BitMiracle.LibTiff.Classic;

namespace MaxssauLibraries
{
    
    public class Tiff16BitSaver
    {
        public static void SaveRgb16BitTiff(string filePath, ushort[,] redChannel, ushort[,] greenChannel, ushort[,] blueChannel)
        {
            int width = redChannel.GetLength(0);
            int height = redChannel.GetLength(1);

            // Проверка размеров массивов
            if (greenChannel.GetLength(0) != width || greenChannel.GetLength(1) != height ||
                blueChannel.GetLength(0) != width || blueChannel.GetLength(1) != height)
            {
                throw new ArgumentException("All channels must have the same dimensions");
            }

            // Создаем Bitmap с форматом 48bppRgb (16 бит на канал)
            using (var bitmap = new Bitmap(width, height, PixelFormat.Format48bppRgb))
            {
                // Блокируем биты изображения для прямого доступа
                var bitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format48bppRgb);

                try
                {
                    // Копируем данные из массивов в битовый массив изображения
                    for (int y = 0; y < height; y++)
                    {
                        IntPtr scanLine = bitmapData.Scan0 + y * bitmapData.Stride;

                        for (int x = 0; x < width; x++)
                        {
                            // Формат 48bppRgb хранит данные как BGR (по 2 байта на канал)
                            Marshal.WriteInt16(scanLine, x * 6 + 0, (short)blueChannel[x, y]);
                            Marshal.WriteInt16(scanLine, x * 6 + 2, (short)greenChannel[x, y]);
                            Marshal.WriteInt16(scanLine, x * 6 + 4, (short)redChannel[x, y]);
                        }
                    }
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                // Сохраняем как TIFF с 16 битами на канал
                var encoder = GetEncoder(ImageFormat.Tiff);
                var encoderParams = new EncoderParameters(2);
                encoderParams.Param[0] = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionNone);
                encoderParams.Param[1] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.MultiFrame);

                bitmap.Save(filePath, encoder, encoderParams);
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }

    public class LibTiffNET
    {
        public static void SaveWithLibTiff(string path, ushort[,] red, ushort[,] green, ushort[,] blue)
        {
            int width = red.GetLength(0);
            int height = red.GetLength(1);

            using (Tiff tiff = Tiff.Open(path, "w"))
            {
                tiff.SetField(TiffTag.IMAGEWIDTH, width);
                tiff.SetField(TiffTag.IMAGELENGTH, height);
                tiff.SetField(TiffTag.SAMPLESPERPIXEL, 3);
                tiff.SetField(TiffTag.BITSPERSAMPLE, 16);
                tiff.SetField(TiffTag.PHOTOMETRIC, Photometric.RGB);
                tiff.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                tiff.SetField(TiffTag.ROWSPERSTRIP, height);

                byte[] buffer = new byte[width * 3 * 2]; // 3 канала по 2 байта
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Buffer.BlockCopy(BitConverter.GetBytes(red[x, y]), 0, buffer, x * 6 + 0, 2);
                        Buffer.BlockCopy(BitConverter.GetBytes(green[x, y]), 0, buffer, x * 6 + 2, 2);
                        Buffer.BlockCopy(BitConverter.GetBytes(blue[x, y]), 0, buffer, x * 6 + 4, 2);
                    }
                    tiff.WriteScanline(buffer, y);
                }
            }
        }
    }
}
