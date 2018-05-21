using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
using System;
/// <summary>
/// 访问一个全局的function [2种方式]
/// </summary>

 // 1.2 有一个返回值 
 [CSharpCallLua]
public delegate int Sum(int a, int b);

 // 1.3 有多个返回值
 [CSharpCallLua]
public delegate int MultiReturnDele(int x, int y,out int z1 ,out int z2,out int z3);

public class CSharpCallLua_Function : MonoBehaviour
{
    // 1. 使用委托的方式,映射到delegate [推荐使用!!!]
    // 1.1 无参无返回值的类型 : 可以使用Action委托 , 不需要加 [CSharpCallLua] ,在 OnDestroy 时要置空,不然报错
    // 1.2 有参有一个返回值 : 使用delegate 委托 , 需要加 [CSharpCallLua] ,在 OnDestroy 时要置空,不然报错
    // 1.3 有参有多个返回值 : 使用ref ,out 关键字接收

    // 2. 映射到 LuaFunction
    // 耗费性能,少用较好 ;


    private LuaEnv luaEnv;
    private string outPath;
    
    private void Start()
    {
        luaEnv = new LuaEnv();      
        luaEnv.AddLoader(MyLoader);
        luaEnv.DoString("require 'CSharpCallLua_Function'");

        // 1.1 无参无返回值的函数
        NoArgNoReturn();

        // 1.2 一个返回值的函数
        OneReturn();

        // 1.3 返回多个返回值
        MultiReturn();

        // 2. 使用LuaFunction
        UseLuaFunction();

    }
    #region 第一种使用delegate的方式
    /// <summary>
    ///  1.1 无参无返回值(有参数就需要使用delegate,并且加上[CSsharpCallLua])
    ///  使用 Action , 不用加[CSharpCallLua]
    ///  在 OnDestroy 时要置空
    /// </summary>
    Action Say;
    private void NoArgNoReturn()
    {
         Say = luaEnv.Global.Get<Action>("Say");      
        if (Say != null)
        {
            Say();
        }
    }

    /// <summary>
    /// 1.2 有一个返回值(参数有没有都行)
    /// 使用delegate , 要加[CSharpCallLua]
    /// 在 OnDestroy 需要置空
    /// </summary>
    Sum sum;
    private void OneReturn()
    {
       sum = luaEnv.Global.Get<Sum>("Sum");
        if (sum!=null)
        {
            int resSum = sum(20,30);
            print("从lua函数'Sum'返回回来的结果为:" + resSum);
        }
    }

    /// <summary>
    /// 1.3 有多个返回值
    /// 使用delegate , 要加[CSharpCallLua]
    /// 在 OnDestroy  需要置空
    /// 使用ref 关键字也一样 , 不过abcd需要赋一个初值即可
    /// </summary>
    MultiReturnDele multiReturnDele;
    private void MultiReturn()
    {
        multiReturnDele = luaEnv.Global.Get<MultiReturnDele>("MultiReturn");
        if (multiReturnDele != null)
        {
            int a;
            int b;
            int c;
            int d;
            a =  multiReturnDele(100, 50,out b,out c,out d);
            print("x + y 的值为 :" + a);
            print("x - y 的值为 :" + b);
            print("x * y 的值为 :" + c);
            print("x / y 的值为 :" + d);

        }
    }

    #endregion

    #region 第二种使用LuaFunction的方法

    private void UseLuaFunction()
    {
        LuaFunction func = luaEnv.Global.Get<LuaFunction>("LuaFunc");
        object[] objs = func.Call(10,20);
        foreach (object item in objs)
        {
            print("使用LuaFunction返回结果为" + item);
        }
    }



    #endregion

    #region 获取lua文件
    private byte[] MyLoader(ref string fileName)
    {
        byte[] Bytes = null;
        outPath = Application.streamingAssetsPath + "/" + "LuaFile";
        DirectoryInfo directoryInfo = new DirectoryInfo(outPath);
        TraversingFileSystem(directoryInfo, fileName,ref Bytes);
       // TraverseFileSystemInfo.Instance.TraversingFileSystem(directoryInfo,fileName, ref Bytes);
        return Bytes;
    }

    private void TraversingFileSystem(FileSystemInfo fileSystemInfo, string fileName ,ref byte[] by )
    {

        DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
        FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
        foreach (FileSystemInfo item in fileSystemInfos)
        {
            FileInfo file = item as FileInfo;

            if (file == null)
            {
                TraversingFileSystem(item, fileName,ref by);
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

    #endregion

    private void OnDestroy()
    {
        Say = null;
        sum = null;
        multiReturnDele = null;
        luaEnv.Dispose();
       
    }
}
