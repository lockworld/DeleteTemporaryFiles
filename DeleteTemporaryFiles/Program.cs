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

            // Read args into local variables
            if (args.Length == 2)
            {
                TemporaryFilePath = args[0];
                KeepForDays = Convert.ToInt32(args[1]);
            }
            else
            {
                // This code is in place for development and testing ONLY. It should be removed in a working environment.
                TemporaryFilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Documents\Temporary Files TEST\";
                KeepForDays = 1;

                // This is what should happen in a working environment:
                //Environment.Exit(0);
                // Notify the user that the parameters supplied are not valid
            }
            
            // Instantiate the main logging service (which also holds global variables for TemporaryFilePath and KeepForDays)
            Logging log = new Logging(TemporaryFilePath, KeepForDays);
            
            // Validate user-supplied command-line arguments before running the application
            bool valid = Validation.ValidateArguments(TemporaryFilePath, KeepForDays);
            if (valid)
            {
                Process.ProcessFolder();
            }
            else
            {
                // If the arguments are invalid log the error and exit the application
                var entry = new LogEntry
                {
                    Status = LogStatus.Error,
                    Success = false,
                    Message = "The application is exiting because the supplied parameters are not valid."
                };
                Logging.Log(entry);
                Logging.CloseLog();
                Environment.Exit(0);
            }

            Logging.CloseLog();
        }
    }
}
