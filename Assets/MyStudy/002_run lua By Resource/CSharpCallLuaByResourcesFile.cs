using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
/// <summary>
/// C#里加载lua文件 
/// 放到Resources文件夹下 (不推荐使用,仅供参考)
/// </summary>
public class CSharpCallLuaByResourcesFile : MonoBehaviour
{

    private  LuaEnv luaEnv;

    private void Start()
    {
        luaEnv = new LuaEnv();

    /*
        // 方法一 : Resources.Load<TextAsset>() 通过加载文本的方式
        // luaByFile.lua.txt 文件 , 不用加 .txt 后缀 
        TextAsset ta = Resources.Load<TextAsset>("luaByFile.lua");    
        luaEnv.DoString(ta.ToString());
    */

        // 方法二 : require '' 方式
        // luaByFile.lua.txt 文件 , 不用加后缀
        luaEnv.DoString("require'luaByFile'");

    }

    private void OnDestroy()
    {
        luaEnv.Dispose();
    }

    // 总结 :
    // 两种方式都有一个前提条件 : lua文件必须放在Resources文件夹下面,而且必须后缀是 .txt
    // 不推荐这种方式 , 因为一般Lua文件都是从服务器下载下来的,不是直接放在工程的Resources文件夹下的
    // 推荐使用自定义 Loader 加载lua文件;

}
