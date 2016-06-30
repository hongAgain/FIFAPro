
module("CommonScript", package.seeall)

function RegisterCallback(t,k,f)
	if t[k] ~= nil then
		table.insert(t[k],f);
	else
		t[k] = {};
		table.insert(t[k],f);
	end
end


function UnregisterCallback(t,k,f)
	if t[k] ~= nil then
		for i = 1, #(t[k]) do
			if t[k][i] == f then
				table.remove(t[k],i);
				break;
			end
		end
	end
end


local index = {}
local evetable = {};
local mt=
{
	__index = function (t,k)
		return t[index][k]
	end,
	__newindex = function (t,k,v)
		t[index][k] = v
		if(evetable[k] == nil) then
			evetable[k] = {}
		else
			local cbFucTable = evetable[k];
			for i = 1, #(cbFucTable) do
				local f = cbFucTable[i];
				if f ~= nil then
					f(k,v);
				end
			end
		end
	end
}


function Listener(t, evet)
local proxy = {}
proxy[index] = t
setmetatable(proxy, mt)
evetable = evet;
return proxy;
end


function PrintG()
	for k,v in pairs(_G) do
	   print(k)
	end
end


function NewCacheString()

	local strCache = {};

	local instance = {};

	local mt =
	{
		Add = function (s)
			table.insert(strCache, s)
		end,

		AllStr = function ()
			return table.concat(strCache, "")
		end,

		Del = function ()
			strCache = nil;
		end
	};

	mt.__index = mt;

	setmetatable(instance, mt);

	return instance;
end




function DeepCopy(obj)
    local tempTable = {};

    local function copy(obj)
        if type(obj) ~= "table" then
            return obj
        elseif tempTable[obj] then
            return tempTable[obj]
        end
        local newTable = {};
        tempTable[obj] = newTable;
        for index, value in pairs(obj) do
            newTable[copy(index)] = copy(value);
        end
        return setmetatable(newTable, getmetatable(obj))
    end

    return copy(obj);
end



function SetDefault(mt, key, value)
	mt.__index = function(table, key)
		print('try access none field:\''..key..'\', return default value: ');
		mt[key] = value;
		return mt[key];
	end
end

----申明一个类的快捷方式------
local _class={};
function Class(super)
	local classType={};

	classType.ctor=false;
	classType.super=super;
	classType.new=
		function(...) 
			local obj={}
			do
				local create
				create = function(c,...)
					if c.super then
						create(c.super,...);
					end
					if c.ctor then
						c.ctor(obj,...);
					end
				end
				create(classType,...);
			end
			setmetatable(obj,{ __index=_class[classType] });
			return obj
		end;

	local visualFuncTable={};
	_class[classType]=visualFuncTable;
 
	setmetatable(classType,{__newindex=
		function(t,k,v)
			visualFuncTable[k]=v
		end
	});
 
	if super then
		setmetatable(visualFuncTable,{__index=
			function(t,k)
				local ret=_class[super][k]
				visualFuncTable[k]=ret
				return ret
			end
		});
	end;
 	
	return classType;
end


function QuickSort(tb, compare)
    local temp = {};
    for _,v in pairs(tb) do
        table.insert(temp, v);
    end
    
    if (#temp > 0) then

        function Sort(left, right)
            if (left >= right) then
                return;
            end
            
            local basic = temp[left];
            local i = left;
            local j = right;
            
            while (i < j) do
                while (i < j) do
                    local res = compare(temp[j], basic);
                    if (res > 0) then
                        break;
                    end
                    j = j - 1;
                end
                
                while (i < j) do
                    local res = compare(basic, temp[i]);
                    if (res > 0) then
                        break;
                    end
                    i = i + 1;
                end
                
                if (i < j) then
                    local t = temp[i];
                    temp[i] = temp[j];
                    temp[j] = t;
                end
            end
            
            temp[left] = temp[i];
            temp[i] = basic;
            
            Sort(left, i - 1);
            Sort(i + 1, right);
        end
        
        Sort(1, #temp);
    end
    
    return temp;
end

function SaveGetData(tb, fieldName)
    if (tb == nil) then
        return nil;
    end

    local splitNames = fieldName:split(".");
    local value = tb;
    for _, v in ipairs(splitNames) do
        local temp = value[v];
        if (temp ~= nil) then
            value = temp;
        else
            print('Index Field "'..value..'" is null');
        end
    end
    return value;
end

function string:split(sep)
    local sep, fields = sep or "\t", {}
    local pattern = string.format("([^%s]+)", sep)
    self:gsub(pattern, function(c) fields[#fields+1] = c end)
    return fields
end

function GetIntPart(x)
    if x <= 0 then
       return math.ceil(x);
    end

    if math.ceil(x) == x then
       x = math.ceil(x);
    else
       x = math.ceil(x) - 1;
    end
    return x;
end

function TableContainValue(tb, value)
    for _, v in ipairs(tb) do
        if (v == value) then
            return true;
        end
    end
    
    return false;
end

function TableRemoveValue(tb, value)
    for k, v in ipairs(tb) do
        if (v == value) then
            table.remove(tb, k);
            break;
        end
    end
end

function ReplaceClone(str1)
    return string.gsub(str1, "%(Clone%)", "");
end

function CreatePerson(playerId, actionCollectionIdx, defaultAnim, onCreate, AnimationPrefabName, CreatePlayerType)
    if (CreatePlayerType ~= LuaConst.Const.PlayerTypeCoach) then
        local clubId  = "48004";--Config.GetProperty(Config.HeroTable(), playerId, "club");
        local shoesId = Config.GetProperty(Config.HeroTable(), playerId, "shoes");
        local bodyId  = Config.GetProperty(Config.HeroTable(), playerId, "modelId");
        local skin    = Config.GetProperty(Config.HeroTable(), playerId, "skin");
        
        local r = 1;
        local g = 1;
        local b = 1;
        --print(string.format("id:%s\t\t\t\tskin:%s", playerId, skin));
        if (nil ~= skin and skin ~= "") then
            local rgb = skin:split(".");
            r = tonumber(rgb[1]) / 255;
            g = tonumber(rgb[2]) / 255;
            b = tonumber(rgb[3]) / 255
        end
        local skinColor = Color.New(r, g, b, 1);
        Util.CreatePerson(bodyId, playerId, clubId, shoesId, skinColor, actionCollectionIdx, defaultAnim, onCreate, AnimationPrefabName);
    else
        Util.CreateCoach(playerId, onCreate, defaultAnim, AnimationPrefabName);
    end
    -- if (isLobbyPlayer == nil or isLobbyPlayer == false) then
    --     Util.CreatePerson(bodyId, playerId, clubId, shoesId, skinColor, actionCollectionIdx, defaultAnim, onCreate,AnimationPrefabName);
    -- else
    --     Util.CreateLobbyPlayer(bodyId, playerId, clubId, shoesId, skinColor, onCreate, LobbyAnimName,AnimationPrefabName);
    -- end
end

function SetButtonActive( btnTrans, active )
    local enable = TransformFindChild(btnTrans, "EnableLbl").gameObject
    local disable = TransformFindChild(btnTrans, "DisableLbl").gameObject
    enable:SetActive(active)
    disable:SetActive(not active)
    UIHelper.SetButtonActive(btnTrans, active, false)
end

--迭代器，以orderkey的值(数值或字符串)排序,isNum表示是否以数值形式比较
function PairsByOrderKey(t, orderKey, isNum)
    local a = {}
    for _,v in pairs(t) do
        if isNum then
            a[#a+1] = tonumber(v[orderKey])
        else
            a[#a+1] = tostring(v[orderKey])
        end
    end
    table.sort(a)
    local i = 0
    return function()
        i = i + 1
        for k,v in pairs(t) do
            local key = nil
            if isNum then
                key = tonumber(v[orderKey])
            else
                key = tostring(v[orderKey])
            end
            if key == a[i] then
                return k, v
            end
        end
        return nil,nil
    end
end

--通过progressbar滚动grid，div-总的调用次数（跟时间挂钩）
function UpdatePBValueByDivision(barTrans,fromValue,toValue,div,endFunc)
    local delta = math.abs(toValue - fromValue) / div
    local cur = UIHelper.GetProgressBar(barTrans)
    if math.abs(toValue - cur) <= delta then
        UIHelper.SetProgressBar(barTrans,toValue)
        endFunc()
    end
    UIHelper.SetProgressBar(barTrans,math.MoveTowards(cur,toValue,delta))
end

--当前时分秒转化成秒
function DayHMS2Second(hms)
    return hms[1]*3600 + hms[2]*60+hms[3]
end