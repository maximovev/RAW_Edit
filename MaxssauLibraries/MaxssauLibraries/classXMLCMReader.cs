using System.Xml.Linq;
using System.Globalization;

namespace MaxssauLibraries
{
    public struct CM_Value
    {
        public double[,] ColorMatrix;
        public string name;
        public double SMI;
        public double[] WB_coeff; 
    }


    public class classXMLCMReader
    {

        public OperationStatus Status;

        public List<CM_Value> values=new List<CM_Value>();

        public classXMLCMReader(string filename)
        {
            XDocument doc = XDocument.Load(filename);

            IEnumerable<XElement> camera_data = doc.Descendants("camera");

            int ReadCount = 0;

            ReadColorMatrix(ref values,camera_data,ref ReadCount);
        }

        private void ReadColorMatrix(ref List<CM_Value> result, IEnumerable<XElement> node,ref int ReadCount)
        {
            Status = OperationStatus.STATUS_FAIL;
            int col = 0;
            int row = 0;

            foreach (XElement element in node)
            {
                CM_Value item = new CM_Value();

                var ColorMatrix = element.Descendants("ColorMatrix");
                var SMI = element.Descendants("sensivity");
                var WB = element.Descendants("WBCoeff");

                foreach (var color in ColorMatrix)
                {
                    
                    col = 0; row=0;

                    foreach (var val in WB)
                    {

                        foreach (var attrib in WB.Attributes())
                        {
                            switch (attrib.Name.ToString())
                            {
                                case "Rows":
                                    {
                                        row = int.Parse(attrib.Value);
                                    }
                                    break;
                            };
                        }

                        item.WB_coeff = new double[row];

                        foreach (var elem in val.Elements())
                        {
                            foreach (var attrib in elem.Attributes())
                            {
                                switch (attrib.Name.ToString())
                                {
                                    case "Row":
                                        {
                                            row = int.Parse(attrib.Value);
                                        }
                                        break;
                                };
                            }
                            item.WB_coeff[row] = double.Parse(elem.Value, new NumberFormatInfo
                            {
                                NumberDecimalSeparator = "."
                            });
                        }
                    }


                    foreach (var attrib in color.Attributes())
                    {
                        switch (attrib.Name.ToString())
                        {
                            case "Cols":
                                {
                                    col = int.Parse(attrib.Value);
                                }
                                break;
                            case "Rows":
                                {
                                    row = int.Parse(attrib.Value);
                                }
                                break;
                        }
                    }
                    
                    item.ColorMatrix = new double[row,col];

                    foreach (var elem in color.Elements())
                    {
                        foreach (var attrib in elem.Attributes())
                        {
                            switch (attrib.Name.ToString())
                            {
                                case "Row":
                                    {
                                        row = int.Parse(attrib.Value);
                                    }
                                    break;
                                case "Col":
                                    {
                                        col = int.Parse(attrib.Value);
                                    }
                                    break;
                            }
                        }
                        item.ColorMatrix[row,col] = double.Parse(elem.Value, new NumberFormatInfo
                        {
                            NumberDecimalSeparator = "."
                        });
                    }
                }

                foreach (var val in SMI)
                {
                    item.SMI = double.Parse(val.Value, new NumberFormatInfo
                    {
                        NumberDecimalSeparator = "."
                    });
                }

                foreach (var attrib in element.Attributes())
                {
                    switch (attrib.Name.ToString())
                    {
                        case "name":
                            {
                                item.name = attrib.Value;
                            }
                            break;
                    }
                }
                result.Add(item);
            }
            Status = OperationStatus.STATUS_OK;
        }
    }
}
