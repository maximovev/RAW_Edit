using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using image_designer;

namespace MaxssauLibraries
{
    public class classHistogramBuilder: ClassAddToLog
    {
        private string ModuleName = "Histogram Builder";

        private List<double> Values=new List<double>();

        public int[] result;

        public classHistogramBuilder(ClassLogger logger)
        {
            Logger = logger;
            result=new int[1];
        }

        public OperationStatus SetBinsCount(int bins)
        {
            if (bins > 0)
            {
                BinsCount = bins;
                return OperationStatus.STATUS_OK;
            }
            else
            {
                BinsCount = 1;
                return OperationStatus.STATUS_FAIL;
            }
        }

        private int BinsCount=256;

        public OperationStatus CalculateBins()
        {
            try
            {
                double bins_width=(Values.Max()-Values.Min())/BinsCount;

                result = new int[BinsCount];

                foreach (var value in Values)
                {
                    int binIndex = (int)(value / bins_width);
                    if(binIndex < BinsCount)
                    {
                        result[binIndex]++;
                    }
                }

                return OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                AddToLog(ex, ModuleName);
                return OperationStatus.STATUS_FAIL;
            }
        }

        public OperationStatus Reset()
        {
            try
            {
                Values.Clear();
                return OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                AddToLog(ex, ModuleName);
                return OperationStatus.STATUS_FAIL;
            }
        }

        public OperationStatus AddValue(double value)
        {
            try
            {
                Values.Add(value);
                return OperationStatus.STATUS_OK;
            }
            catch (Exception ex)    
            {
                AddToLog(ex, ModuleName);
                return OperationStatus.STATUS_FAIL;
            }
        }
    }
}
