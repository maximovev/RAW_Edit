using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using image_designer;
using SixLabors.ImageSharp.PixelFormats;

namespace MaxssauLibraries
{

    public class classRAWConverter : ClassAddToLog
    {
        private string ModuleName = "RAW Converter v0.1";


        public RAW_ConversionStages ConversionStageSetup = new RAW_ConversionStages();
        public RAW_ConversionSetup ConversionSetup = new RAW_ConversionSetup();

        public InputRAWImage RawImage = new InputRAWImage();

        private classColorConversion ColorConverter = new classColorConversion();

        public RGB_Histogram rgb_histogram_output;
        public HSV_Histogram hsv_histogram_output;

        public ImagePack ImageOutput = new ImagePack();
        public ImagePack ImageTemp = new ImagePack();

        public double gamma = 2.2f;

        public OperationStatus RAW_Process()
        {
            try
            {
                /* stage 0
                    * 
                    * Check input data
                    * 
                    * 
                    */

                if (RawImage.ImageHeight > 0)
                {
                    if (RawImage.ImageWidth > 0)
                    {
                        if (RawImage.Image_Input_MinMaxLevels.R.get_max() > 0 && RawImage.Image_Input_MinMaxLevels.R.get_min() >= 0)
                        {
                            if (RawImage.Image_Input_MinMaxLevels.G1.get_max() > 0 && RawImage.Image_Input_MinMaxLevels.G2.get_max() > 0)
                            {
                                if (RawImage.Image_Input_MinMaxLevels.B.get_max() > 0 && RawImage.Image_Input_MinMaxLevels.B.get_min() >= 0)
                                {
                                    /*
                                        * Prepare buffers
                                        * 
                                        */

                                    ImageOutput.Image_RGB = new RGB_Pixel[RawImage.ImageWidth, RawImage.ImageHeight];
                                    ImageOutput.Image_XYZ = new XYZ_Pixel[RawImage.ImageWidth, RawImage.ImageHeight];
                                    ImageOutput.Image_LAB = new LAB_Pixel[RawImage.ImageWidth, RawImage.ImageHeight];
                                    ImageOutput.Image_HSV = new HSV_Pixel[RawImage.ImageWidth, RawImage.ImageHeight];

                                    ImageOutput.RGB_MinMax = new RGB_MinMaxValues();

                                    double black_min_level = 0;

                                    double clip_level_min = 0;

                                    if (ConversionStageSetup.BlackSubstract == true)
                                    {
                                        black_min_level = Math.Min(Math.Min(RawImage.Image_Input_MinMaxLevels.R.get_min(), RawImage.Image_Input_MinMaxLevels.G1.get_min()), Math.Min(RawImage.Image_Input_MinMaxLevels.B.get_min(), RawImage.Image_Input_MinMaxLevels.G2.get_min()));
                                    }
                                    /*
                                    *  parallel processiong stage
                                    */

                                    ImageOutput.RGB_MinMax.Reset();

                                    Parallel.For(0, RawImage.ImageWidth, x =>
                                    {
                                        for (int y = 0; y < RawImage.ImageHeight; y++)
                                        {
                                            // black level substract
                                            ImageOutput.Image_RGB[x, y].R = (RawImage.Image_Input_RAW_RGB[x, y].R - black_min_level);
                                            ImageOutput.Image_RGB[x, y].G1 = (RawImage.Image_Input_RAW_RGB[x, y].G1 - black_min_level);
                                            ImageOutput.Image_RGB[x, y].G2 = (RawImage.Image_Input_RAW_RGB[x, y].G2 - black_min_level);
                                            ImageOutput.Image_RGB[x, y].B = (RawImage.Image_Input_RAW_RGB[x, y].B - black_min_level);
                                            ImageOutput.Image_RGB[x, y].G = ((ImageOutput.Image_RGB[x, y].G1 + ImageOutput.Image_RGB[x, y].G2) / 2);

                                            ImageOutput.Image_RGB[x, y].R = ImageOutput.Image_RGB[x, y].R * RawImage.pre_mul[0];
                                            ImageOutput.Image_RGB[x, y].G = ImageOutput.Image_RGB[x, y].G * RawImage.pre_mul[1];
                                            ImageOutput.Image_RGB[x, y].B = ImageOutput.Image_RGB[x, y].B * RawImage.pre_mul[2];

                                            ImageOutput.RGB_MinMax.R.calc(ImageOutput.Image_RGB[x, y].R);
                                            ImageOutput.RGB_MinMax.G.calc(ImageOutput.Image_RGB[x, y].G);
                                            ImageOutput.RGB_MinMax.B.calc(ImageOutput.Image_RGB[x, y].B);

                                        }
                                    });

                                    clip_level_min = Math.Min(Math.Min(ImageOutput.RGB_MinMax.R.get_max(), ImageOutput.RGB_MinMax.B.get_max()), ImageOutput.RGB_MinMax.G.get_max());

                                    double [,] cm_data = new double[3, 3];

                                    for (int i = 0; i < 3; i++)
                                    {
                                        for (int j = 0; j < 3; j++)
                                        {
                                            cm_data[i, j] = RawImage.rgb_cam_mul[i, j];
                                        }
                                    }

                                    ImageOutput.RGB_MinMax.Reset();

                                    Parallel.For(0, RawImage.ImageWidth, x =>
                                    {
                                        for (int y = 0; y < RawImage.ImageHeight; y++)
                                        {
                                            /*if (ImageOutput.Image_RGB[x, y].R < 0)
                                            {
                                                ImageOutput.Image_RGB[x, y].R = 0;
                                            }
                                            if (ImageOutput.Image_RGB[x, y].G < 0)
                                            {
                                                ImageOutput.Image_RGB[x, y].G = 0;
                                            }
                                            if (ImageOutput.Image_RGB[x, y].B < 0)
                                            {
                                                ImageOutput.Image_RGB[x, y].B = 0;
                                            }*/

                                            if (ImageOutput.Image_RGB[x, y].R > clip_level_min)
                                            {
                                                ImageOutput.Image_RGB[x, y].R = clip_level_min;
                                            }                                            
                                            if (ImageOutput.Image_RGB[x, y].B > clip_level_min)
                                            {
                                                ImageOutput.Image_RGB[x, y].B = clip_level_min;
                                            }                                            
                                            if (ImageOutput.Image_RGB[x, y].G > clip_level_min)
                                            {
                                                ImageOutput.Image_RGB[x, y].G = clip_level_min;
                                            }

                                            ImageOutput.Image_RGB[x, y].R = ImageOutput.Image_RGB[x, y].R / clip_level_min;
                                            ImageOutput.Image_RGB[x, y].G = ImageOutput.Image_RGB[x, y].G / clip_level_min;
                                            ImageOutput.Image_RGB[x, y].B = ImageOutput.Image_RGB[x, y].B / clip_level_min;

                                            double[] rgb_data = new double[3];                                            
                                            double[] out_data = new double[3];

                                            rgb_data[0] = ImageOutput.Image_RGB[x, y].R;
                                            rgb_data[1] = ImageOutput.Image_RGB[x, y].G;
                                            rgb_data[2] = ImageOutput.Image_RGB[x, y].B;
                                            
                                            ColorConverter.MulMatrix3x3withM3(ref cm_data, ref out_data, rgb_data);

                                            if (ConversionStageSetup.ConvertTosRGB == true)
                                            {
                                                ImageOutput.Image_RGB[x, y].R = ColorConverter.RGB_to_sRGB(out_data[0]);
                                                ImageOutput.Image_RGB[x, y].G = ColorConverter.RGB_to_sRGB(out_data[1]);
                                                ImageOutput.Image_RGB[x, y].B = ColorConverter.RGB_to_sRGB(out_data[2]);
                                            }
                                            else
                                            {
                                                ImageOutput.Image_RGB[x, y].R = out_data[0];
                                                ImageOutput.Image_RGB[x, y].G = out_data[1];
                                                ImageOutput.Image_RGB[x, y].B = out_data[2];
                                            }

                                            ImageOutput.RGB_MinMax.R.calc(ImageOutput.Image_RGB[x, y].R);
                                            ImageOutput.RGB_MinMax.G.calc(ImageOutput.Image_RGB[x, y].G);
                                            ImageOutput.RGB_MinMax.B.calc(ImageOutput.Image_RGB[x, y].B);

                                        }
                                    });

                                    clip_level_min = Math.Min(Math.Min(ImageOutput.RGB_MinMax.R.get_max(), ImageOutput.RGB_MinMax.B.get_max()), ImageOutput.RGB_MinMax.G.get_max());

                                    ImageOutput.RGB_MinMax.Reset();

                                    Parallel.For(0, RawImage.ImageWidth, x =>
                                    {
                                        for (int y = 0; y < RawImage.ImageHeight; y++)
                                        {
                                            if(ImageOutput.Image_RGB[x, y].R < 0)
                                            {
                                                ImageOutput.Image_RGB[x, y].R = 0;
                                            }
                                            if (ImageOutput.Image_RGB[x, y].R > clip_level_min)
                                            {
                                                ImageOutput.Image_RGB[x, y].R = clip_level_min;
                                                //ImageOutput.Image_RGB[x, y].R = 1;
                                            }
                                            if (ImageOutput.Image_RGB[x, y].B < 0)
                                            {
                                                ImageOutput.Image_RGB[x, y].B = 0;
                                            }
                                            if (ImageOutput.Image_RGB[x, y].B > clip_level_min)
                                            {
                                                ImageOutput.Image_RGB[x, y].B = clip_level_min;
                                                //ImageOutput.Image_RGB[x, y].B = 1;
                                            }
                                            if (ImageOutput.Image_RGB[x, y].G < 0)
                                            {
                                                ImageOutput.Image_RGB[x, y].G = 0;
                                            }
                                            if (ImageOutput.Image_RGB[x, y].G > clip_level_min)
                                            {
                                                ImageOutput.Image_RGB[x, y].G = clip_level_min;
                                                //ImageOutput.Image_RGB[x, y].G = 1;
                                            }

                                            ImageOutput.Image_RGB[x, y].R = ImageOutput.Image_RGB[x, y].R / clip_level_min;
                                            ImageOutput.Image_RGB[x, y].G = ImageOutput.Image_RGB[x, y].G / clip_level_min;
                                            ImageOutput.Image_RGB[x, y].B = ImageOutput.Image_RGB[x, y].B / clip_level_min;

                                            ImageOutput.Image_RGB[x, y].R = Math.Pow(ImageOutput.Image_RGB[x, y].R, 1/gamma);
                                            ImageOutput.Image_RGB[x, y].G = Math.Pow(ImageOutput.Image_RGB[x, y].G, 1/gamma);
                                            ImageOutput.Image_RGB[x, y].B = Math.Pow(ImageOutput.Image_RGB[x, y].B, 1/gamma);

                                            ImageOutput.RGB_MinMax.R.calc(ImageOutput.Image_RGB[x, y].R);
                                            ImageOutput.RGB_MinMax.G.calc(ImageOutput.Image_RGB[x, y].G);
                                            ImageOutput.RGB_MinMax.B.calc(ImageOutput.Image_RGB[x, y].B);

                                            /*rgb_histogram_output.R.AddValue(ImageOutput.Image_RGB[x, y].R);
                                            rgb_histogram_output.G.AddValue(ImageOutput.Image_RGB[x, y].G);
                                            rgb_histogram_output.B.AddValue(ImageOutput.Image_RGB[x, y].B);*/
                                        }
                                    });

                                    if (ConversionStageSetup.ApplyToneCurve == true)
                                    {

                                    }


                                }
                            }
                        }
                    }
                }

                return OperationStatus.STATUS_FAIL;
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
