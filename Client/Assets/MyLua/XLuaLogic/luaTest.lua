
print('欢迎来到lua世界')

-- 测试function number
function TestNumber(x)
    return x *10
end
local result = TestNumber(2.5)
print("计算结果为: " .. result)

-----------------------------------------------

-- 测试function string
function TestString(str)

    --return "你输入了:" .. str    -- 使用..连接字符串
    --return string.len(str)      -- 获取字符串长度(汉字占3个字符)
    --return #str                 -- 获取字符串长度(汉字占3个字符)
    --return string.lower(str)    -- 返回字母小写形式
    --return string.upper(str)    -- 返回字母大写形式
    --return string.find(str,"b")   -- 返回从str里找到的第一个b,下标从1开始。result==2
    --return string.format("%s,好好学习了,不玩游戏了,加油 %s",str,"guoShuai")   --%s相当于C#里的{}
    --return string.rep(str,5)    --str重复5次
end

--print("result == " .. TestString("老宋"));


-----------------------------------------------

-- if语句
function TestIf(x)

    if(x > 5) then
        print("输入数字大于5")
    else
        print("输入数字小于5")
    end
end
--print(TestIf(6))


-----------------------------------------------

-- 循环
function  TestLoop(num)
   for i = 1, num do        --i=1,i <= 10 , i++ , 注意这里可以取到10
       print("i==" .. i );
   end
end

TestLoop(10)


-----------------------------------------------


-- 数组
function TestArr()

    local arr = {"老王","今天九点","峡谷双排"}
    for k,v in pairs(arr)  do
        print(k .. v);
    end
end 
TestArr();


-----------------------------------------------


-- table
function TestTable()

    local tmpTable = {}
    tmpTable[#tmpTable + 1] = "说好的"
    tmpTable[#tmpTable+1] = "今天九点"
    tmpTable[#tmpTable+1] =  "峡谷双排呢?"
    tmpTable[#tmpTable+1] =  "淦"

    print(table.concat(tmpTable,"|||")) 
    table.remove(tmpTable,4);
    -- for key, value in pairs(tmpTable) do
    --     print(value)
    -- end

    tmpTable.Name = "老王"
    tmpTable.Age = 23
    tmpTable.Id = 1
    
end
TestTable();

