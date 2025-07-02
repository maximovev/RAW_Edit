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
                            if (RawImage.Image_Input_MinMaxLevels.G.get_max() > 0 && RawImage.Image_Input_MinMaxLevels.G.get_min() >= 0)
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

                                            ImageOutput.Image_RGB[x, y].R = ImageOutput.Image_RGB[x, y].R * RawImage.pre_mul[0];
                                            ImageOutput.Image_RGB[x, y].G1 = ImageOutput.Image_RGB[x, y].G1 * RawImage.pre_mul[1];
                                            ImageOutput.Image_RGB[x, y].B = ImageOutput.Image_RGB[x, y].B * RawImage.pre_mul[2];
                                            ImageOutput.Image_RGB[x, y].G2 = ImageOutput.Image_RGB[x, y].G2 * RawImage.pre_mul[3];

                                            ImageOutput.RGB_MinMax.R.calc(ImageOutput.Image_RGB[x, y].R);
                                            ImageOutput.RGB_MinMax.G1.calc(ImageOutput.Image_RGB[x, y].G1);
                                            ImageOutput.RGB_MinMax.B.calc(ImageOutput.Image_RGB[x, y].B);
                                            ImageOutput.RGB_MinMax.G2.calc(ImageOutput.Image_RGB[x, y].G2);
                                        }
                                    });

                                    clip_level_min = Math.Min(Math.Min(ImageOutput.RGB_MinMax.R.get_max(), ImageOutput.RGB_MinMax.B.get_max()), Math.Min(ImageOutput.RGB_MinMax.G1.get_max(), ImageOutput.RGB_MinMax.G2.get_max()));

                                    Matrix rgb_data = new Matrix(4, 1);
                                    Matrix cm_data = new Matrix(4, 3);
                                    Matrix out_data = new Matrix(4, 1);

                                    for (int i = 0; i < 4; i++)
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

                                            if (ImageOutput.Image_RGB[x, y].R > clip_level_min)
                                            {
                                                ImageOutput.Image_RGB[x, y].R = clip_level_min;
                                            }
                                            if (ImageOutput.Image_RGB[x, y].B > clip_level_min)
                                            {
                                                ImageOutput.Image_RGB[x, y].B = clip_level_min;
                                            }
                                            if (ImageOutput.Image_RGB[x, y].G1 > clip_level_min)
                                            {
                                                ImageOutput.Image_RGB[x, y].G1 = clip_level_min;
                                            }
                                            if (ImageOutput.Image_RGB[x, y].G2 > clip_level_min)
                                            {
                                                ImageOutput.Image_RGB[x, y].G2 = clip_level_min;
                                            }

                                            ImageOutput.Image_RGB[x, y].R = ImageOutput.Image_RGB[x, y].R / clip_level_min;
                                            ImageOutput.Image_RGB[x, y].G1 = ImageOutput.Image_RGB[x, y].G1 / clip_level_min;
                                            ImageOutput.Image_RGB[x, y].B = ImageOutput.Image_RGB[x, y].B / clip_level_min;
                                            ImageOutput.Image_RGB[x, y].G2 = ImageOutput.Image_RGB[x, y].G2 / clip_level_min;

                                            rgb_data[0, 0] = ImageOutput.Image_RGB[x, y].R;
                                            rgb_data[0, 1] = ImageOutput.Image_RGB[x, y].G1;
                                            rgb_data[0, 2] = ImageOutput.Image_RGB[x, y].B;
                                            rgb_data[0, 3] = ImageOutput.Image_RGB[x, y].G2;

                                            out_data = rgb_data * cm_data;

                                            ImageOutput.Image_RGB[x, y].R = out_data[0, 0];
                                            ImageOutput.Image_RGB[x, y].G1 = out_data[0, 1];
                                            ImageOutput.Image_RGB[x, y].B = out_data[0, 2];
                                            ImageOutput.Image_RGB[x, y].G2 = out_data[0, 3];

                                            ImageOutput.RGB_MinMax.R.calc(ImageOutput.Image_RGB[x, y].R);
                                            ImageOutput.RGB_MinMax.G1.calc(ImageOutput.Image_RGB[x, y].G1);
                                            ImageOutput.RGB_MinMax.B.calc(ImageOutput.Image_RGB[x, y].B);
                                            ImageOutput.RGB_MinMax.G2.calc(ImageOutput.Image_RGB[x, y].G2);
                                        }
                                    });

                                    clip_level_min = Math.Min(Math.Min(ImageOutput.RGB_MinMax.R.get_max(), ImageOutput.RGB_MinMax.B.get_max()), Math.Min(ImageOutput.RGB_MinMax.G1.get_max(), ImageOutput.RGB_MinMax.G2.get_max()));

                                    Parallel.For(0, RawImage.ImageWidth, x =>
                                    {
                                        for (int y = 0; y < RawImage.ImageHeight; y++)
                                        {
                                            
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
