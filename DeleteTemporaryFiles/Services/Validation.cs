using DeleteTemporaryFiles.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeleteTemporaryFiles.Services
{
    public class Validation
    {
        public static void ValidateArguments(string temporaryFilePath, int keepForDays)
        {
            ValidateKeepForDays(keepForDays);            
        }

        private static void ValidateKeepForDays(int keepForDays)
        {
            LogEntry logEntry = new LogEntry();
            if (keepForDays < 1)
            {
                logEntry.Success = false;
                logEntry.Status = Entities.Enums.LogStatus.Error;
                logEntry.Message = "The \"Keep For Days\" parameter supplied (" + keepForDays.ToString() + ") is not valid. Please enter an integer greater than 1 and re-run the application.\r\n";
                Logging.Log(logEntry);
            }
        }
        
    }
}
