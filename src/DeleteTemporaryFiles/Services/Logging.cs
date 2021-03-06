﻿using DeleteTemporaryFiles.Entities;
using DeleteTemporaryFiles.Entities.Enums;
using System;
using System.IO;

namespace DeleteTemporaryFiles.Services
{
    public class Logging
    {
        public static string LogFileName = DateTime.Now.ToString("yyyyMMdd") + "-DeleteFilesAndFolders.log";
        public static string DataSource = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DeleteFilesAndFoldersData.json";
        public static string LogFilePath;
        public static string LogFile;
        public static string TemporaryFilePath;
        public static int KeepForDays;

        public Logging(string temporaryFilePath, int keepForDays)
        {
            TemporaryFilePath = temporaryFilePath.TrimEnd('\\') + @"\";
            KeepForDays = keepForDays;
            LogFilePath = TemporaryFilePath + @"Logs\";
            LogFile = LogFilePath + LogFileName;

            if (!Directory.Exists(TemporaryFilePath))
            {
                Directory.CreateDirectory(TemporaryFilePath);
            }
            if (!Directory.Exists(LogFilePath))
            {
                Directory.CreateDirectory(LogFilePath);
            }

            Logging.LogSpacer("");
            Logging.LogSpacer();

            var entry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Status = LogStatus.Information,
                Success = true,
                Message = "Delete Files and Folders process started on folder \"" + TemporaryFilePath + "\""
            };
            Logging.Log(entry);
            Logging.LogSpacer("");
        }

        public static void Log(LogEntry entry)
        {
            string LogEntry = string.Format("{0,-25}  {1,-15}  {2}", entry.Timestamp.ToString("yyyy'-'MM'-'dd' 'hh':'mm':'ss.fff"), entry.Status.ToString().ToUpper(), entry.Message);
            var Response = new Response
            {
                Success = true,
                Message = ""
            };

            using (StreamWriter sw = File.AppendText(LogFile))
            {
                sw.WriteLine(LogEntry);
            }
        }
        public static void LogSpacer(string spacer = "-----")
        {
            using (StreamWriter sw = File.AppendText(LogFile))
            {
                sw.WriteLine(spacer);
            }
        }

        public static void CloseLog()
        {
            LogSpacer();
            LogSpacer("");
        }

    }
}
