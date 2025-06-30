using image_designer;
using System.Drawing.Imaging;
using static System.Runtime.InteropServices.Marshal;
using System.Threading.Tasks;

namespace MaxssauLibraries
{
    

    public class classRAWReader: ClassAddToLog
    {
        private string ModuleName = "RAW Reader v0.1";

        private LibRawProcessor Libraw;

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
                Libraw=new LibRawProcessor(filename,ref RAWImage);
                return OperationStatus.STATUS_OK;
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
