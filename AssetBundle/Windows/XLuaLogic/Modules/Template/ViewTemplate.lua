ViewTemplate = {};
local this = ViewTemplate;

local transform;
local gameObject;

-- awake
function ViewTemplate.awake(obj)
    gameObject = obj ;
    transform = obj.transform;

    this.InitView();
end


-- 获取UI组件
function ViewTemplate.InitView()


end

-- start
function ViewTemplate.start()

end


-- update
function ViewTemplate.update()

end


-- ondestroy
function ViewTemplate.ondestroy()


end