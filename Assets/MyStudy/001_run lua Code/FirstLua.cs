using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
public class FirstLua : MonoBehaviour
{
    private LuaEnv luaEnv;


    private void Start()
    {
        luaEnv = new LuaEnv();

        luaEnv.DoString("print('这是C#调Lua里的print()输出的')");
        luaEnv.DoString("CS.UnityEngine.Debug.Log('这是在lua里调用C#的Debug.Log()输出的')");

    }


    private void OnDestroy()
    {
        luaEnv.Dispose();      
    }

}
