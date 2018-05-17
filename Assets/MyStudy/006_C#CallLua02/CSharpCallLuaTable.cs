using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
/// <summary>
/// C# 访问 Lua 一个全局table 的4种方法；
/// 修改lua文件,请务必在Lua编辑器或者Notepad++ 里修改 ; 在文本编辑器里修改,保存之后,文件编码格式又变成UTF-8-BOM 格式了 , 运行又会报错 !!! 
/// </summary>

public class Person
{
    // 方法一 : 定义一个和lua脚本里相同的类【类名，字段名无所谓，但最好相同，修饰符必须为public】
    public string name;
    public int age;
    public string str3;
}

[CSharpCallLua]
public interface IPerson
{
    // 方法二 : 映射到一个Interface , 字段需要设置get,set
    string name { get; set; }
    int age { get; set; }
    void Eat();
    void Sum(int a,int b);
    void Sub(int c, int d);
}

public class CSharpCallLuaTable : MonoBehaviour
{
    // 1、映射到普通class或struct : 定义一个class，有对应于table的字段的public属性，而且有无参数构造函数即可
    // 缺点 : 这个过程是值拷贝，如果class比较复杂代价会比较大。
    // 而且修改class的字段值不会同步到table，反过来也不会。

    // 2. 映射到一个interface : 
    // 需要用 [CSharpCallLua]
    // 修改class的字段值会同步到table，反过来会。

    LuaEnv luaEnv;
    byte[] Bytes;
    string outPath;
    private void Start()
    {
        luaEnv = new LuaEnv();
        luaEnv.AddLoader(MyLoader);
        luaEnv.DoString("require 'CSharpCallLuaTable'");

        // 方法一 :映射到类
        Person person =  luaEnv.Global.Get<Person>("Person");
        print("姓名:" + person.name + "," + "年龄:" + person.age + person.str3 );
        person.name = "钻石老王";
        luaEnv.DoString("print('在修改class后Person.name ='..Person.name)"); // person.name 还是 guoShuai , 修改class不会同步到table,反之亦然

        // 方法二 :映射到接口
        IPerson ip = luaEnv.Global.Get<IPerson>("Person");
        print(ip.name + "--" + ip.age);
        ip.name = "钻石老王";
        luaEnv.DoString("print('在修改interface后Person.name ='..Person.name)"); // person.name 变为 "钻石老王" , 修改class会同步到table,反之亦然
        ip.Eat();
        ip.Sum(10, 30); // ip.Sum(ip,10,30)
        ip.Sub(30, 10);
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

   
}
