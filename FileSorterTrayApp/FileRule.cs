using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSorterTrayApp
{
    public class FileRule
    {
        public string ruleName;
        public string fileNameMask;
        public string originPath;
        public string destinationPath;
        public bool deleteAfterCopy;
        public string fileType; //server health, user status, etc
        public string fileIdentifier; //substring of file name, used to identify file type

        public FileRule(string RuleName, string FileNameMask, string OriginPath, string DestinationPath, bool DeleteAfterCopy, string FileType, string FileIdentifier)
        {
            this.ruleName = RuleName;
            this.fileNameMask = FileNameMask;
            this.originPath = OriginPath;
            this.destinationPath = DestinationPath;
            this.deleteAfterCopy = DeleteAfterCopy;
            this.fileType = FileType;
            this.fileIdentifier = FileIdentifier;
        }
    }
}
