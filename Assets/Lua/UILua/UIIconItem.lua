module("UIIconItem", package.seeall)

require "Common/UnityCommonScript"
require "UILua/UIItemIcon"

local iconItemName = "UIItemIcon"
local iconItemPref = nil
--local countName = "Num"
--local iconName = "Icon"

function Init()
	if iconItemPref == nil then
		iconItemPref = Util.GetGameObject(iconItemName)
	end
end

--param={scale,disableColid,offsetDepth,color}
function CreateRewardIconItem( grid, scrollview, pair, param )
	local id = pair[1]
	local num = tonumber(pair[2]) or 0
	local iconItem = {}
	if id~=nil and id~="" then
		iconItem.trans = InstantiatePrefab(iconItemPref,grid,id).transform
		local curScale = param.scale or 1
		--iconItem.trans.localScale = NewVector3(curScale, curScale, curScale)
		local depth = param.offsetDepth or 0
		UIHelper.AdjustDepth(iconItem.trans.gameObject, depth)
		if scrollview ~= nil then
			UIHelper.SetDragScrollViewTarget(iconItem.trans, scrollview)
		end
		if not param.disableColid then
			local listener = Util.ChangeClick(iconItem.trans.gameObject, OnClickIconItem);
            listener.parameter = {itemId = id};
		else
			UIHelper.SetButtonActive(iconItem.trans, false)
		end
        local itemIcon = UIItemIcon.New(iconItem.trans.gameObject);
        itemIcon:SetSize("win_wb_20", Vector3.one * curScale);
        itemIcon:Init({ id = id, num = num}, false);
		-- local numLbl = TransformFindChild(iconItem.trans,countName)
		-- UIHelper.SetLabelTxt(numLbl,ConvertNumber(num))
		if param.color ~= nil then
			itemIcon:SetSize(param.color, Vector3.one * curScale);
		end
		-- Util.SetUITexture(TransformFindChild(iconItem.trans,iconName),LuaConst.Const.ItemIcon,Config.GetProperty(Config.ItemTable(), id, 'icon'), false)
	end
	return iconItem
end

--用于升级物品的icon

function OnClickIconItem(go)
    local listener = UIHelper.GetUIEventListener(go)
    print(listener ~= nil);
    if(listener~=nil and listener.parameter~=nil)then
        WindowMgr.ShowWindow(LuaConst.Const.UIItemTips,{id = listener.parameter.itemId})
    end
end

function RefreshItemCount( item, newCount )
	if item.trans == nil then
		return
	end
	--local lbl = TransformFindChild(item.trans, countName)
end

function OnDestroy()
	iconItemPref = nil
end