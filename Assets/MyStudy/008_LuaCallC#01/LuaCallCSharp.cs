using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
/// <summary>
/// 这里没有注释,去LuaCallCSharp.lua.txt 里找找吧
/// </summary>
public class LuaCallCSharp : MonoBehaviour
{
    private LuaEnv luaEnv;
    string outPath;
    System.Action update;
    private void Start()
    {       
        luaEnv = new LuaEnv();
        luaEnv.AddLoader(MyLoader);
        luaEnv.DoString("require 'LuaCallCSharp'");

        update = luaEnv.Global.Get<System.Action>("update");
    }

    private byte[] MyLoader(ref string fileName)
    {
        byte[] Bytes = null ;
        outPath = Application.streamingAssetsPath + "/LuaFile";
        DirectoryInfo directoryInfo = new DirectoryInfo(outPath);
        TraverseFileSystemInfo.Instance.TraversingFileSystem(directoryInfo, fileName, ref Bytes);
        return Bytes;
        
    }

    private void Update()
    {
        if (update!=null)
        {
            update();
        }
    }

    private void OnDestroy()
    {
        update = null;
        luaEnv.Dispose();
    }

}
