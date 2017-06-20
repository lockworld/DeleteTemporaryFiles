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
            try
            {
                TemporaryFilePath = args[0];
                KeepForDays = Convert.ToInt32(args[1]);
                     
            // Instantiate the main logging service (which also stores global variables for TemporaryFilePath and KeepForDays)
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
                    Message = "The application is exiting because the supplied parameters ([" + string.Join(",", args) + "]) are not valid."
                };
                Logging.Log(entry);
                Logging.CloseLog();
                Environment.Exit(0);
            }
            Logging.CloseLog();

            }
            catch (Exception ex)
            {
                // This should probably be replaced by a GUI element notifying the user of the error. But for now, any fatal errors in the application at least get logged
                Logging log = new Logging(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DeleteTemporaryFiles\", 0);

                var entry = new LogEntry
                {
                    Status = LogStatus.Error,
                    Success = false,
                    Message = "An unexpected error caused the application to terminate:\r\n     " + ex.ToString()
                };
                Logging.Log(entry);
                Logging.CloseLog();
                Environment.Exit(0);
            }
        }
    }
}
