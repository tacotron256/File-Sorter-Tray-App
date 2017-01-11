using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSorterTrayApp
{
    public class FileEntity
    {
        public FileInfo fileInfo;
        public FileRule associatedRule;
        public string UIC;

        public FileEntity() { }

        public FileEntity(FileInfo Info, FileRule AssociatedRule)
        {
            this.fileInfo = Info;
            this.associatedRule = AssociatedRule;
            //get UIC from file name using fileRule mask
            this.UIC = getUICfromFileName(associatedRule.fileNameMask, Info.Name);
        }

        private string getUICfromFileName(string mask, string fileName)
        {
            //ex mask: "yyyyMMdd-UUUUUU-TTTTTTTTTT"
            //ex data: "20160426-WZA213-USERHEALTH"

            //find UUUUUU in the mask and indexes of start and beginning
            int startIndex = mask.IndexOf("UUUUUU");

            string UIC = fileName.Substring(startIndex, 6);

            return UIC;
        }
    }
}
