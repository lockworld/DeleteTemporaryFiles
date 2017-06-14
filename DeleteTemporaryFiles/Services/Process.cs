using DeleteTemporaryFiles.Entities;
using DeleteTemporaryFiles.Entities.Enums;
using Microsoft.VisualBasic;
using Shell32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using static DeleteTemporaryFiles.Entities.ShellUtilities;

namespace DeleteTemporaryFiles.Services
{
    public class Process
    {
        private static DirectoryInfo temporaryFolder = new DirectoryInfo(Logging.TemporaryFilePath);
        private static DirectoryInfo recycleBin = new DirectoryInfo(ShellSpecialFolderConstants.ssfBITBUCKET.ToString());
        private static DateTime referenceDate = DateTime.Now.AddDays(0 - Logging.KeepForDays);

        private static Type t = Type.GetTypeFromProgID("Shell.Application");
        private static dynamic shell = Activator.CreateInstance(t);

        public static void ProcessFolder()
        {            
            ProcessFolder(temporaryFolder);            
        }

        public static void ProcessFolder(DirectoryInfo di)
        {
            LogEntry directoryLog = new LogEntry
            {
                Status = LogStatus.Information,
                Message = "Checking \"" + di.FullName + "\" for files modified before " + referenceDate.ToString("MM/dd/yyyy hh:mm:ss") + "."
            };
            Logging.Log(directoryLog);

            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;

            try
            {
                files = di.GetFiles("*.*");
            }
            catch (Exception e)
            {
                LogEntry entry = new LogEntry
                {
                    Status = LogStatus.Error,
                    Message = e.Message.ToString()
                };
                Logging.Log(entry);
            }

            //Requires a reference to Microsoft Shell Controls and Automation COM Type Library
            Folder RecycleBin = shell.NameSpace(ShellSpecialFolderConstants.ssfBITBUCKET);            
                                                 //ssfBITBUCKET is the user's recycle bin
                                                 //Ref: https://msdn.microsoft.com/en-us/library/windows/desktop/bb774096(v=vs.85).aspx

            // Process directories first (This prevents encountering a "Directory Not Found" error by scanning a directory AFTER it is deleted...
            try
            {
                subDirs = di.GetDirectories();
            }
            catch (UnauthorizedAccessException uae)
            {
                LogEntry entry = new LogEntry
                {
                    Status = LogStatus.Error,
                    Message = "An Unauthorized Access Exception occurred while scanning the directory \"" + di.FullName + ":\"\r\n     " + uae.Message.ToString()
                };
                Logging.Log(entry);
            }
            catch (DirectoryNotFoundException dne)
            {

                LogEntry entry = new LogEntry
                {
                    Status = LogStatus.Error,
                    Message = "A Directory Not Found Exception occurred while scanning the directory \"" + di.FullName + ":\"\r\n     " + dne.Message.ToString()
                };
                Logging.Log(entry);
            }

            if (subDirs != null)
            {
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    ProcessFolder(dirInfo);
                }
            }


            // Process files next
            if (files.Count()>0)
            {
                foreach (FileInfo fi in files)
                {
                    
                    if (fi.LastWriteTime < referenceDate)
                    {
                        try
                        {
                            // TODO: Find a way to prevent progress dialog from appearing on screen. The vOptions: 0x4 flag is not working.
                            RecycleBin.MoveHere(fi.FullName, vOptions: 0x4);
                                                                    //Ref: https://msdn.microsoft.com/en-us/library/windows/desktop/bb787874(v=vs.85).aspx
                                                                    /*
                                                                    4:          0x4         Do not display a progress dialog box
                                                                    64:       0x40       Preserve undo information, if possible
                                                                    512:     0x200    Do not confirm the creation of a new directory if the operation requires one to be created
                                                                    9182:  0x23DE  Do not move connected files as a group. Only move the specified files
                                                                    */
                            
                            LogEntry entry = new LogEntry
                            {
                                Status = LogStatus.Information,
                                Message = "File \"" + fi.FullName + "\" was moved to the Recycle Bin."
                            };
                            Logging.Log(entry);
                        }
                        catch (Exception ex)
                        {
                            LogEntry entry = new LogEntry
                            {
                                Status = LogStatus.Error,
                                Message = "Error moving \"" + fi.FullName + "\" to \"" + recycleBin.FullName + fi.Name + "\"5\r\n     " + ex.Message.ToString()
                            };
                            Logging.Log(entry);
                        }
                    }
                    
                }                
            }
            else
            {
                if (di.FullName != temporaryFolder.FullName)
                {
                    // Remove empty folders
                    try
                    {
                        RecycleBin.MoveHere(di.FullName, vOptions: 0x4);
                        LogEntry entry = new LogEntry
                        {
                            Status = LogStatus.Warning,
                            Message = "Directory \"" + di.FullName + "\" was moved to the Recycle Bin."
                        };
                        Logging.Log(entry);
                    }
                    catch (Exception ex)
                    {
                        LogEntry entry = new LogEntry
                        {
                            Status = LogStatus.Error,
                            Message = "Error deleting directory \"" + di.FullName + ":\"\r\n     " + ex.Message.ToString()
                        };
                        Logging.Log(entry);
                    }
                }
            }

        }
    }
}
