using DeleteTemporaryFiles.Entities.Enums;
using System;

namespace DeleteTemporaryFiles.Entities
{
    public class LogEntry
    {
        public LogEntry()
        {
            Success = true;
            Timestamp = DateTime.Now;
        }
        public bool Success { get; set; }
        public LogStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
    }
}
