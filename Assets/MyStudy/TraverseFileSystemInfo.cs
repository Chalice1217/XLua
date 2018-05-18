using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TraverseFileSystemInfo
{
    private static TraverseFileSystemInfo instance;
    public static TraverseFileSystemInfo Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TraverseFileSystemInfo();
            }
            return instance;
        }
    }
 
    

    public void TraversingFileSystem(FileSystemInfo fileSystemInfo, string fileName, ref byte[] by)
    {

        DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
        FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
        foreach (FileSystemInfo item in fileSystemInfos)
        {
            FileInfo file = item as FileInfo;

            if (file == null)
            {
                TraversingFileSystem(item, fileName, ref by);
            }
            else
            {
                string tempName = item.Name.Split('.')[0];
                if (tempName != fileName || item.Extension == ".meta")
                    continue;
                //byte[] Bytes = Encoding.UTF8.GetBytes(File.ReadAllText(file.FullName));
                by = File.ReadAllBytes(file.FullName);

            }
        }

    }

}
