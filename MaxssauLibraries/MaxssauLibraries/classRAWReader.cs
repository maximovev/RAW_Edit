using image_designer;
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
                var libraw_handler=libraw_init(LibRaw_init_flags.LIBRAW_OPTIONS_NONE);
                libraw_set_demosaic(libraw_handler, LibRaw_interpolation_quality.VNG);
                libraw_set_output_bps(libraw_handler, LibRaw_output_bps.BPS16);
                libraw_set_output_color(libraw_handler, LibRaw_output_color.RAW);
                libraw_result = libraw_open_file(libraw_handler, filename);
                if(libraw_result == LibRaw_errors.LIBRAW_SUCCESS)
                {
                    libraw_result = libraw_unpack(libraw_handler);
                    if (libraw_result == LibRaw_errors.LIBRAW_SUCCESS)
                    {
                        libraw_result = libraw_raw2image(libraw_handler);
                        if (libraw_result == LibRaw_errors.LIBRAW_SUCCESS)
                        {
                            var piparam = libraw_get_iparams(libraw_handler);
                            var iparam = PtrToStructure<libraw_iparams_t>(piparam);
                            var poparam = libraw_get_imgother(libraw_handler);
                            var oparam = PtrToStructure<libraw_imgother_t>(poparam);
                            var errc = 0;
                            var ptr = libraw_dcraw_make_mem_image(libraw_handler, ref errc);
                            var img = PtrToStructure<libraw_processed_image_t>(ptr);

                            // rqeuired step before accessing the "data" array
                            Array.Resize(ref img.data, (int)img.data_size);
                            var adr = ptr + OffsetOf(typeof(libraw_processed_image_t), "data").ToInt32();
                            Copy(adr, img.data, 0, (int)img.data_size);

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
