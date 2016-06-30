module("ItemSys", package.seeall)

require("Common/CommonScript")
--require("Game/DataSystemScript")

local itemDataTable = {};

--local itemDefaultValue = {};
--CommonScript.SetDefault(itemDefaultValue, 'id', 'unknown');
--CommonScript.SetDefault(itemDefaultValue, 'key', 'unknown')

local defaultItem = { id = 0, num = 0 };

function OnInit()
	DataSystemScript.RegisterMsgHandler(MsgID.tb.SellItem, OnSellItem);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.UseItem, OnUseItem);
end

function GetItemData(key)
	if (itemDataTable[key] == nil) then
		-- LogManager:RedLog('Try access illegal itemdata key = '..key);
        return defaultItem;
	end	
	return itemDataTable[key];
end

function UpdateItemData(key, value)
    key = tostring(key);
    if itemDataTable[key] == nil then
	    itemDataTable[key] = {id = key, num = value};
    else
        itemDataTable[key].num = value
    end

end

function FilterItemByTab(itemType_)
	local tempItemList = {};

	for k,v in pairs(itemDataTable) do
		 if (GetItemType(k) == itemType_) then
		 	tempItemList[k] = v.num;
		 end
	end

    return tempItemList;
end

function AllItem()
	return itemDataTable;
end

function UseItem(id, num,tar)
	local dic = {};
	dic['id'] = id;
	dic['num'] = num or 1;
    dic['tar'] = tar or -1;

	DataSystemScript.RequestWithParams(LuaConst.Const.UseItem, dic, MsgID.tb.UseItem);
end

function HeroEatExp(item, hero)

end

function SellItem(item, num)
    if (item == nil) then
        return;
    end

	local dic = {};
	dic['id'] = item.id or 0;
	dic['num'] = num or 1;
    

	DataSystemScript.RequestWithParams(LuaConst.Const.SellItem, dic, MsgID.tb.SellItem);
end

function OnItemData(data)
    if (data ~= nil) then
		for k,v in pairs(data) do
            local item = {id = k, num = v};
			setmetatable(item, itemDefaultValue);
			itemDataTable[item.id] = item;
		end
	end
end

function OnSellItem(code, data)
	local sub = data['sub'];
	local add = data['add'];
end

function OnUseItem(code, data)
    
end

function CanUse(itemKey)
	return Config.GetProperty(Config.ItemTable(), itemKey, 'isUse') ~= 0;
end

function CanSell(itemKey)    
	return Config.GetProperty(Config.ItemTable(), itemKey, 'price') ~= 0;
end

function GetItemType(itemId_)
    return Config.GetProperty(Config.ItemTable(), itemId_, 'type');
end

function GetItemName(itemId_)
	return Config.GetProperty(Config.ItemTable(), itemId_, 'name');
end

function GetItemIconName(itemId_)
	return Config.GetProperty(Config.ItemTable(), itemId_, 'icon');
end

function GetItemDescription(itemId_)
	return Config.GetProperty(Config.ItemTable(), itemId_, 'desc');
end

function ItemEffect(itemId)
    local useType = Config.GetProperty(Config.ItemTable(), itemId, "useType");
    if (useType == 1) then
        local args = Config.GetProperty(Config.itemGroupTB, itemId, "args");
        local name = Config.GetProperty(Config.itemGroupInfoTB, args[1], "name");
        return name.."+"..tostring(args[2]);
    end
    
    return "";
end
