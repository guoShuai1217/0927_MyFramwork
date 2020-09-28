RegistTestEntity = {Id = 0,Title = "",Status = 0,Content = ""};

-- 重定义元表的索引,有了这句话,才可以作为一个类
RegistTestEntity.__index = RegistTestEntity;

function RegistTestEntity.New(id,title,Status,content )

	local self = {}; --初始化self
	setmetatable(self,RegistTestEntity); --将self的元表设定为Class

	self.Id = id ;
	self.Title = title;
	self.Status = Status;
	self.Content = content;

	return self ;		 
end
