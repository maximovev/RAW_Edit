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

        public OperationStatus Calculate(ref RGB_Pixel rgb_pixel, int color_temperarue)
        {
            try
            {



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
