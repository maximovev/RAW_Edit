using image_designer;
using static MaxssauLibraries.classLibRAW;
using static System.Runtime.InteropServices.Marshal;


namespace MaxssauLibraries
{
    public class Libraw_Settings
    {
        public Libraw_Settings() 
        {
            quality=LibRaw_interpolation_quality.AHD;
            output_color=LibRaw_output_color.RAW;
            output_format=LibRaw_output_formats.TIFF;
            output_bps = LibRaw_output_bps.BPS16;
        }
        public LibRaw_interpolation_quality quality;
        public LibRaw_output_bps output_bps;
        public LibRaw_output_formats output_format;
        public LibRaw_output_color output_color;
        public bool NoBrightness;
        public bool UseToneCurve;
    }

    public class classRAWReader: ClassAddToLog
    {
        private string ModuleName = "RAW Reader v0.1";

        private classLibRAW Libraw = new classLibRAW();

        public Libraw_Settings settings;

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
                var errc = 0;

                var libraw_handler=libraw_init(LibRaw_init_flags.LIBRAW_OPTIONS_NONE);
                libraw_result = libraw_open_file(libraw_handler, filename);

                if(libraw_result == LibRaw_errors.LIBRAW_SUCCESS)
                {
                    libraw_result = libraw_unpack(libraw_handler);
                    if (libraw_result == LibRaw_errors.LIBRAW_SUCCESS)
                    {
                        
                        libraw_set_demosaic(libraw_handler, settings.quality);
                        libraw_set_output_bps(libraw_handler, settings.output_bps);
                        libraw_set_output_color(libraw_handler, settings.output_color);
                        libraw_set_no_auto_bright(libraw_handler, 1);
                        
                        if (settings.UseToneCurve == false)
                        {
                            libraw_set_gamma(libraw_handler, 0, 1);
                            libraw_set_gamma(libraw_handler, 1, 1);    
                        }

                        //libraw_result = libraw_raw2image(libraw_handler);
                        if (libraw_result == LibRaw_errors.LIBRAW_SUCCESS)
                        {
                            if (libraw_result == libraw_dcraw_process(libraw_handler))
                            {
                                var ptr = libraw_dcraw_make_mem_image(libraw_handler, ref errc);
                                var img = PtrToStructure<libraw_processed_image_t>(ptr);

                                Array.Resize(ref img.data, (int)img.data_size);
                                var adr = ptr + OffsetOf(typeof(libraw_processed_image_t), "data").ToInt32();
                                Copy(adr, img.data, 0, (int)img.data_size);

                                RAWImage = new InputRAWImage();

                                RAWImage.Image_Input_MinMaxLevels = new RGB_MinMaxValues();

                                RAWImage.ImageHeight = img.height;
                                RAWImage.ImageWidth = img.width;
                                RAWImage.Image_Input_RAW_RGB = new RGB_Pixel[RAWImage.ImageWidth, RAWImage.ImageHeight];

                                RAWImage.Image_Input_MinMaxLevels.Reset();
                               
                                Parallel.For(0, RAWImage.ImageWidth, x =>
                                {
                                    for (int y = 0; y < RAWImage.ImageHeight; y++)
                                    {
                                        int coord = 6 * (y * RAWImage.ImageWidth + x);
                                        
                                        RAWImage.Image_Input_RAW_RGB[x, y].R = (double)BitConverter.ToUInt16(new byte[] { (byte)img.data[coord + 0], (byte)img.data[coord + 1] });
                                        RAWImage.Image_Input_RAW_RGB[x, y].G = (double)BitConverter.ToUInt16(new byte[] { (byte)img.data[coord + 2], (byte)img.data[coord + 3] });
                                        RAWImage.Image_Input_RAW_RGB[x, y].B = (double)BitConverter.ToUInt16(new byte[] { (byte)img.data[coord + 4], (byte)img.data[coord + 5] });

                                        RAWImage.Image_Input_MinMaxLevels.CalcRGB(RAWImage.Image_Input_RAW_RGB[x, y]);
                                    }
                                });
                                libraw_dcraw_clear_mem(ptr);
                                libraw_recycle(libraw_handler);
                                GC.Collect();
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
                settings=new Libraw_Settings();
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
