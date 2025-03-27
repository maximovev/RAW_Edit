/*
 * Created by SharpDevelop.
 * User: maxss
 * Date: 25.05.2024
 * Time: 21:01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace image_designer
{
    /// <summary>
    /// Description of classColorConversion.
    /// </summary>
    /// 

    public struct RGB_MinMaxValues
    {
        public Double_MaxValue R;
        public Double_MaxValue G;
        public Double_MaxValue B;
        public void reset()
        {
            R.reset();
            G.reset();
            B.reset();
        }
    }

    public struct XYZ_MinMaxValues
    {
        public Double_MaxValue X;
        public Double_MaxValue Y;
        public Double_MaxValue Z;
    }

    public struct HSV_MinMaxValues
    {
        public Double_MaxValue H;
        public Double_MaxValue S;
        public Double_MaxValue V;
    }

    public struct LAB_MinMaxValues
    {
        public Double_MaxValue L;
        public Double_MaxValue a;
        public Double_MaxValue b;
    }

    

    public struct Double_MaxValue
    {
        private double max;
        private double min;
        public void calc_max(double value)
        {
            if (value > max)
            {
                max = value;
            }
        }
        public void calc_min(double value)
        {
            if (value < min)
            {
                
                min = value;
                
            }
        }

		public void calc(double value)
		{
			calc_max(value);
            calc_min(value);
        }

		public double get_max()
		{
			return max;
		}

		public double get_min()
		{
			return min;
		}

        public void reset()
        {
            max = double.MinValue;
            min = double.MaxValue;
        }
    }

    public struct RGB_Pixel
	{
		public double R;
		public double G;
		public double B;
	};
	public struct HSV_Pixel
	{
		public double H;
		public double S;
		public double V;
	};

    public struct XYZ_Pixel
    {
        public double X;
		public double Y;
        public double Z;
    };

	public struct LAB_Pixel
	{
        public double L;
        public double a;
        public double b;
    }

	public struct pixel
	{
		public RGB_Pixel rgb;
		public HSV_Pixel hsv;
	};

	
	public class classColorConversion
	{
        public void NormalizeImageTo1(ref double[,] data, int weight, int height)
        {
            double max = double.MinValue;
            double min = double.MaxValue;
            double delta;
            for (int x = 0; x < weight; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // search maximum and minimum
                    if (data[x, y] > max) { max = data[x, y]; };
                    if (data[x, y] < min) { min = data[x, y]; };
                }
            }

            delta = max - min;

            for (int x = 0; x < weight; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    data[x, y] = (data[x, y] - min) / delta;
                }
            }
        }

        public void NormalizeImageTo1(ref XYZ_Pixel[,] data, int weight, int height)
        {
            double max_x = double.MinValue;
            double min_x = double.MaxValue;
            double max_y = double.MinValue;
            double min_y = double.MaxValue;
            double max_z = double.MinValue;
            double min_z = double.MaxValue;

            double max;
            double min;

            double delta;

            for (int x = 0; x < weight; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // search maximum and minimum
                    if (data[x, y].X > max_x) { max_x = data[x, y].X; };
                    if (data[x, y].Y > max_y) { max_y = data[x, y].Y; };
                    if (data[x, y].Z > max_z) { max_z = data[x, y].Z; };

                    if (data[x, y].X < min_x) { min_x = data[x, y].X; };
                    if (data[x, y].Y < min_y) { min_y = data[x, y].Y; };
                    if (data[x, y].Z < min_z) { min_z = data[x, y].Z; };
                }
            }

            max = Math.Max(Math.Max(max_x, max_y), max_z);
            min = Math.Min(Math.Min(min_x, min_y), min_z);

            delta = max - min;

            for (int x = 0; x < weight; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    data[x, y].X = (data[x, y].X - min) / delta;
                    data[x, y].Y = (data[x, y].Y - min) / delta;
                    data[x, y].Z = (data[x, y].Z - min) / delta;
                }
            }
        }

        public void NormalizeImageTo1(ref RGB_Pixel[,] data, int weight, int height)
        {
            double max_r = double.MinValue;
            double min_r = double.MaxValue;
            double max_g = double.MinValue;
            double min_g = double.MaxValue;
            double max_b = double.MinValue;
            double min_b = double.MaxValue;

            double max;
            double min;

            double delta;

            for (int x = 0; x < weight; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // search maximum and minimum
                    if (data[x, y].R > max_r) { max_r = data[x, y].R; };
                    if (data[x, y].G > max_g) { max_g = data[x, y].G; };
                    if (data[x, y].B > max_b) { max_b = data[x, y].B; };

                    if (data[x, y].R < min_r) { min_r = data[x, y].R; };
                    if (data[x, y].G < min_g) { min_g = data[x, y].G; };
                    if (data[x, y].B < min_b) { min_b = data[x, y].B; };
                }
            }

            max = Math.Max(Math.Max(max_r, max_g), max_b);
            min = Math.Min(Math.Min(min_r, min_g), min_b);

            delta = max - min;

            for (int x = 0; x < weight; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    data[x, y].R = (data[x, y].R - min) / delta;
                    data[x, y].G = (data[x, y].G - min) / delta;
                    data[x, y].B = (data[x, y].B - min) / delta;
                }
            }
        }


        public void MulMatrix3x3withM3(ref double[,] input, ref double[] result, double[] M3)
        {
            for (int i = 0; i < 3; i++)
            {
                result[i] = M3[0] * input[i, 0] + M3[1] * input[i, 1] + M3[2] * input[i, 2];
            }
        }

        public double[,] CA_XYZ_A_to_D65_Bradfort = { { 0.8446965, -0.1179225, 0.3948108 }, { -0.1366303, 1.1041226, 0.1291718 }, { 0.0798489, -0.1348999, 3.1924009 } };
        public double[,] CA_XYZ_A_to_D50_Bradfort = { { 0.8779529, -0.0915288, 0.2566181 }, { -0.1117372, 1.0924325, 0.0851788 }, { 0.0502012, -0.0837636, 2.3994031 } };
        public double[,] CA_XYZ_A_to_D55_Bradfort = { { 0.8644459, -0.1021330, 0.3073182 }, { -0.1222890, 1.0982532, 0.1013945 }, { 0.0609732, -0.1022820, 2.6887535 } };
        public double CutFrom0To1(double value)
		{
			return Math.Min(1, Math.Max(0, value));
        }

        public double[,] InvertMatrix(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[,] augmented = new double[n, n * 2];



            // Initialize augmented matrix with the input matrix and the identity matrix
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmented[i, j] = matrix[i, j];
                    augmented[i, j + n] = (i == j) ? 1 : 0;
                }
            }

            // Apply Gaussian elimination
            for (int i = 0; i < n; i++)
            {
                int pivotRow = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (Math.Abs(augmented[j, i]) > Math.Abs(augmented[pivotRow, i]))
                    {
                        pivotRow = j;
                    }
                }

                if (pivotRow != i)
                {
                    for (int k = 0; k < 2 * n; k++)
                    {
                        double temp = augmented[i, k];
                        augmented[i, k] = augmented[pivotRow, k];
                        augmented[pivotRow, k] = temp;
                    }
                }

                if (Math.Abs(augmented[i, i]) < 1e-10)
                {
                    return null;
                }

                double pivot = augmented[i, i];
                for (int j = 0; j < 2 * n; j++)
                {
                    augmented[i, j] /= pivot;
                }

                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        double factor = augmented[j, i];
                        for (int k = 0; k < 2 * n; k++)
                        {
                            augmented[j, k] -= factor * augmented[i, k];
                        }
                    }
                }
            }

            double[,] result = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i, j] = augmented[i, j + n];
                }
            }

            return result;
        }

        public RGB_Pixel LAB2RGB(LAB_Pixel pixel)
        {
			RGB_Pixel result = new RGB_Pixel();

            double X, Y, Z, fX, fY, fZ;
            double RR, GG, BB;

            fY = Math.Pow((pixel.L + 16.0) / 116.0, 3.0);

            if (fY < 0.008856)
                fY = pixel.L / 903.3;
            Y = fY;

            if (fY > 0.008856)
                fY = Math.Pow(fY, 1.0 / 3.0);
            else
                fY = 7.787 * fY + 16.0 / 116.0;

            fX = pixel.a / 500.0 + fY;
            if (fX > 0.206893)
                X = Math.Pow(fX, 3.0);
            else
                X = (fX - 16.0 / 116.0) / 7.787;

            fZ = fY - pixel.b / 200.0;
            if (fZ > 0.206893)
                Z = Math.Pow(fZ, 3.0);
            else
                Z = (fZ - 16.0 / 116.0) / 7.787;

            X *= (0.950456);
            Y *= 1;
            Z *= (1.088754);

            RR = (3.240479 * X - 1.537150 * Y - 0.498535 * Z + 0.5);
            GG = (-0.969256 * X + 1.875992 * Y + 0.041556 * Z + 0.5);
            BB = (0.055648 * X - 0.204043 * Y + 1.057311 * Z + 0.5);

            result.R = (RR < 0 ? 0 : RR > 1 ? 1 : RR);
            result.G = (GG < 0 ? 0 : GG > 1 ? 1 : GG);
            result.B = (BB < 0 ? 0 : BB > 1 ? 1 : BB);

			return result;
        }

        public classColorConversion()
		{
		}
		
		public enum rgb_color_mode:int 
		{
			RGB_MODE_8BIT=0,
			RGB_MODE_16BIT=1
		};
		
		public rgb_color_mode color_mode;

        public double RGB_to_sRGB(double C)
        {
            if (Math.Abs(C) < 0.0031308)
            {
                return 12.92 * C;
            }
            return ((Math.Pow(C, 0.41666)*1.055)  - 0.055);
        }

        private double F(double t)
        {
            double Delta = 6.0 / 29.0;

            return t > Math.Pow(Delta, 3)
                ? Math.Pow(t, 1.0 / 3.0)
                : t / (3 * Delta * Delta) + 4.0 / 29.0;
        }

        public LAB_Pixel XYZ_to_LAB(XYZ_Pixel xyz_value)
        {
            double Xn = 0.95047;
            double Yn = 1.00000;
            double Zn = 1.08883;

            double fx = F(xyz_value.X / Xn);
            double fy = F(xyz_value.Y / Yn);
            double fz = F(xyz_value.Z / Zn);

            // Вычисление L*, a*, b*
            double l = 116.0 * fy - 16.0;
            double a = 500.0 * (fx - fy);
            double b = 200.0 * (fy - fz);

            LAB_Pixel result = new LAB_Pixel();

            result.L = l;
            result.a = a;
            result.b = b;

            return result;
        }

        public XYZ_Pixel LAB_to_XYZ(LAB_Pixel lab_value)
        {
            double l = lab_value.L;
            double a = lab_value.a;
            double b = lab_value.b;

            double delta = 6.0 / 29.0;

            double fy = (l + 16.0) / 116.0;
            double fx = a / 500.0 + fy;
            double fz = fy - b / 200.0;

            double x = fx > delta ? Math.Pow(fx, 3) : (fx - 16.0 / 116.0) * 3 * delta * delta;
            double y = l > (8.0 * delta) ? Math.Pow((l + 16.0) / 116.0, 3) : l / (3.0 * delta * delta);
            double z = fz > delta ? Math.Pow(fz, 3) : (fz - 16.0 / 116.0) * 3 * delta * delta;

            // D65 белая точка
            x *= 0.95047;
            y *= 1.00000;
            z *= 1.08883;

            XYZ_Pixel result = new XYZ_Pixel();

            result.X = x;
            result.Y = y;
            result.Z = z;

            return result;
        }

        public RGB_Pixel XYZ_to_sRGB(XYZ_Pixel xyz)
		{
			RGB_Pixel result = new RGB_Pixel();

			result.R = RGB_to_sRGB(3.2404542 * xyz.X - 1.5371385 * xyz.Y - 0.4985314 * xyz.Z);
			result.G = RGB_to_sRGB(-0.9692660 * xyz.X + 1.8760108 * xyz.Y + 0.0415560 * xyz.Z);
			result.B = RGB_to_sRGB(0.0556434 * xyz.X - 0.2040259 * xyz.Y + 1.0572252 * xyz.Z);

            return result;
		}

        public RGB_Pixel XYZ_to_RGB(XYZ_Pixel xyz)
        {
            RGB_Pixel result = new RGB_Pixel();

            result.R = (3.2404542 * xyz.X - 1.5371385 * xyz.Y - 0.4985314 * xyz.Z);
            result.G = (-0.9692660 * xyz.X + 1.8760108 * xyz.Y + 0.0415560 * xyz.Z);
            result.B = (0.0556434 * xyz.X - 0.2040259 * xyz.Y + 1.0572252 * xyz.Z);

            return result;
        }

        public RGB_Pixel HSV_to_RGB(HSV_Pixel pixel)
		{
            RGB_Pixel result=new RGB_Pixel();
            double fV=pixel.V;
			double fS=pixel.S;
			double fH=pixel.H;
			
			double fR=0;
			double fG=0;
			double fB=0;
			
			double fC = fV * fS; // Chroma
			double fHPrime = (fH / 60.0f) % 6.0f;
			double fX = fC * (1.0f - Math.Abs((fHPrime % 2.0f) - 1.0f));
			double fM = fV - fC;
			  
			if(0 <= fHPrime && fHPrime < 1.0f) 
			{
				fR = fC;
			  	fG = fX;
			    fB = 0;
			} 
			else if(1.0f <= fHPrime && fHPrime < 2.0f)
			{
			    fR = fX;
			    fG = fC;
			    fB = 0;
			} 
			else if(2.0f <= fHPrime && fHPrime < 3.0f) 
			{
			    fR = 0;
			    fG = fC;
			    fB = fX;
			} 
			else if(3.0f <= fHPrime && fHPrime < 4.0f) 
			{
			    fR = 0;
			    fG = fX;
			    fB = fC;
			} 
			else if(4.0f <= fHPrime && fHPrime < 5.0f) 
			{
			    fR = fX;
			    fG = 0;
			    fB = fC;
			} 
			else if(5.0f <= fHPrime && fHPrime < 6.0f) 
			{
			    fR = fC;
			    fG = 0;
			    fB = fX;
			} 
			else 
			{
			    fR = 0;
			    fG = 0;
			    fB = 0;
			}
			 
			fR += fM;
			fG += fM;
			fB += fM;
  			
            /*double coeff=this.GetCoeff();
  			
  			result.R=fB*coeff;
  			result.G=fR*coeff;
  			result.B=fG*coeff;*/

            /*result.R = fB;
            result.G = fR;
            result.B = fG;*/

            result.R = fR;
            result.G = fG;
            result.B = fB;


            return result;
		}

		private double GetCoeff()
		{
			switch(this.color_mode)
			{
				case rgb_color_mode.RGB_MODE_8BIT:
					{
						return 255;
					};
				case rgb_color_mode.RGB_MODE_16BIT:
					{
						return 65535;
					};
			}
			return 1;
		}

        
		
		public HSV_Pixel RGB_to_HSV(RGB_Pixel pixel)
		{
			HSV_Pixel result;
			
			double r=0;
			double b=0;
			double g=0;
			
			/*double coeff=0;
			
			switch(color_mode)
			{
				case rgb_color_mode.RGB_MODE_16BIT:
					{
						coeff=65535;
					}break;
				case rgb_color_mode.RGB_MODE_8BIT:
					{
						coeff=255;
					}break;
			}*/

            // convert to 0...1 value


            /*r=pixel.R/coeff;
			b=pixel.B/coeff;
			g=pixel.G/coeff;*/

            r = pixel.R;
			b = pixel.B;
			g = pixel.G;

            double Cmax = Math.Max(Math.Max(r,b),g);
			double Cmin = Math.Min(Math.Min(r,b),g);
			double delta = Cmax-Cmin;
			
			double H = 0;
			double S = 0;
			double V = 0;
			
			if(delta == 0)
			{
				H = 0;
			}
			else
			{
				if(Cmax == r)
				{
					H = (60.0f*(((g-b)/delta)+360.0f)%360.0f);
				}
				if(Cmax == g)
				{
					H = (60.0f*((2+(b-r)/delta)+120.0f)%360.0f);
				}
				if(Cmax == b)
				{
					H = (60.0f*((4+(r-g)/delta)+240.0f)%360.0f);
				}
			}
			
			if(Cmax == 0)
			{
				S = 0;
			}
			else
			{
				S = delta/Cmax;
			}
			
			V = Cmax;
			
			result.H = H;
			result.S = S;
			result.V = V;
			
			return result;
		}
		
	}
}
