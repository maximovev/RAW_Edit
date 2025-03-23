/*
 * Created by SharpDevelop.
 * User: maximove
 * Date: 18.12.2024
 * Time: 16:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Globalization;

namespace MaxssauLibraries
{
    /// <summary>
    /// Description of classDCPXMLReader.
    /// </summary>
    /// 

    public enum DataType
    {
        DATA_COLOR_MATRIX_1,
        DATA_COLOR_MATRIX_2,
        DATA_COLOR_MATRIX_3,
        DATA_FORWARD_MATRIX_1,
        DATA_FORWARD_MATRIX_2,
        DATA_FORWARD_MATRIX_3
    }

    public enum OperationStatus
    {
        STATUS_OK,
        STATUS_FAIL,
        STATUS_FILE_ERROR,
        STATUS_FILE_EMPITY
    }

    public class classDCPXMLReader
	{
		
		public struct ColorMatrix
		{
			public double[,] coeff;
		}

		public struct HueSatDeltas
		{
			public int hueDivisions;
			public int satDivisions;
            public int valDivisions;
            public int count;
            public double[,,] HueShift;
			public double[,,] SatScale;
			public double[,,] ValScale;
		}

		public struct Tone_Curve
		{ 
			public int count;
			public int[] N;
			public int[] h;
            public int[] v;
        }


		public struct _DCP_Data
		{
			public bool Valid;
			public bool NotEmpity;
			public bool HasColorMatrix1;
			public bool HasColorMatrix2;
			public bool HasColorMatrix3;
			public bool HasForwardMatrix1;
			public bool HasForwardMatrix2;
            public bool HasForwardMatrix3;
			public bool HasHueSatMap1;
            public bool HasHueSatMap2;
            public bool HasHueSatMap3;
			public bool HasLUT;
			public bool HasToneCurve;

        }
		
		public _DCP_Data HasDCPData=new classDCPXMLReader._DCP_Data();

        public ColorMatrix ColorMatrix1;
        public ColorMatrix ColorMatrix2;
		public ColorMatrix ColorMatrix3;
		public ColorMatrix ForwardMatrix1;
		public ColorMatrix ForwardMatrix2;
        public ColorMatrix ForwardMatrix3;
		public HueSatDeltas HueSatDeltas1;
        public HueSatDeltas HueSatDeltas2;
		public HueSatDeltas LUT;
		public Tone_Curve ToneCurve;

        public string ProfileName = "";

        public bool HasDataset(DataType type)
		{
			switch(type)
			{
				case DataType.DATA_COLOR_MATRIX_1:
					{
						return HasDCPData.HasColorMatrix1;
					};
				case DataType.DATA_COLOR_MATRIX_2:
					{
						return HasDCPData.HasColorMatrix2;
					};
				case DataType.DATA_COLOR_MATRIX_3:
					{
						return HasDCPData.HasColorMatrix3;
					};
				case DataType.DATA_FORWARD_MATRIX_1:
					{
						return HasDCPData.HasForwardMatrix1;
					};
				case DataType.DATA_FORWARD_MATRIX_2:
					{
						return HasDCPData.HasForwardMatrix2;
					};
			}
			return false;
		}

		void ReadHueSatDeltas(ref HueSatDeltas result, IEnumerable<XElement> node,ref int read_count)
		{
            
            foreach (var element in node)
			{
				foreach (var attrib in element.Attributes())
				{
					switch (attrib.Name.ToString())
					{
						case "hueDivisions":
							{
								result.hueDivisions = int.Parse(attrib.Value);
							}
							break;
						case "satDivisions":
							{
								result.satDivisions = int.Parse(attrib.Value);
							}
							break;
						case "valDivisions":
							{
								result.valDivisions = int.Parse(attrib.Value);
							}
							break;
					}
				}

				result.HueShift = new double[result.hueDivisions, result.satDivisions, result.valDivisions];
                result.SatScale = new double[result.hueDivisions, result.satDivisions, result.valDivisions];
                result.ValScale = new double[result.hueDivisions, result.satDivisions, result.valDivisions];

				int huediv = 0;
                int satdiv = 0;
                int valdiv = 0;
				double HueShift = 0;
                double SatScale = 0;
                double ValScale = 0;


                foreach (var values in node.Elements())
				{
					foreach(var attrib in values.Attributes())
					{
						switch (attrib.Name.ToString())
						{
							case "HueDiv":
								{
									huediv = int.Parse(attrib.Value);
								}break;
                            case "SatDiv":
                                {
                                    satdiv = int.Parse(attrib.Value);
                                }
                                break;
                            case "ValDiv":
                                {
                                    valdiv = int.Parse(attrib.Value);
                                }
                                break;
							case "HueShift":
								{
                                    HueShift = double.Parse(attrib.Value, new NumberFormatInfo
                                    {
                                        NumberDecimalSeparator = "."
                                    });
								}break;
                            case "SatScale":
                                {
                                    SatScale = double.Parse(attrib.Value, new NumberFormatInfo
                                    {
                                        NumberDecimalSeparator = "."
                                    });
                                }
                                break;
                            case "ValScale":
                                {
                                    ValScale = double.Parse(attrib.Value, new NumberFormatInfo
                                    {
                                        NumberDecimalSeparator = "."
                                    });
                                }
                                break;
                        }
					}

					result.HueShift[huediv, satdiv, valdiv] = HueShift;
                    result.SatScale[huediv, satdiv, valdiv] = SatScale;
                    result.ValScale[huediv, satdiv, valdiv] = ValScale;

                    read_count++;
				}

            }
		}

		void ReadMatrixValues(ref double [,] result, IEnumerable<XElement> node,ref int read_count)
		{
            foreach (var element in node)
            {
                int col = 0;
                int row = 0;
                double value = 0;
                foreach (var values in element.Elements())
                {
					read_count++;
                    foreach (var address in values.Attributes())
                    {
                        switch (address.Name.ToString())
                        {
                            case "Col":
                                {
                                    col = int.Parse(address.Value);
                                }
                                break;
                            case "Row":
                                {
                                    row = int.Parse(address.Value);
                                }
                                break;
                        }
                    }
					value = double.Parse(values.Value, new NumberFormatInfo
					{
						NumberDecimalSeparator = "."
					});
					result[row, col] = value;
                };
            }

        }

        public classDCPXMLReader(string filename)
		{
			XDocument doc=XDocument.Load(filename);

            ColorMatrix1.coeff = new double[3, 3];
            ColorMatrix2.coeff = new double[3, 3];
            ColorMatrix3.coeff = new double[3, 3];
            ForwardMatrix1.coeff = new double[3, 3];
            ForwardMatrix2.coeff = new double[3, 3];
            ForwardMatrix3.coeff = new double[3, 3];

			int read_count = 0;

            // load color matrix 1
            IEnumerable<XElement> cm1_data = doc.Descendants("ColorMatrix1");
            IEnumerable<XElement> cm2_data = doc.Descendants("ColorMatrix2");
            IEnumerable<XElement> cm3_data = doc.Descendants("ColorMatrix3");
            IEnumerable<XElement> fm1_data = doc.Descendants("ForwardMatrix1");
            IEnumerable<XElement> fm2_data = doc.Descendants("ForwardMatrix2");
            IEnumerable<XElement> fm3_data = doc.Descendants("ForwardMatrix3");

            IEnumerable<XElement> satdeltas1_data = doc.Descendants("HueSatDeltas1");
            IEnumerable<XElement> satdeltas2_data = doc.Descendants("HueSatDeltas2");

            IEnumerable<XElement> LUT_data = doc.Descendants("LookTable");

            IEnumerable<XElement> profile_name = doc.Descendants("ProfileName");

            read_count = 0;
            ReadHueSatDeltas(ref HueSatDeltas1, satdeltas1_data, ref read_count);
			if(read_count>0)
			{
				HasDCPData.HasHueSatMap1 = true;
			}

            read_count = 0;
            ReadHueSatDeltas(ref HueSatDeltas2, satdeltas2_data, ref read_count);
            if (read_count > 0)
            {
                HasDCPData.HasHueSatMap2 = true;
            }

            read_count = 0;
            ReadHueSatDeltas(ref LUT, LUT_data, ref read_count);
            if (read_count > 0)
            {
                HasDCPData.HasLUT = true;
            }

            read_count = 0;
            ReadMatrixValues(ref ColorMatrix1.coeff, cm1_data,ref read_count);
			if(read_count==9)
			{
				HasDCPData.HasColorMatrix1 = true;
			}

            read_count = 0;
            ReadMatrixValues(ref ColorMatrix2.coeff, cm2_data, ref read_count);
            if (read_count == 9)
            {
                HasDCPData.HasColorMatrix2 = true;
            }

            read_count = 0;
            ReadMatrixValues(ref ColorMatrix3.coeff, cm3_data, ref read_count);
            if (read_count == 9)
            {
                HasDCPData.HasColorMatrix3 = true;
            }

            read_count = 0;
            ReadMatrixValues(ref ForwardMatrix1.coeff, fm1_data, ref read_count);
            if (read_count == 9)
            {
                HasDCPData.HasForwardMatrix1 = true;
            }

            read_count = 0;
            ReadMatrixValues(ref ForwardMatrix2.coeff, fm2_data, ref read_count);
            if (read_count == 9)
            {
                HasDCPData.HasForwardMatrix2 = true;
            }

            read_count = 0;
            ReadMatrixValues(ref ForwardMatrix3.coeff, fm3_data, ref read_count);
            if (read_count == 9)
            {
                HasDCPData.HasForwardMatrix3 = true;
            }

			foreach(var p_name in profile_name)
			{
				// get last value
				ProfileName = p_name.Value;
			}

        }


        public void classDCPXMLReader1(string filename)
		{
			ColorMatrix1.coeff =new double[3,3];
			ColorMatrix2.coeff =new double[3,3];
			ColorMatrix3.coeff =new double[3,3];
			ForwardMatrix1.coeff =new double[3,3];
			ForwardMatrix2.coeff =new double[3,3];

			int row = 0;
			int col = 0;
			double value = 0;
			string node = "";
			string attribute="";
			
			if(File.Exists(filename))
			{
				using(XmlReader reader=XmlReader.Create(filename))
				{

					int line_counter=0;
					while(reader.Read())
					{
						if(reader.NodeType == XmlNodeType.Element)
						{
							node= reader.Name.ToLower();
							switch(node)
							{
								case "colormatrix1":
                                case "colormatrix2":
                                case "colormatrix3":
                                case "forwardmatrix1":
                                case "forwardmatrix2":
                                case "forwardmatrix3":
                                    {
										if (reader.HasAttributes)
										{
											attribute = reader.GetAttribute("Rows");
                                            row = int.Parse(attribute);
                                            attribute = reader.GetAttribute("Cols");
                                            col = int.Parse(attribute);

											for (int i = 0; i < row * col; i++)
											{
												reader.Read();
												if (reader.HasAttributes)
												{
													attribute = reader.GetAttribute("Row");
													row = int.Parse(attribute);
													attribute = reader.GetAttribute("Col");
													col = int.Parse(attribute);
													value = double.Parse(reader.Value);

													if (node.Contains("colormatrix"))
													{
														if (node.Contains("1"))
														{
															ColorMatrix1.coeff[col, row] = value;
															HasDCPData.HasColorMatrix1 = true;
														}
														if (node.Contains("2"))
														{
															ColorMatrix2.coeff[col, row] = value;
															HasDCPData.HasColorMatrix2 = true;
														}
														if (node.Contains("3"))
														{
															ColorMatrix3.coeff[col, row] = value;
															HasDCPData.HasColorMatrix3 = true;
														}
													}
												}
											}
										}

									}break;
							}
						}

						
						line_counter++;
					}
					
					if(line_counter==0)
					{
						Status= OperationStatus.STATUS_FILE_EMPITY;
						HasDCPData.NotEmpity=false;
					}
				}
			}
			else
			{
				Status= OperationStatus.STATUS_FILE_ERROR;
			}
		}
		
		XmlDocument xml_file=new XmlDocument();
		
		public OperationStatus Status;
		
		
		
		
	}
}
