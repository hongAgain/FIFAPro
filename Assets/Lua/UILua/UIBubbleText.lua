module("UIBubbleText", package.seeall)

local prefab = nil
local transform = nil
local bubbleList = nil

local bubbleText = 
{
	transform = nil,
}
bubbleText.__index = bubbleText

function NewBubble(content, color)
	local inst = {}
	setmetatable(inst, bubbleText)
	if prefab == nil then
		prefab = Util.GetGameObject("UIBubbleText")
	end
	local clone = InstantiatePrefab(prefab,WindowMgr.UIParent).transform
	inst.transform = clone
	local textLbl = TransformFindChild(clone, "Text")
	UIHelper.SetLabelTxt(textLbl, content)
	if color ~= nil then
		UIHelper.SetWidgetColor(textLbl, color)
	end
	local bg = TransformFindChild(clone, "BG")
	local textWidth = UIHelper.SizeOfWidget(textLbl).x
	local bgOSize = UIHelper.SizeOfWidget(bg)
	UIHelper.SetSizeOfWidget(bg, NewVector2(bgOSize.x+textWidth, bgOSize.y))
	local endFun = function()
		inst:Destroy()
	end
	UIHelper.SetTweenPositionOnFinish(clone, endFun)

	if bubbleList == nil then
		bubbleList = {}
	end
	if #bubbleList ~= 0 then --每次创建新的就销毁之前的
		for i,v in ipairs(bubbleList) do
			if v ~= nil then
				v:Destroy()
			end
		end
		bubbleList = {}
	end
	bubbleList[#bubbleList + 1] = inst
	return inst
end

function bubbleText:Destroy()
	if self.transform ~= nil then
		GameObjectDestroy(self.transform.gameObject)
		self.transform = nil
	end
end