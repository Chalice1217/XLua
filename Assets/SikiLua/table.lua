
-- һ . ��Ļ����÷�

--1.1 ��Ķ���

tab1 = {} --�ձ�,������ʽ
tab2 = {key1 = "ddd",key2 = 34} --��ʼ��һ����

print(tab1)
print(tab2.key2)
print(tab2["key2"])

tab3 = {"value1","value2","value3","value4"}
print(tab3[1])

for k,v in pairs(tab3) do

print(k .."--".. v)

end


--1.2 ����޸�

--��
tab1.key1 = "table1"
tab1["key2"] = "1111"
print(tab1.key1)
print(tab1["key2"])

--ɾ
tab1.key1 = nil
print(tab1.key1)

--��
tab1.key2 = "2222"
print(tab1.key2)

-- ����Ԫ�ز���������,ɾ���м�һ��,������±겻�ı�
tab3[2] = nil
for k,v in pairs(tab3) do

print(k.."--".. v)

end

--1--value1
--3--value3
--4--value4
