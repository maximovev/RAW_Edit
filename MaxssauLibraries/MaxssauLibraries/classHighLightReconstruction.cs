using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using image_designer;

namespace MaxssauLibraries
{
    public class classHighLightReconstruction
    {

        public classLogger Logger;

        public enum HighLightReconstructionMode
        {
            ModeNone=0,
            ModeGrayFill=1
        }

        public classHighLightReconstruction(classLogger logger)
        {
            Logger = logger;
        }

        private RGB_MinMaxValues minmax=new RGB_MinMaxValues();

        private double depth_max_value;

        OperationStatus GetMinMax(ref RGB_Pixel[,] data, ref RGB_MinMaxValues result, int height, int width)
        {
            try
            {
                result.reset();

                for(int x=0;x<width; x++)
                {
                    for (int y=0;y<height;y++)
                    {
                        result.R.calc(data[x, y].R);
                        result.G.calc(data[x, y].G);
                        result.B.calc(data[x, y].B);
                    }
                }

                return OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    if (Logger.status == classLogger.STATUS.OPEN)
                    {
                        Logger.add_to_log("HighLight Reconstruction", ex.Message);
                        if (ex.StackTrace != null)
                        {
                            Logger.add_to_log("HighLight Reconstruction", ex.StackTrace);
                        }
                    }
                }
                return OperationStatus.STATUS_FAIL;
            }
        }

        private OperationStatus HighLightReconstructionDoWork(HighLightReconstructionMode recovery_mode, ref RGB_Pixel[,] data, int height, int width)
        {
            try
            {
                switch (recovery_mode)
                {
                    case HighLightReconstructionMode.ModeGrayFill:
                        {
                            double min_value = Math.Min(Math.Min(minmax.R.get_max(), minmax.G.get_max()), minmax.B.get_max());
                            double max_value = Math.Max(Math.Max(minmax.R.get_max(), minmax.G.get_max()), minmax.B.get_max());

                            for (int x = 0; x < width; x++)
                            {
                                for (int y = 0; y < height; y++)
                                {
                                    double r = data[x, y].R;
                                    double g = data[x, y].G;
                                    double b = data[x, y].B;

                                    if (Math.Max(Math.Max(r, g), b) > depth_max_value)
                                    {
                                        // process only overload data

                                        if (data[x, y].R >= min_value || data[x, y].G >= min_value || data[x, y].B >= min_value)
                                        {



                                            double color_max = Math.Max(Math.Max(r, g), b);
                                            /* burning area
                                             * 
                                             * fill gray
                                            */
                                            /*data[x, y].R = Math.Max(Math.Max(data[x, y].R, data[x, y].G), data[x, y].B);
                                            data[x, y].G = Math.Max(Math.Max(data[x, y].R, data[x, y].G), data[x, y].B);
                                            data[x, y].B = Math.Max(Math.Max(data[x, y].R, data[x, y].G), data[x, y].B);*/

                                            /*data[x, y].R = Math.Min(Math.Min(data[x, y].R, data[x, y].G), data[x, y].B);
                                            data[x, y].G = Math.Min(Math.Min(data[x, y].R, data[x, y].G), data[x, y].B);
                                            data[x, y].B = Math.Min(Math.Min(data[x, y].R, data[x, y].G), data[x, y].B);*/

                                            data[x, y].R = r + (color_max - r) / 2;
                                            data[x, y].G = g + (color_max - g) / 2;
                                            data[x, y].B = b + (color_max - b) / 2;
                                        }
                                    }
                                }
                            }
                        }break;
                        case HighLightReconstructionMode.ModeNone:
                        {
                            return OperationStatus.STATUS_OK;
                        };
                }
                
                return OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    if (Logger.status == classLogger.STATUS.OPEN)
                    {
                        Logger.add_to_log("HighLight Reconstruction", ex.Message);
                        if (ex.StackTrace != null)
                        {
                            Logger.add_to_log("HighLight Reconstruction", ex.StackTrace);
                        }
                    }
                }
                return OperationStatus.STATUS_FAIL;
            }
        }

        public OperationStatus HighLightReconstruction(HighLightReconstructionMode recovery_mode, ref RGB_Pixel[,] data, int height, int width, ref RGB_MinMaxValues minmax_values, double depth_max)
        {
            try 
            {
                depth_max_value=depth_max;
                minmax=minmax_values;
                if (HighLightReconstructionDoWork(recovery_mode, ref data, height, width) == OperationStatus.STATUS_OK)
                {
                    return OperationStatus.STATUS_OK;
                }
            }
            catch(Exception ex)
            {
                if (Logger != null)
                {
                    if (Logger.status == classLogger.STATUS.OPEN)
                    {
                        Logger.add_to_log("HighLight Reconstruction", ex.Message);
                        if (ex.StackTrace != null)
                        {
                            Logger.add_to_log("HighLight Reconstruction", ex.StackTrace);
                        }
}
                }
                
            }
            return OperationStatus.STATUS_FAIL;
        }

        public OperationStatus HighLightReconstruction(HighLightReconstructionMode recovery_mode, ref RGB_Pixel[,] data, int height, int width, double depth_max)
        {
            try
            {
                depth_max_value = depth_max;
                if (GetMinMax(ref data, ref minmax, height, width) == OperationStatus.STATUS_OK)
                {
                    if(HighLightReconstructionDoWork(recovery_mode, ref data, height, width)==OperationStatus.STATUS_OK)
                    {
                        return OperationStatus.STATUS_OK;
                    }
                }
                return OperationStatus.STATUS_FAIL;
            }
            catch(Exception ex)
            {
                if (Logger != null)
                {
                    if (Logger.status == classLogger.STATUS.OPEN)
                    {
                        Logger.add_to_log("HighLight Reconstruction", ex.Message);
                        if (ex.StackTrace != null)
                        {
                            Logger.add_to_log("HighLight Reconstruction", ex.StackTrace);
                        }
                    }
                }
                return OperationStatus.STATUS_FAIL;
            }           
        }
    }
}
