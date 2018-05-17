using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.IO;
using System.Text;
/// <summary>
/// lua文件在任意文件夹下都可以被加载
/// 一般Lua文件都会在 StreamingAssets 文件夹下
/// </summary>
public class AddluaFileByAnyFileSystem : MonoBehaviour
{
   private  string outPath;
   private  LuaEnv luaEnv;
   private  byte[] Bytes;

    private void Start()
    {
        luaEnv = new LuaEnv();
      
        luaEnv.AddLoader(MyLoader);

        luaEnv.DoString("require 'AnyFile'");
    }


    private byte[] MyLoader( ref string fileName)
    {
        Bytes = null;
        outPath = Application.streamingAssetsPath + "/" + "LuaFile";
        DirectoryInfo directoryInfo = new DirectoryInfo(outPath);

        TraversingFileSystemInfo(fileName, directoryInfo);
        return Bytes;
    }
 
    /// <summary>
    /// 返回值为 byte[] 时 ，递归会出错，无法返回byte[]
    /// 所以用 void 作为返回值 ， 定义了一个私有变量Bytes来接收
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="fileSystemInfo"></param>
    private void TraversingFileSystemInfo(string fileName,FileSystemInfo fileSystemInfo)
    {
        
       DirectoryInfo directoryInfo =  fileSystemInfo as DirectoryInfo;
       FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();

        foreach (FileSystemInfo item in fileSystemInfos)
        {
           FileInfo file = item as FileInfo;

            if (file == null)
            {
                TraversingFileSystemInfo(fileName, item);
            }
            else
            {               
                string tempName = item.Name.Split('.')[0];
                if (tempName != fileName || item.Extension == ".meta")
                    continue;   
                
                Bytes = File.ReadAllBytes(file.FullName);
                 
            }
        }
     
    }

    private void OnDestroy()
    {
        luaEnv.Dispose();
    }


}
