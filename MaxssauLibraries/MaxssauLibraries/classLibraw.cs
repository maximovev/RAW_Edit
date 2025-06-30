using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static MaxssauLibraries.LibRawProcessor;

namespace MaxssauLibraries
{

    public struct RawData
    {
        public RAWImageInfo ImageInfo;
        public ushort[,] Data;
    }

    public struct RAWImageInfo
    {
        public ushort raw_width;
        public ushort raw_height;
        public ushort width;
        public ushort height;
        public ushort colors;
        public ushort bpp;
    }

    public enum Result
    {
        SUCCESS = 0,
        FAIL = 1
    }


    public sealed unsafe class LibRawProcessor : IDisposable
    {

        public OperationStatus LastOperationResult;

        private IntPtr _handle;
        private bool _disposed;

        private const string LibRawDll = "libraw.dll"; // Используйте libraw.so под Linux

        [DllImport(LibRawDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr libraw_init(uint flags);

        [DllImport(LibRawDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern int libraw_open_file(IntPtr lr, string fileName);

        [DllImport(LibRawDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern int libraw_unpack(IntPtr lr);

        [DllImport(LibRawDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void libraw_recycle(IntPtr lr);

        [DllImport(LibRawDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void libraw_close(IntPtr lr);

        [DllImport(LibRawDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern int libraw_get_raw_width(IntPtr lr);

        [DllImport(LibRawDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern int libraw_get_raw_height(IntPtr lr);

        [DllImport(LibRawDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr libraw_get_image(IntPtr lr);

        [DllImport(LibRawDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr libraw_get_raw_pixel(IntPtr lr, uint index);

        [DllImport(LibRawDll, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_raw2image(IntPtr lr);

        [DllImport(LibRawDll, CharSet = CharSet.Ansi)]
        public static extern int libraw_get_iheight(IntPtr handler);

        [DllImport(LibRawDll, CharSet = CharSet.Ansi)]
        public static extern int libraw_get_iwidth(IntPtr handler);

        [DllImport(LibRawDll, CharSet = CharSet.Ansi)]
        public static extern int libraw_COLOR(IntPtr lr, int row, int col);


        private readonly IntPtr _librawHandle;


        public LibRawProcessor(string filename, ref InputRAWImage image_data)
        {
            _handle = libraw_init(0);
            if (_handle == IntPtr.Zero)
            {
                LastOperationResult = OperationStatus.STATUS_FAIL;
                return;
            }
            LastOperationResult = GetRawPixels(filename, ref image_data);
        }


        private OperationStatus GetRawPixels(string filePath, ref InputRAWImage image_data)
        {
            // Открываем файл
            int openResult = libraw_open_file(_handle, filePath);
            if (openResult != 0)
            {
                return OperationStatus.STATUS_FAIL;   
            }

            // Распаковываем метаданные и RAW данные
            int unpackResult = libraw_unpack(_handle);


            if (unpackResult != 0)
            {
                libraw_close(_handle);
                return OperationStatus.STATUS_FAIL;
            }

            uint width = (uint)libraw_get_raw_width(_handle);
            uint height = (uint)libraw_get_raw_height(_handle);

            image_data.Image_Input_RAW_RGB = new image_designer.RGB_Pixel[width / 2, height / 2];
            image_data.ImageWidth = (int)width / 2;
            image_data.ImageHeight = (int)height / 2;
            image_data.Image_Input_MinMaxLevels = new image_designer.RGB_MinMaxValues();


            int result = 0;

            uint counter = 0;

            for (uint j = 0; j < height; j++)
            {
                for (uint i = 0; i < width; i++)
                {
                    counter = i + j * width;
                    result = (int)libraw_get_raw_pixel(_handle, counter);
                    if (result != -1)
                    {
                        switch ((uint)libraw_COLOR(_handle, (int)i, (int)j))
                        {
                            case 0:
                                {
                                    image_data.Image_Input_RAW_RGB[i / 2, j / 2].R = (ushort)result;
                                    image_data.Image_Input_MinMaxLevels.R.calc((ushort)result);
                                }
                                break;
                            case 1:
                                {
                                    image_data.Image_Input_RAW_RGB[i / 2, j / 2].G1 = (ushort)result;
                                    image_data.Image_Input_MinMaxLevels.G1.calc((ushort)result);
                                    image_data.Image_Input_RAW_RGB[i / 2, j / 2].G += (ushort)result / 2;
                                }
                                break;
                            case 2:
                                {
                                    image_data.Image_Input_RAW_RGB[i / 2, j / 2].B = (ushort)result;
                                    image_data.Image_Input_MinMaxLevels.B.calc((ushort)result);
                                }
                                break;
                            case 3:
                                {
                                    image_data.Image_Input_RAW_RGB[i / 2, j / 2].G2 = (ushort)result;
                                    image_data.Image_Input_MinMaxLevels.G2.calc((ushort)result);
                                    image_data.Image_Input_RAW_RGB[i / 2, j / 2].G += (ushort)result / 2;
                                    image_data.Image_Input_MinMaxLevels.G.calc(image_data.Image_Input_RAW_RGB[i / 2, j / 2].G);
                                }
                                break;
                            default:
                                {

                                }
                                break; ;
                        }
                    }
                }
            }

            return OperationStatus.STATUS_OK;

        }

        public void Dispose()
        {
            if (!_disposed)
            {
                libraw_close(_handle);
                _disposed = true;
            }
        }
    }

    public class LibRawException : Exception
    {
        public LibRawException(string message) : base(message) { }
    }


}
