# DeleteTemporaryFilesAndFolders
This Windows application can be run as a scheduled task to purge files from a designated temporary location that are older than the specified number of days.


## To Use:
Download and build the application to generate the executable file. Once this is done, run the executable from the command line with two parameters:
1. The full path to the Temporary Folder you wish to scan and clean
2. The number of days you'd like to keep files in this folder (The application will delete ANYTHING more than this number of days old)

Download link: <https://github.com/lockworld/DeleteTemporaryFiles/raw/master/DeleteTemporaryFiles.exe>

### Example:
C:\> "DeleteTemporaryFiles.exe" "C:\Users\Lockworld\Documents\Temporary Files\" 14
> This will delete everything in the folder "C:\Users\Lockworld\Documents\Temporary Files\" that has not been modified in more than 14 days.


## Recommendation:
Create a scheduled task to run this application with the appropriate parameters on a regular basis.
> TIP: Create one scheduled task, and create multiple actions within the task to run this application against multiple folders, such as:
> * Your Downloads folder
> * The temporary folders you create to store downloaded files and resources
> * Any Log folders that are not cleaned up automatically by the application that creates them
> * Folders you've configured to automatically save all of your screenshots


# **_WARNINGS_**
1. _Use at your own risk._ The application does not prevent you from deleting files needed by your operating system. Improper use of this application can render your computer inoperable.
2. This application moves files and folders from their original location into the Recycle Bin so that they can be restored by the user. However, the recycle bin may be emptied at any time, and some files are too large to fit in the recycle bin. Use at your own risk. There is no guarantee that you can restore your files once they have been deleted.
3. This application is posted here as-is, with no warranty of any kind. The repository has been created to allow users to view the codes provided. Users who simply download and run the application without understanding what it is doing and how it works do so at their own risk.
