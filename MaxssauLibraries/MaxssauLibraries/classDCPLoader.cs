using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace MaxssauLibraries
{
    public class classDCPLoader : ClassAddToLog
    {
        private string ModuleName = "DCP data loader";
        private OperationStatus LoadDCPFiles(ref string[] dcp_files)
        {
            try
            {
                if (!Directory.Exists("DCP"))
                {
                    Directory.CreateDirectory("DCP");
                }

                dcp_files = Directory.GetFiles("DCP", "*.dcp_xml");

                return OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                AddToLog(ex, ModuleName);
                return OperationStatus.STATUS_FAIL;
            }
        }

        public OperationStatus  status=new OperationStatus();

        public List<classDCPXMLReader>DCP_Data=new List<classDCPXMLReader>();
        public classDCPLoader(ClassLogger logger)
        {
            try
            {
                Logger=logger;
                string[] files = new string[1];
                if (LoadDCPFiles(ref files) == OperationStatus.STATUS_OK)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        classDCPXMLReader element = new classDCPXMLReader(files[i]);
                        DCP_Data.Add(element);
                    }
                    status = OperationStatus.STATUS_OK;
                }
                else
                {
                    status = OperationStatus.STATUS_FAIL;
                }
            }
            catch (Exception ex)
            {
                AddToLog(ex, ModuleName);
            }
        }
    }
}
