--创建Class
 function class(classname,super)
	local superType = type(super)
	local cls
	if super then
		cls = {}
		setmetatable(cls, {__index = super})
		cls.super = super
	else
		cls = {ctor = function() end}
	end

	cls.__cname = classname
	cls.__index = cls

	function cls.new(...)
		local instance = setmetatable({}, cls)
		instance.class = cls
		instance:ctor(...)
		return instance
	end
	return cls	
end


function CreateEnumTable(tbl,index)
    --assert(IsTable(tbl))
    local enumtbl = {}
    local enumindex=index or 0
    for i,v in ipairs(tbl) do
        enumtbl[v] = enumindex + i
    end
    return enumtbl
end

-- 各种打印函数

function RedPrint(kMsg)
    LogManager.Instance:RedLog(tostring(kMsg))
end

function GreenPrint(kMsg)
    LogManager.Instance:GreenLog(tostring(kMsg))
end

function BluePrint(kMsg)
	print("<Color=blue>"..tostring(kMsg).."</Color>")
end

function YellowPrint(kMsg)
    LogManager.Instance:YellowLog(tostring(kMsg))
end

-- Button Time Interval
local lastClickTime = os.time();
function IsValidClick(interval_)
    if os.time() - lastClickTime > (interval_ or 1) then
        lastClickTime = os.time();
        return true;
    else
        return false;
    end
end

function ConvertNumber(num)
    num = tonumber(num) or 0
    if num >= 100000000 then
        return string.sub(num, 1, string.len(num)-8)..Util.LocalizeString("Yi")
    elseif num >= 100000 then
        return string.sub(num, 1, string.len(num)-4)..Util.LocalizeString("Wan")
    else
        return num
    end
end