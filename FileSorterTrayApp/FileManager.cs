using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSorterTrayApp
{
    public class FileManager
    {

        List<FileRule> rules;
        string eventLogAppName = "File Sorter Service";

        public void start()
        {
            //get rules
            getRules();
            
            //kick off threads for each rule to watch multiple directories
            foreach(FileRule rule in rules)
            {
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = rule.originPath;
                watcher.Filter = "*.*";
                watcher.Created += new FileSystemEventHandler(TestFileCreated);
                watcher.EnableRaisingEvents = true;
            }

            while (true)
            { }
        }

        private void TestFileCreated(object sender, FileSystemEventArgs e)
        {

            //get file info
            //see if existing rule applies to file
            FileEntity file = findFileRule(e.FullPath);
            //apply rule actions to file
            applyRuleToFile(file);

        }

        private void applyRuleToFile(FileEntity file)
        {
            try
            {
                string destinationPath = getPathFromFileCreationTime(file);

                if (!Directory.Exists(destinationPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(destinationPath);

                }

                if (!File.Exists(destinationPath + file.fileInfo.Name))
                {
                    File.Move(file.fileInfo.FullName, destinationPath + file.fileInfo.Name);
                }
            }
            catch (IOException ex)
            {
                EventLog.WriteEntry(eventLogAppName, ex.InnerException.ToString());
            }
        }

        private string getPathFromFileCreationTime(FileEntity file)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(file.associatedRule.destinationPath);
            builder.Append(@"\" + file.fileInfo.CreationTime.Year + @"\");
            builder.Append(file.fileInfo.CreationTime.Month + @"\");
            builder.Append(file.fileInfo.CreationTime.Day + @"\");

            return builder.ToString();
        }

        private void getRules()
        {
            rules = new List<FileRule>();
            rules.Add(new FileRule("USERHEALTH", "yyyyMMdd-UUUUUU-TTTTTTTTTT", @"C:\Users\BOB\Documents\Visual Studio 2015\Projects\FileSorter\FileSorter\TestFiles", @"C:\Users\BOB\Documents\Visual Studio 2015\Projects\FileSorter\FileSorter\SortedFiles\USERHEALTH", true, "USERHEALTH", "USERHEALTH"));
        }

        private FileEntity findFileRule(string filePath)
        {
            FileInfo fileInfo;
            try
            {
                fileInfo = new FileInfo(filePath);
            }
            catch (Exception ex)
            {
                //todo: flesh out exception
                EventLog.WriteEntry(eventLogAppName, ex.InnerException.ToString());
                throw ex;
            }

            FileEntity returnFile = new FileEntity();
            foreach (FileRule rule in rules)
            {
                if (fileInfo.Name.Contains(rule.fileIdentifier))
                {
                    //file name contains identifier in rule
                    //Apply rule to file
                    returnFile = new FileEntity(fileInfo, rule);
                }
            }
            return returnFile;
        }
    }
}
