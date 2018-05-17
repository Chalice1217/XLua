using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using XLua;
/// <summary>
/// C#调用 Lua 里面的全局基本数据类型 
/// </summary>
public class CSharpCallLua : MonoBehaviour
{
    private LuaEnv luaEnv;
    string outPath;
    private Dictionary<string, byte[]> fileDic = new Dictionary<string, byte[]>();
    private byte[] Bytes;
    private void Start()
    {
        luaEnv = new LuaEnv();
        luaEnv.AddLoader(MyLoader);
        luaEnv.DoString("require 'CSharpCallLua'");

        double a = luaEnv.Global.Get<double>("a");
        print("lua 中 a 的值为 :" + a);
        string b = luaEnv.Global.Get<string>("b");
        print("Lua中 b 的值为 :" + b);
        bool c = luaEnv.Global.Get<bool>("c");
        print("lua中 c 的值为 :" + c);
 
    }

    private byte[] MyLoader( ref string fileName)
    {
        Bytes = null;
        outPath = Application.streamingAssetsPath + "/" + "LuaFile";
        DirectoryInfo directoryInfo = new DirectoryInfo(outPath);

        if (fileDic.ContainsKey(fileName))
        {
            return fileDic[fileName];
        }
        TraversingFileSystem(directoryInfo, fileName);
        return Bytes;
    }

    /// <summary>
    /// 返回值为 byte[] 时 ，递归会出错，无法返回byte[]
    /// 所以用 void 作为返回值 ， 定义了一个私有变量Bytes来接收
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="fileSystemInfo"></param>
    private void  TraversingFileSystem(FileSystemInfo fileSystemInfo,string fileName)
    {
        
        DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
        FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();

        foreach (FileSystemInfo item in fileSystemInfos)
        {
            FileInfo file = item as FileInfo;

            if (file == null)
            {
                TraversingFileSystem(item, fileName);
            }
            else
            {
                string tempName = item.Name.Split('.')[0];
                if (tempName != fileName || item.Extension == ".meta")
                    continue;
                //byte[] Bytes = Encoding.UTF8.GetBytes(File.ReadAllText(file.FullName));
                Bytes = File.ReadAllBytes(file.FullName);
                fileDic.Add(fileName, Bytes);                             
            }
        }
         
    }

    private void OnDestroy()
    {
        luaEnv.Dispose();
    }
 
}
