using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
public class CustomMyLoader : MonoBehaviour
{
    LuaEnv luaEnv;

    private void Start()
    {
        luaEnv = new LuaEnv();

        luaEnv.AddLoader(MyCustomLoader);

        luaEnv.DoString("require'luaByFile'");

    }

    /// <summary>
    /// 自定义Loader方法
    /// </summary>
    /// <param name="filePath">filePath 就是require 的Lua文件名 </param>
    /// <returns></returns>
    private byte[] MyCustomLoader(ref string filePath)
    {
        print(filePath);  
    
       //方案一 : 自定义Loader里面有Lua文件的情况 :
      
         // s 是 合法的 lua 语言
        string s = "print('我是自定义Loader方法,在require时,我最先被查询!')";
         // 把 s 转换成 byte[]
        return System.Text.Encoding.UTF8.GetBytes(s);
    

        // 方案二 : 自定义Loader里面没有Lua文件的情况 :
        // return null;

    }

    private void OnDestroy()
    {
        luaEnv.Dispose();
    }



    // 总结 
    // luaEnv.DoString("require''") 在require lua文件时 , 总是先从自定义的loader开始查询 , 
    // 如果从自定义的loader里面找到的话,就用自定义loader里的lua文件
    // 如果从自定义的loader里面找不到的话,再从系统默认的Resources里面找

    //实例验证
    // 如果我require 一个不存在的 Lua文件 , 在 MyCustomLoader()里打开方案一 , 则会输出 filePath = "不存在的 Lua文件" ; "我是自定义Loader方法,在require时,我最先被查询!"
    // 如果我require luaByFile(这是一个存在的Lua文件) , 在 MyCustomLoader()里打开方案一 , 则会输出 filePath = "luaByFile" ; "我是自定义Loader方法,在require时,我最先被查询!"
    // 如果我require 一个不存在的 Lua文件 , 在 MyCustomLoader()里打开方案二, 则会报错
    // 如果我require luaByFile(这是一个存在的Lua文件) , 在 MyCustomLoader()里打开方案二 , 则会输出 luaByFile 里的lua 代码 ;

    // 以上四个例子证明 :
    // 加载Lua文件时 :  CustomLoader 优先级 > 系统默认 Loader 

}
