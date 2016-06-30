module("UIRecharge", package.seeall)
---------------------------------------
--test
---------------------------------------
require "Common/UnityCommonScript"
require "Game/RechargeData"

local window = nil
local windowComponent = nil
local transform = nil

local oneItem = {}
oneItem.label_price = nil
oneItem.__index = oneItem


function NewRechargeItem(transform, data)
    local t = {}
    setmetatable(t, oneItem)
    
    function OnClick()
        print("BuyItem id = "..(data.id or "nil"))
    end

    function t:Init()
        self.label_price = TransformFindChild(transform, "Label")
        Util.AddClick(transform.gameObject, OnClick)
    end
    t:Init()
    return t
end

function OnStart(gameObject, params)
    window = gameObject
    transform = gameObject.transform
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua")
    BindUI()
    -- windowComponent:AdjustSelfPanelDepth() 
    -- WindowMgr.LoadPrefab("UIHead",OnLoadPrefab)
end
function BindUI()

    --test
    local monthlyCardTrans = TransformFindChild(transform,"ItemList/1")
    Util.AddClick(monthlyCardTrans.gameObject, BuyMonthlyCard)
    -------------------------
    -- local transform = window.transform
    -- local rechargePrefab = windowComponent:GetPrefab("RechargeItem")
    -- rechargeRoot = TransformFindChild(transform, "Recharge")
    -- for i = 1, 8 do
    --     local clone = AddChild(rechargePrefab, rechargeRoot)
    --     local line = math.ceil(i / 4)
    --     local col = (i - 1) % 4
    --     clone.transform.localPosition = NewVector3(-265 + col * 176, -7 - (line - 1) * 183, 0)
    --     NewRechargeItem(clone.transform, { id = i })
    -- end

    -- privilegeRoot = TransformFindChild(transform, "Privilege")
    -- lArrow = TransformFindChild(transform, "Privilege/Sprite - LArrow")
    -- rArrow = TransformFindChild(transform, "Privilege/Sprite - RArrow")
    
    -- prevLvlSprite = TransformFindChild(lArrow, "Sprite - PrevLvl")
    -- nextLvlSprite = TransformFindChild(rArrow, "Sprite - NextLvl")
    -- Util.AddClick(lArrow.gameObject, ShowLower)
    -- Util.AddClick(rArrow.gameObject, ShowHigher)

    -- -- local returnBtn = TransformFindChild(transform, "LeftTopNode/Return")
    -- -- Util.AddClick(returnBtn.gameObject, Close)
    
    -- OnToggle(true)

    -- local toggle = TransformFindChild(transform, "Toggle")
    -- UIHelper.AddToggle(toggle, OnToggle)
    
    -- curLvlSprite = TransformFindChild(privilegeRoot, "Sprite - VIPLV")
    -- curShowLvlSprite = TransformFindChild(transform, "Sprite - LVL")
    -- UIHelper.SetSpriteName(curShowLvlSprite, tostring(vipLvl))

    -- UpdateLvl()
end
-- function OnLoadPrefab(gameObject_)
--     print("OnLoadPrefab")
--     local windowObject = GameObjectInstantiate(gameObject_)
--     windowObject.transform.parent = window.transform.parent
--     windowObject.transform.localPosition = Vector3.zero
--     windowObject.transform.localScale = Vector3.one
--     local uiHeadMgr = require("UIHeadScript")
--     uiHeadMgr.OnStart(windowObject)
-- end

function OnDestroy()
end

function OnShow()
    
end

function OnHide()

end

-- function Close()
--     windowComponent:Close()
-- end

function BuyMonthlyCard()
    RechargeData.RequrestBuyMonthlyCard(nil)
end

function ShowLower()
    if (vipLvl <= 0) then
        return
    end
    vipLvl = vipLvl - 1
    UpdateLvl()
end

function ShowHigher()
    if (vipLvl >= maxLvl) then
        return
    end
    vipLvl = vipLvl + 1
    UpdateLvl()
end

function UpdateLvl()
    UIHelper.SetSpriteName(curLvlSprite, tostring(vipLvl))

    if (vipLvl <= 0) then
        GameObjectSetActive(lArrow, false)
    else
        GameObjectSetActive(lArrow, true)
        UIHelper.SetSpriteName(prevLvlSprite, tostring(vipLvl - 1))
    end

    if (vipLvl >= maxLvl) then
        GameObjectSetActive(rArrow, false)
    else
        GameObjectSetActive(rArrow, true)
        UIHelper.SetSpriteName(nextLvlSprite, tostring(vipLvl + 1))
    end
end

function OnToggle(tf)
    GameObjectSetActive(rechargeRoot, tf)
    GameObjectSetActive(privilegeRoot, tf == false)
end