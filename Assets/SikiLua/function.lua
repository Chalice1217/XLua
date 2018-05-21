-- 二 .函数

--2.1 阶乘

function fun1(num)

 if	num == 1 then
   return num
  else
    return num * fun1(num-1)
  end

end

print(fun1(3))

--2.2 赋值
fun2 = fun1

print(fun2(3))


--2.3 传递(委托和事件)
function TestFun(tab,fun3)
 for k,v in pairs(tab) do
 fun3(k,v)
 end
end

tab3 = {key1="guoShuai",key2 = "handsome"}

function PrintFun(k,v)
 print(k..":"..v)
end

TestFun(tab3,PrintFun)

--2.4 匿名函数

 TestFun(tab3,
 function(k,v)
   print(k.."****"..v)
 end
 )



local function dg(n,m,fun)
 if n >= m then
 fun(n)
   return n
   else
   fun(m)
   return m
   end

   end

local myPrint = function(prama)
 print(prama)
 end

dg(10,20,myPrint)

