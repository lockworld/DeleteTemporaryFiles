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
                // Notify the user that the parameters supplied are not valid
            }
            
            Logging log = new Logging(TemporaryFilePath, KeepForDays);
            
            bool valid = Validation.ValidateArguments(TemporaryFilePath, KeepForDays);
            if (valid)
            {
                Process.ProcessFolder();
            }
            else
            {
                var entry = new LogEntry
                {
                    Status = LogStatus.Error,
                    Success = false,
                    Message = "The application is exiting because the supplied parameters are not valid."
                };
                Logging.Log(entry);
                Environment.Exit(0);
            }


            Logging.LogSpacer();
            Logging.LogSpacer("");
        }
    }
}
