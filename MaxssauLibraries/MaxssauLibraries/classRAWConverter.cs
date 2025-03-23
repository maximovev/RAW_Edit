using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using image_designer;
using SixLabors.ImageSharp.PixelFormats;

namespace MaxssauLibraries
{

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
    }

    public enum RAW_Converter_Output_Type
    {
        RAW_Output_RGB=0,
        RAW_Output_LAB=1,
        RAW_Output_XYZ=2,
        RAW_Output_HSV=3
    }

    public struct RAW_ConversionSetup
    {
        public bool HasDCPData;
    }

    public struct InputRAWImage
    {
        public RGB_Pixel[,]                     Image_Input_RAW_RGB;
        public RGB_MinMaxValues                 Image_Input_MinMaxLevels;
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

    public class classRAWConverter
    {
        public classDCPXMLReader               DCP_data;

        public RAW_ConversionStages             ConversionStageSetup=new RAW_ConversionStages();
        public RAW_ConversionSetup              ConversionSetup=new RAW_ConversionSetup();

        public RAW_Converter_Output_Type        OutputType;

        private classLogger                     Logger;

        public InputRAWImage                    RawImage = new InputRAWImage();

        public ImagePack                        ImageOutput = new ImagePack();
        public ImagePack                        ImageTemp = new ImagePack();

        public WhiteBalanceRGBAvg               WB_rgb =new WhiteBalanceRGBAvg();

        private classColorConversion            ColorConverter=new classColorConversion();

        public classXMLCMReader                 DCP_CM_Settings;

        public BitDepthCoeff RAW_bitdepth_coeff = new BitDepthCoeff();
        public int                              CM_SelectedProfile=0;

        public int BlackLevel_User = 10000;

        public  OperationStatus RAW_Process()
        {
            try
            {
                //if (DCP_data.HasDCPData.HasColorMatrix1 == true)
                if(true)
                {
                    ImageOutput = new ImagePack();
                    if (RawImage.Image_Input_RAW_RGB != null)
                    {
                        if (RawImage.Image_Input_RAW_RGB.Length > 0)
                        {
                            ImageOutput.Image_RGB = new RGB_Pixel[RawImage.ImageWidth, RawImage.ImageHeight];
                            ImageOutput.Image_XYZ = new XYZ_Pixel[RawImage.ImageWidth, RawImage.ImageHeight];

                            RGB_MinMaxValues BlackSubstractRGB_MinMax = new RGB_MinMaxValues();
                            RGB_MinMaxValues WBRGB_MinMax = new RGB_MinMaxValues();

                            BlackSubstractRGB_MinMax.reset();
                            WBRGB_MinMax.reset();

                            WB_rgb.clear();

                            for (int x = 0; x < RawImage.ImageWidth; x++)
                            {
                                for (int y = 0; y < RawImage.ImageHeight; y++)
                                {
                                    if (ConversionStageSetup.BlackSubstract == true)
                                    {
                                        if (ConversionStageSetup.UserBlackLevel == false)
                                        {
                                            ImageOutput.Image_RGB[x, y].R = RawImage.Image_Input_RAW_RGB[x, y].R - RawImage.Image_Input_MinMaxLevels.R.get_min();
                                            ImageOutput.Image_RGB[x, y].G = RawImage.Image_Input_RAW_RGB[x, y].G - RawImage.Image_Input_MinMaxLevels.G.get_min();
                                            ImageOutput.Image_RGB[x, y].B = RawImage.Image_Input_RAW_RGB[x, y].B - RawImage.Image_Input_MinMaxLevels.B.get_min();
                                        }
                                        else
                                        {
                                            ImageOutput.Image_RGB[x, y].R = RawImage.Image_Input_RAW_RGB[x, y].R - BlackLevel_User;
                                            ImageOutput.Image_RGB[x, y].G = RawImage.Image_Input_RAW_RGB[x, y].G - BlackLevel_User;
                                            ImageOutput.Image_RGB[x, y].B = RawImage.Image_Input_RAW_RGB[x, y].B - BlackLevel_User;
                                        }

                                        BlackSubstractRGB_MinMax.R.calc(ImageOutput.Image_RGB[x, y].R);
                                        BlackSubstractRGB_MinMax.G.calc(ImageOutput.Image_RGB[x, y].G);
                                        BlackSubstractRGB_MinMax.B.calc(ImageOutput.Image_RGB[x, y].B);
                                    }

                                    WB_rgb.add(ImageOutput.Image_RGB[x, y].R, ImageOutput.Image_RGB[x, y].G, ImageOutput.Image_RGB[x, y].B);

                                    int selected_profile = CM_SelectedProfile;
                                    if(selected_profile>= DCP_CM_Settings.values.Count)
                                    {
                                        selected_profile = 0;
                                    }
                                    if (ConversionStageSetup.WhiteBalanceCorrection == true)
                                    {
                                        ImageOutput.Image_RGB[x, y].R = ImageOutput.Image_RGB[x, y].R * DCP_CM_Settings.values[selected_profile].WB_coeff[0];
                                        ImageOutput.Image_RGB[x, y].G = ImageOutput.Image_RGB[x, y].G * DCP_CM_Settings.values[selected_profile].WB_coeff[1];
                                        ImageOutput.Image_RGB[x, y].B = ImageOutput.Image_RGB[x, y].B * DCP_CM_Settings.values[selected_profile].WB_coeff[2];
                                    }

                                    if (ConversionStageSetup.ClipImageData == true)
                                    {
                                        double bit_depth_coeff = 0;
                                        double bit_depth_max_level = 0;

                                        switch (RAW_bitdepth_coeff)
                                        {
                                            case BitDepthCoeff.RAW_12Bit:
                                                {
                                                    bit_depth_coeff = 16;
                                                    bit_depth_max_level = 1024*1024;
                                                }
                                                break;
                                             case BitDepthCoeff.RAW_14Bit:
                                                {
                                                    bit_depth_coeff = 4;
                                                    bit_depth_max_level = 262144;
                                                }
                                                break;
                                        }

                                        ImageOutput.Image_RGB[x, y].R = Math.Min(bit_depth_max_level, ImageOutput.Image_RGB[x, y].R * bit_depth_coeff);
                                        ImageOutput.Image_RGB[x, y].G = Math.Min(bit_depth_max_level, ImageOutput.Image_RGB[x, y].G * bit_depth_coeff);
                                        ImageOutput.Image_RGB[x, y].B = Math.Min(bit_depth_max_level, ImageOutput.Image_RGB[x, y].B * bit_depth_coeff);                                        
                                    }

                                    if(ConversionStageSetup.ColorTransform == true)
                                    {
                                        double[] result = new double[3];
                                        double[] pixel = new double[3];
                                        double[,] cm_data = new double[3,3];

                                        pixel[0] = ImageOutput.Image_RGB[x, y].R;
                                        pixel[1] = ImageOutput.Image_RGB[x, y].G;
                                        pixel[2] = ImageOutput.Image_RGB[x, y].B;

                                        for(int i = 0; i < 3; i++)
                                        {
                                            for (int j = 0; j < 3; j++)
                                            {
                                                cm_data[i, j] = DCP_CM_Settings.values[selected_profile].ColorMatrix[i, j];
                                            }
                                        }
                                        
                                        ColorConverter.MulMatrix3x3withM3(ref cm_data, ref result, pixel);

                                        ImageOutput.Image_RGB[x, y].R = result[0];
                                        ImageOutput.Image_RGB[x, y].G = result[1];
                                        ImageOutput.Image_RGB[x, y].B = result[2];
                                    }
                                }
                            }

                            ColorConverter.NormalizeImageTo1(ref ImageOutput.Image_RGB, RawImage.ImageWidth, RawImage.ImageHeight);

                            if (ConversionStageSetup.ApplyGamma == true)
                            {
                                for (int x = 0; x < RawImage.ImageWidth; x++)
                                {
                                    for (int y = 0; y < RawImage.ImageHeight; y++)
                                    {
                                        ImageOutput.Image_RGB[x, y].R = ColorConverter.RGB_to_sRGB(Math.Min(1, (Math.Max(0,ImageOutput.Image_RGB[x, y].R))));
                                        ImageOutput.Image_RGB[x, y].G = ColorConverter.RGB_to_sRGB(Math.Min(1, (Math.Max(0, ImageOutput.Image_RGB[x, y].G))));
                                        ImageOutput.Image_RGB[x, y].B = ColorConverter.RGB_to_sRGB(Math.Min(1, (Math.Max(0, ImageOutput.Image_RGB[x, y].B))));
                                    }
                                }
                            }
                            //ImageOutput.Image_RGB = ImageTemp.Image_RGB;

                            /*double coeff = Math.Min(BlackSubstractRGB_MinMax.R.get_max(), Math.Min(BlackSubstractRGB_MinMax.G.get_max(), BlackSubstractRGB_MinMax.B.get_max()));

                            double[] temp_rgb_in=new double[3];
                            double[] temp_rgb_out = new double[3];


                            for (int x = 0; x < RawImage.ImageWidth; x++)
                            {
                                for (int y = 0; y < RawImage.ImageHeight; y++)
                                {
                                    if (ConversionStageSetup.WhiteBalanceCorrection == true)
                                    {
                                        ImageTemp.Image_RGB[x, y].R = (ImageTemp.Image_RGB[x, y].R / coeff) / (WB_rgb.GetRCoeff());
                                        ImageTemp.Image_RGB[x, y].G = (ImageTemp.Image_RGB[x, y].G / coeff) / (WB_rgb.GetGCoeff());
                                        ImageTemp.Image_RGB[x, y].B = (ImageTemp.Image_RGB[x, y].B / coeff) / (WB_rgb.GetBCoeff());
                                    }

                                    // trunc data
                                    ImageTemp.Image_RGB[x, y].R = Math.Min(1, ImageTemp.Image_RGB[x, y].R);
                                    ImageTemp.Image_RGB[x, y].G = Math.Min(1, ImageTemp.Image_RGB[x, y].G);
                                    ImageTemp.Image_RGB[x, y].B = Math.Min(1, ImageTemp.Image_RGB[x, y].B);

                                    if(ConversionStageSetup.ColorTransform==true)
                                    {
                                        temp_rgb_in[0] = ImageTemp.Image_RGB[x, y].R;
                                        temp_rgb_in[1] = ImageTemp.Image_RGB[x, y].G;
                                        temp_rgb_in[2] = ImageTemp.Image_RGB[x, y].B;
                                        ColorConverter.MulMatrix3x3withM3(ref DCP_data.ColorMatrix1.coeff, ref temp_rgb_out, temp_rgb_in);
                                        ImageTemp.Image_XYZ[x, y].X = temp_rgb_out[0];
                                        ImageTemp.Image_XYZ[x, y].Y = temp_rgb_out[1];
                                        ImageTemp.Image_XYZ[x, y].Z = temp_rgb_out[2];
                                    }
                                }
                            }*/



                            return OperationStatus.STATUS_OK;
                        }
                        else
                        {
                            return OperationStatus.STATUS_FAIL;
                        }
                    }
                    else
                    {
                        return OperationStatus.STATUS_FAIL;
                    }
                }
                else
                {
                    return OperationStatus.STATUS_FAIL;
                }
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.add_to_log("RAW Converter: conversion", ex.Message);
                    if (ex.StackTrace != null)
                    {
                        Logger.add_to_log("RAW Converter: conversion", ex.StackTrace);
                    }
                }
                return OperationStatus.STATUS_FAIL;
            }
        }

        public OperationStatus LoadDCPData(string filename)
        {
            try
            {
                if (filename != null)
                {
                    if (filename != "")
                    {
                        DCP_data = new classDCPXMLReader(filename);
                        if (DCP_data.HasDCPData.HasColorMatrix1 == true)
                        {
                            ConversionSetup.HasDCPData = true;
                            return OperationStatus.STATUS_OK;
                        }
                        else
                        {
                            return OperationStatus.STATUS_FAIL;
                        }
                    }
                    else
                    {
                        return OperationStatus.STATUS_FAIL;
                    }
                }
                else
                {
                    return OperationStatus.STATUS_FAIL;
                }
            }
            catch (Exception ex)
            {
                if(Logger != null)
                {
                    Logger.add_to_log("RAW Converter: DCP Loader", ex.Message);
                    if(ex.StackTrace != null)
                    {
                        Logger.add_to_log("RAW Converter: DCP Loader", ex.StackTrace);
                    }
                }
                return OperationStatus.STATUS_FAIL;
            }
        }

        public classRAWConverter(ref classLogger logger)
        {
            Logger = logger;
        }

        
    }
}
