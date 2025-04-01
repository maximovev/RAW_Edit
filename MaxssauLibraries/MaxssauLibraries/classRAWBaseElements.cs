using image_designer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxssauLibraries
{
    public enum RAW_Converter_Output_Type
    {
        RAW_Output_RGB = 0,
        RAW_Output_LAB = 1,
        RAW_Output_XYZ = 2,
        RAW_Output_HSV = 3
    }

    public struct RAW_ConversionSetup
    {
        public bool HasDCPData;
    }

    public struct InputRAWImage
    {
        public RGB_Pixel[,] Image_Input_RAW_RGB;
        public RGB_MinMaxValues Image_Input_MinMaxLevels;
        public int ImageWidth;
        public int ImageHeight;
    }

    public struct ImagePack
    {
        public RGB_Pixel[,] Image_RGB;
        public XYZ_Pixel[,] Image_XYZ;
        public LAB_Pixel[,] Image_LAB;
        public HSV_Pixel[,] Image_HSV;
        public RGB_MinMaxValues RGB_MinMax;
        public XYZ_MinMaxValues XYZ_MinMax;
        public HSV_MinMaxValues HSV_MinMax;
        public LAB_MinMaxValues LAB_MinMax;
    }

    public struct WhiteBalanceRGBAvg
    {
        private double R_summ;
        private double G_summ;
        private double B_summ;

        private double summ;

        private double R_coeff;
        private double G_coeff;
        private double B_coeff;

        public double GetRCoeff()
        {
            double max = GetMax();
            if (GetMax() != 0)
            {
                return R_coeff / GetMax();
            }
            else
            {
                return 1;
            }
        }

        public double GetGCoeff()
        {
            double max = GetMax();
            if (GetMax() != 0)
            {
                return G_coeff / GetMax();
            }
            else
            {
                return 1;
            }
        }

        public double GetBCoeff()
        {
            double max = GetMax();
            if (GetMax() != 0)
            {
                return B_coeff / GetMax();
            }
            else
            {
                return 1;
            }
        }

        private double GetMax()
        {
            return Math.Max(Math.Max(R_summ, G_summ), B_summ);
        }

        public void add(double R, double G, double B)
        {
            R_summ = R_summ + R;
            G_summ = G_summ + G;
            B_summ = B_summ + B;

            summ = summ + R + G + B;
        }

        public void clear()
        {
            R_summ = 0;
            G_summ = 0;
            B_summ = 0;
        }
    }

    public class RGB_Histogram : ClassAddToLog
    {
        private string ModuleName = "RGB Histogram";

        public classHistogramBuilder R;
        public classHistogramBuilder G;
        public classHistogramBuilder B;
        public classHistogramBuilder RGB;
        public OperationStatus Calc()
        {
            try
            {
                R.CalculateBins();
                G.CalculateBins();
                B.CalculateBins();
                RGB.CalculateBins();
                return OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                AddToLog(ex, ModuleName);
                return OperationStatus.STATUS_FAIL;
            }
        }

        public OperationStatus clear()
        {
            try
            {
                R.Reset();
                G.Reset();
                B.Reset();
                RGB.Reset();
                return OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                AddToLog(ex, ModuleName);
                return OperationStatus.STATUS_FAIL;
            }
        }

        public RGB_Histogram(ClassLogger logger)
        {
            Logger = logger;
            R = new classHistogramBuilder(logger);
            G = new classHistogramBuilder(logger);
            B = new classHistogramBuilder(logger);
            RGB = new classHistogramBuilder(logger);

            R.SetBinsCount(256);
            G.SetBinsCount(256);
            B.SetBinsCount(256);
        }
    }

    public class HSV_Histogram : ClassAddToLog
    {
        private string ModuleName = "HSV Histogram";

        public classHistogramBuilder H;
        public classHistogramBuilder S;
        public classHistogramBuilder V;

        public OperationStatus Calc()
        {
            try
            {
                H.CalculateBins();
                S.CalculateBins();
                V.CalculateBins();
                return OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                AddToLog(ex, ModuleName);
                return OperationStatus.STATUS_FAIL;
            }
        }

        public OperationStatus clear()
        {
            try
            {
                H.Reset();
                S.Reset();
                V.Reset();
                return OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                AddToLog(ex, ModuleName);
                return OperationStatus.STATUS_FAIL;
            }
        }

        public HSV_Histogram(ClassLogger logger)
        {
            Logger = logger;
            H = new classHistogramBuilder(logger);
            S = new classHistogramBuilder(logger);
            V = new classHistogramBuilder(logger);

            H.SetBinsCount(360);
            S.SetBinsCount(100);
            V.SetBinsCount(100);
        }
    }

    public enum BitDepthCoeff
    {
        RAW_12Bit = 4, RAW_14Bit = 16
    }

    public struct RAW_ConversionStages
    {
        public bool LinearizeData;
        public bool WhiteBalanceCorrection;
        public bool ClipImageData;
        public bool Demosaic;
        public bool ColorTransform;
        public bool GammaCorrection;
        public bool BlackSubstract;
        public bool UseCMInsteadDCP;
        public bool ApplyGamma;
        public bool UserBlackLevel;
        public bool UseHighLightReconstructuion;
    }

    internal class classRAWBaseElements
    {
    }
}
