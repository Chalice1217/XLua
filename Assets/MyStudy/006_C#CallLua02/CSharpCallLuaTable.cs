using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
/// <summary>
/// C# 访问 Lua 一个全局table 的4种方法；
/// </summary>
public class CSharpCallLuaTable : MonoBehaviour
{
    // 1、映射到普通class或struct : 定义一个class，有对应于table的字段的public属性，而且有无参数构造函数即可



    LuaEnv luaEnv;
    byte[] Bytes;
    string outPath;
    private void Start()
    {
        luaEnv = new LuaEnv();
        luaEnv.AddLoader(MyLoader);
        luaEnv.DoString("require 'CSharpCallLuaTable'");

        // 方法一 :
        //  Person person =  luaEnv.Global.Get<Person>("Person");
        //  print(person.name + "--" + person.age + "--");

    }

    private byte[] MyLoader(ref string fileName)
    {
        Bytes = null;
        outPath = Application.streamingAssetsPath + "/" + "LuaFile";
        DirectoryInfo directoryInfo = new DirectoryInfo(outPath);      
        TraversingFileSystem(directoryInfo, fileName);
        return Bytes;
    }

    private void TraversingFileSystem(FileSystemInfo fileSystemInfo, string fileName)
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
                
            }
        }

    }

    private void OnDestroy()
    {
        luaEnv.Dispose();
    }

    //public class Person
    //{
    //    // 方法一 : 定义一个和lua脚本里相同的类【类名，字段名无所谓，但最好相同，必须为public】
    //    public string name;
    //    public int age;

    //}
}
