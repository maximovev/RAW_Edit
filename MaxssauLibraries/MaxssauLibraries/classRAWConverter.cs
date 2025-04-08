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

    

    

    


    public class classRAWConverter: ClassAddToLog
    {
        private string ModuleName = "RAW Converter v0.1";


        public classDCPXMLReader               DCP_data;

        public RAW_ConversionStages             ConversionStageSetup=new RAW_ConversionStages();
        public RAW_ConversionSetup              ConversionSetup=new RAW_ConversionSetup();

        public RAW_Converter_Output_Type        OutputType;

        public InputRAWImage                    RawImage = new InputRAWImage();

        public ImagePack                        ImageOutput = new ImagePack();
        public ImagePack                        ImageTemp = new ImagePack();

        public WhiteBalanceRGBAvg               WB_rgb =new WhiteBalanceRGBAvg();

        private classColorConversion            ColorConverter=new classColorConversion();

        public classXMLCMReader                 DCP_CM_Settings;

        public BitDepthCoeff RAW_bitdepth_coeff = new BitDepthCoeff();
        public int                              CM_SelectedProfile=0;

        public int BlackLevel_User = 10000;

        public RGB_Histogram rgb_histogram_output;
        public HSV_Histogram hsv_histogram_output;


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

                            BlackSubstractRGB_MinMax.Reset();
                            WBRGB_MinMax.Reset();

                            double bit_depth_max_level = 0;

                            double raw_min_level = Math.Min(Math.Min(RawImage.Image_Input_MinMaxLevels.R.get_min(), RawImage.Image_Input_MinMaxLevels.G.get_min()) , RawImage.Image_Input_MinMaxLevels.B.get_min());

                            WB_rgb.clear();

                            for (int x = 0; x < RawImage.ImageWidth; x++)
                            {
                                for (int y = 0; y < RawImage.ImageHeight; y++)
                                {
                                    if (ConversionStageSetup.BlackSubstract == true)
                                    {
                                        if (ConversionStageSetup.UserBlackLevel == false)
                                        {
                                            ImageOutput.Image_RGB[x, y].R = RawImage.Image_Input_RAW_RGB[x, y].R - raw_min_level;
                                            ImageOutput.Image_RGB[x, y].G = RawImage.Image_Input_RAW_RGB[x, y].G - raw_min_level;
                                            ImageOutput.Image_RGB[x, y].B = RawImage.Image_Input_RAW_RGB[x, y].B - raw_min_level;
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
                                    else
                                    {
                                        ImageOutput.Image_RGB[x, y].R = RawImage.Image_Input_RAW_RGB[x, y].R;
                                        ImageOutput.Image_RGB[x, y].G = RawImage.Image_Input_RAW_RGB[x, y].G;
                                        ImageOutput.Image_RGB[x, y].B = RawImage.Image_Input_RAW_RGB[x, y].B;
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

                            if(ConversionStageSetup.UseHighLightReconstructuion==true)
                            {
                                classHighLightReconstruction hlr = new classHighLightReconstruction(Logger);

                                hlr.HighLightReconstruction(classHighLightReconstruction.HighLightReconstructionMode.ModeGrayFill, ref ImageOutput.Image_RGB, RawImage.ImageHeight, RawImage.ImageWidth, bit_depth_max_level);
                            }

                            ColorConverter.NormalizeImageTo1(ref ImageOutput.Image_RGB, RawImage.ImageWidth, RawImage.ImageHeight);

                            
                            rgb_histogram_output.clear();

                            double gamma = 2.4;
                            byte[] gammaTable = new byte[256];
                            for (int i = 0; i < 256; i++)
                            {
                                gammaTable[i] = (byte)Math.Min(255, (int)((255.0 * Math.Pow(i / 255.0, 1.0 / gamma)) + 0.5));
                            }


                            for (int x = 0; x < RawImage.ImageWidth; x++)
                            {
                                for (int y = 0; y < RawImage.ImageHeight; y++)
                                {
                                    if (ConversionStageSetup.ApplyGamma == true)
                                    {
                                        /*ImageOutput.Image_RGB[x, y].R = ColorConverter.RGB_to_sRGB(Math.Min(1, (Math.Max(0, ImageOutput.Image_RGB[x, y].R))));
                                        ImageOutput.Image_RGB[x, y].G = ColorConverter.RGB_to_sRGB(Math.Min(1, (Math.Max(0, ImageOutput.Image_RGB[x, y].G))));
                                        ImageOutput.Image_RGB[x, y].B = ColorConverter.RGB_to_sRGB(Math.Min(1, (Math.Max(0, ImageOutput.Image_RGB[x, y].B))));*/
                                        ImageOutput.Image_RGB[x, y].R = Math.Pow((Math.Min(1, (Math.Max(0, ImageOutput.Image_RGB[x, y].R)))), 1.0 / gamma);
                                        ImageOutput.Image_RGB[x, y].G = Math.Pow((Math.Min(1, (Math.Max(0, ImageOutput.Image_RGB[x, y].G)))), 1.0 / gamma);
                                        ImageOutput.Image_RGB[x, y].B = Math.Pow((Math.Min(1, (Math.Max(0, ImageOutput.Image_RGB[x, y].B)))), 1.0 / gamma);

                                    }
                                    else
                                    {
                                        ImageOutput.Image_RGB[x, y].R = Math.Min(1, (Math.Max(0, ImageOutput.Image_RGB[x, y].R)));
                                        ImageOutput.Image_RGB[x, y].G = Math.Min(1, (Math.Max(0, ImageOutput.Image_RGB[x, y].G)));
                                        ImageOutput.Image_RGB[x, y].B = Math.Min(1, (Math.Max(0, ImageOutput.Image_RGB[x, y].B)));
                                    }

                                    rgb_histogram_output.R.AddValue(ImageOutput.Image_RGB[x, y].R);
                                    rgb_histogram_output.G.AddValue(ImageOutput.Image_RGB[x, y].G);
                                    rgb_histogram_output.B.AddValue(ImageOutput.Image_RGB[x, y].B);
                                    rgb_histogram_output.RGB.AddValue((ImageOutput.Image_RGB[x, y].R + ImageOutput.Image_RGB[x, y].G + ImageOutput.Image_RGB[x, y].B) / 3);
                                }
                            }

                            rgb_histogram_output.Calc();
                            
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

                            GC.Collect();

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
                AddToLog(ex, ModuleName);
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
                AddToLog(ex, ModuleName);
                return OperationStatus.STATUS_FAIL;
            }
        }

        public classRAWConverter(ref ClassLogger logger)
        {
            Logger = logger;
            rgb_histogram_output = new RGB_Histogram(Logger);
        }

        
    }
}
