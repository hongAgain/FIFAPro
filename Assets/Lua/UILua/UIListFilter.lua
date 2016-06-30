module("UIListFilter", package.seeall)

require "Common/UnityCommonScript"
require "UILua/UICircleListManager"

--params={title,contentList,confirmCB,defaulFilterId} 
function OnStart(gameObject, params)
	local filterId = params.defaulFilterId or 1
	local transform = gameObject.transform
	local windowComponent = GetComponentInChildren(gameObject, "UIBaseWindowLua")
	local titleLbl = TransformFindChild(transform, "Title")
	local confirmBtn = TransformFindChild(transform, "ConfirmBtn")
	local closeBtn = TransformFindChild(transform, "CloseBtn")
	local sv = TransformFindChild(transform, "ScrollView")
	local grid = TransformFindChild(transform, "ScrollView/UIGrid")
	UIHelper.SetLabelTxt(titleLbl, params.title)
	local closeFun = function(go)
		windowComponent:Close()
	end
	local confirmFun = function(go)
		if params.confirmCB ~= nil then
			params.confirmCB(filterId)
		end
		closeFun()
	end
	Util.AddClick(closeBtn.gameObject, closeFun)
	Util.AddClick(confirmBtn.gameObject, confirmFun)
	local itemDatas = {}
	for i,v in ipairs(params.contentList) do
		itemDatas[i] = {groupID = i, value = v}
	end
	local circleListManager = UICircleListManager.New()
	local createGroupItemFun = function(randomIndex, key, value, itemNameTrans)
		UIHelper.SetLabelTxt(itemNameTrans, value.value)
	end
	local clickItemFun = function(param)
		if(param.data ~= nil) then
			filterId = param.data.groupID
		end
	end
	circleListManager:CreateUICircleList(sv, grid, itemDatas, 
		{
			OnCreateItem = createGroupItemFun,
			OnClickItem = clickItemFun,
			OnSelectByDrag = clickItemFun
		},
		1,filterId)

end