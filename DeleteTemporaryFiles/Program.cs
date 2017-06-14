using DeleteTemporaryFiles.Entities;
using DeleteTemporaryFiles.Entities.Enums;
using DeleteTemporaryFiles.Services;
using System;

namespace DeleteTemporaryFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            string TemporaryFilePath;
            int KeepForDays;
            if (args.Length == 2)
            {
                TemporaryFilePath = args[0];
                KeepForDays = Convert.ToInt32(args[1]);
            }
            else
            {
                TemporaryFilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Documents\Temporary Files TEST\";
                KeepForDays = 1;
                //Environment.Exit(0);
                // Open a form to notify the user that the parameters supplied are not valid
            }

            Logging log = new Logging(TemporaryFilePath, KeepForDays);
            Logging.LogSpacer("");
            Logging.LogSpacer();

            var entry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Status = LogStatus.Information,
                Success = true,
                Message = "Delete Files and Folders process started on folder \"" + TemporaryFilePath + "\""
            };
            Response respTest = Logging.Log(entry);
            Logging.LogSpacer("");

            Validation.ValidateArguments(TemporaryFilePath, KeepForDays);



            //Type t = Type.GetTypeFromProgID("Shell.Application");
            //dynamic shell = Activator.CreateInstance(t);

            //Folder RecycleBin = shell.NameSpace(ShellSpecialFolderConstants.ssfBITBUCKET);  //ssfBITBUCKET is the user's recycle bin
            //                                                                                //Ref: https://msdn.microsoft.com/en-us/library/windows/desktop/bb774096(v=vs.85).aspx
            //RecycleBin.MoveHere(@"C:\Users\dlockwood\Documents\Temporary Files\DeleteMe.txt ", new int[] { 4, 64, 512, 9182 });



            //Ref: https://msdn.microsoft.com/en-us/library/windows/desktop/bb787874(v=vs.85).aspx
            /*
            4:          Do not display a progress dialog box
            64:        Preserve undo information, if possible
            512:     Do not confirm the creation of a new directory if the operation requires one to be created
            9182:  Do not move connected files as a group. Only move the specified files
            */
            Logging.LogSpacer();
            Logging.LogSpacer("");
        }
    }
}
