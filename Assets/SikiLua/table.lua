
-- 一 . 表的基本用法

--1.1 表的定义

tab1 = {} --空表,构造表达式
tab2 = {key1 = "ddd",key2 = 34} --初始化一个表

print(tab1)
print(tab2.key2)
print(tab2["key2"])

tab3 = {"value1","value2","value3","value4"}
print(tab3[1])

for k,v in pairs(tab3) do

print(k .."--".. v)

end


--1.2 表的修改

--增
tab1.key1 = "table1"
tab1["key2"] = "1111"
print(tab1.key1)
print(tab1["key2"])

--删
tab1.key1 = nil
print(tab1.key1)

--改
tab1.key2 = "2222"
print(tab1.key2)

-- 表中元素不是连续的,删除中间一个,后面的下标不改变
tab3[2] = nil
for k,v in pairs(tab3) do

print(k.."--".. v)

end

--1--value1
--3--value3
--4--value4
