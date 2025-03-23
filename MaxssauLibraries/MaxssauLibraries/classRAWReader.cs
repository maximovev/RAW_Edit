using image_designer;

namespace MaxssauLibraries
{
    public class classRAWReader
    {
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


        public classRAWReader(string filename, ref classLogger logger)
        {
            try
            {
                if (logger == null)
                {
                    LastError = Errors.Errors_LoggerIsNull;
                    Status = StatusResult.Null;
                    return;
                }
            }
            catch (Exception ex)
            {
                LastError = Errors.Errors_Exception;
                Status = StatusResult.Failed;

                if(logger!=null)
                {
                    if(logger.status==classLogger.STATUS.OPEN)
                    {
                        logger.add_to_log("RawReader",ex.Message);
                        if (ex.StackTrace != null)
                        {
                            logger.add_to_log("RawReader", ex.StackTrace.ToString());
                        }
                    }
                }
            }
        }
    }
}
