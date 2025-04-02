using image_designer;
using static MaxssauLibraries.classLibRAW;

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
                libraw_iparams_t libraw_Iparams_T = new libraw_iparams_t();

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
