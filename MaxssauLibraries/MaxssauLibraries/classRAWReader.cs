using image_designer;
using System.Drawing.Imaging;
using static MaxssauLibraries.classLibRAW;
using static System.Runtime.InteropServices.Marshal;

namespace MaxssauLibraries
{
    public class classRAWReader: ClassAddToLog
    {
        private string ModuleName = "RAW Reader v0.1";

        private classLibRAW Libraw = new classLibRAW();

        LibRaw_errors libraw_result = 0;

        public enum Errors
        { 
            Errors_Success=0,
            Errors_Failed=1,
            Errors_LoggerIsNull=2,
            Errors_Exception=3
        }

        Errors  LastError;

        StatusResult Status;

        public InputRAWImage RAWImage;

        public enum StatusResult
        {
            Success=0,
            Failed=1,
            Null=2
        }

        public OperationStatus OpenRAW(string filename)
        {
            try
            {
                

                libraw_iparams_t libraw_Iparams_T = new libraw_iparams_t();

                var errc = 0;

                var libraw_handler=libraw_init(LibRaw_init_flags.LIBRAW_OPTIONS_NONE);
                libraw_result = libraw_open_file(libraw_handler, filename);

                
                
                if(libraw_result == LibRaw_errors.LIBRAW_SUCCESS)
                {
                    libraw_result = libraw_unpack(libraw_handler);
                    if (libraw_result == LibRaw_errors.LIBRAW_SUCCESS)
                    {
                        libraw_set_demosaic(libraw_handler, LibRaw_interpolation_quality.AHD);
                        libraw_set_output_bps(libraw_handler, LibRaw_output_bps.BPS16);
                        libraw_set_output_color(libraw_handler, LibRaw_output_color.RAW);
                        libraw_set_gamma(libraw_handler, 0, 1);
                        libraw_set_gamma(libraw_handler, 1, 1);

                        //libraw_result = libraw_raw2image(libraw_handler);
                        if (libraw_result == LibRaw_errors.LIBRAW_SUCCESS)
                        {
                            
                            if (libraw_result == libraw_dcraw_process(libraw_handler))
                            {
                                var ptr = libraw_dcraw_make_mem_image(libraw_handler, ref errc);
                                var img = PtrToStructure<libraw_processed_image_t>(ptr);

                                // rqeuired step before accessing the "data" array
                                Array.Resize(ref img.data, (int)img.data_size);
                                var adr = ptr + OffsetOf(typeof(libraw_processed_image_t), "data").ToInt32();
                                Copy(adr, img.data, 0, (int)img.data_size);

                                RAWImage = new InputRAWImage();

                                RAWImage.Image_Input_MinMaxLevels = new RGB_MinMaxValues();

                                RAWImage.ImageHeight=img.height;
                                RAWImage.ImageWidth=img.width;
                                RAWImage.Image_Input_RAW_RGB = new RGB_Pixel[RAWImage.ImageWidth, RAWImage.ImageHeight];

                                RAWImage.Image_Input_MinMaxLevels.Reset();

                                int coord = 0;

                                byte[] short_components = new byte[2];

                                for (int x=0;x< RAWImage.ImageWidth;x++)
                                {
                                    for(int y=0;y< RAWImage.ImageHeight;y++)
                                    {
                                        coord = 6*(y * RAWImage.ImageWidth + x);

                                        short_components[0] = img.data[coord + 0];
                                        short_components[1] = img.data[coord + 1];

                                        int R = BitConverter.ToUInt16(short_components);
                                        
                                        short_components[0] = img.data[coord + 2];
                                        short_components[1] = img.data[coord + 3];

                                        int G = BitConverter.ToUInt16(short_components);

                                        short_components[0] = img.data[coord + 4];
                                        short_components[1] = img.data[coord + 5];

                                        int B = BitConverter.ToUInt16(short_components);

                                        RAWImage.Image_Input_RAW_RGB[x, y].R = (double)R;
                                        RAWImage.Image_Input_RAW_RGB[x, y].G = (double)G;
                                        RAWImage.Image_Input_RAW_RGB[x, y].B = (double)B;

                                        RAWImage.Image_Input_MinMaxLevels.CalcRGB(RAWImage.Image_Input_RAW_RGB[x,y]);
                                    }
                                }

                                libraw_dcraw_clear_mem(ptr);
                                GC.Collect();
                                /*
                                // calculate padding for lines and add padding
                                var num = img.width % 4;
                                var padding = new byte[num];
                                var stride = img.width * img.colors * (img.bits / 8);
                                var line = new byte[stride];
                                var tmp = new List<byte>();
                                for (var i = 0; i < img.height; i++)
                                {
                                    Buffer.BlockCopy(img.data, stride * i, line, 0, stride);
                                    tmp.AddRange(line);
                                    tmp.AddRange(padding);
                                }*/
                                // release memory allocated by [libraw_dcraw_make_mem_image]

                                // create/save bitmap from mem image/array
                                /*var bmp = new Bitmap(img.width, img.height, PixelFormat.Format24bppRgb);
                                var bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                                Copy(tmp.ToArray(), 0, bmd.Scan0, (int)img.data_size);
                                bmp.UnlockBits(bmd);
                                var outJPEG = "out.jpg";
                                //Console.WriteLine("Saving image to: " + outJPEG);
                                bmp.Save(outJPEG, ImageFormat.Jpeg);*/
                            }
                        }
                    }
                    libraw_close(libraw_handler);
                    return OperationStatus.STATUS_FAIL;
                }
                else
                {
                    Status = StatusResult.Failed;
                    return OperationStatus.STATUS_FAIL;
                }
            }
            catch (Exception ex)
            {
                AddToLog(ex, ModuleName);
                return OperationStatus.STATUS_FAIL;
            }
        }

        public classRAWReader(ClassLogger logger)
        {
            try
            {
                if (logger == null)
                {
                    LastError = Errors.Errors_LoggerIsNull;
                    Status = StatusResult.Null;
                    return;
                }
                else
                {
                    Logger = logger;

                    Status = StatusResult.Success;
                    return;
                }
            }
            catch (Exception ex)
            {
                LastError = Errors.Errors_Exception;
                Status = StatusResult.Failed;

                AddToLog(ex, ModuleName);
            }
        }
    }
}
