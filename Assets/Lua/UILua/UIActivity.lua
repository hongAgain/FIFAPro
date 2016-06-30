module("UIActivity", package.seeall)

require "Common/UnityCommonScript"
require "Game/ActivityData"

--activities
require "UILua/UIActFreePower"
require "UILua/UIActLevelFund"
require "UILua/UIActSevenLogin"
require "UILua/UIActAccLogin"
require "UILua/UIActLvlGift"
require "UILua/UIActLvRankReward"
require "UILua/UIActModule1"

--ui配置数据，类型与ui的对应，包括uiModule,prefab,tabIcon
--每个uimodule需要有Init,OnShow,OnHide,OnDestroy四个函数
local uiTypeTable = {}
uiTypeTable[1] = {uiModule = UIActFreePower, prefab = "UIActFreeEnergy" , tabIcon = "3"}  
uiTypeTable[2] = {uiModule = UIActLevelFund, prefab = "UIActLevelFund", tabIcon = "6"}
uiTypeTable[3] = {uiModule = UIActSevenLogin, prefab = "UIActSevenLogin", tabIcon = "8"}
uiTypeTable[4] = {uiModule = UIActAccLogin, prefab = "UIActAccLogin", tabIcon = "10"}
uiTypeTable[5] = {uiModule = UIActLvlGift, prefab = "UIActLevelGift", tabIcon = "9"}
uiTypeTable[6] = {uiModule = UIActLvRankReward, prefab = "UIActLvRankReward", tabIcon = "11"}
uiTypeTable[7] = {uiModule = UIActModule1, prefab = "UIActModule1", tabIcon = "12"}
--限时活动UI显示
local timeActUIConfig = {}
timeActUIConfig["100004"] = {rule = "UIActDCRule", itemTitle1 = "UIActDCItemTitle1", itemTitle2 = "UIActDCItemTitle2"} --钻石消费
--ui
local window = nil
local windowComponent = nil
local scrollRoot = nil
local tabContainer = nil
local activityContainer = nil
local pageDotBg = nil
local pageDotParent = nil
local tabPrefab = nil
--show
local subPageList = nil --里面包含活动的info
local pageDotList = {}

function OnStart(gameObject)
    window = gameObject
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua")
    BindUI()
    InitInfo()
end

function OnDestroy()

    OnClose()
    -- windowComponent:Close()
    DestroyActivityPrefabs()
end

function TryCloseUI()
    -- DestroyActivityPrefabs()
    -- page = {}
    -- GameObjectSetActive(window,false)
    print(">>>>>>>>>>>>>>>>>>UIActivity TryCloseUI.")

    OnClose()
    windowComponent:Close()
    -- luaTb = {}
end

function OnShow()
end

function OnHide()
end

function DestroyActivityPrefabs()
    for _, v in pairs(subPageList) do
        if v.isTimeLimit then
            if v.scriptObj ~=nil then
                v.scriptObj:OnDestroy()
            end
        else
            local mod = uiTypeTable[v.uiType].uiModule
            if (mod~= nil) then
                mod.OnDestroy()
            end
        end
        if (v.prefabClone ~= nil) then
            GameObjectDestroy(v.prefabClone)
            v.prefabClone=nil
        end
    end
    for i,v in ipairs(pageDotList) do
        GameObjectDestroy(v)
    end
    pageDotList = {}
end

function BindUI()
    local transform = window.transform
    -- local btn_close = TransformFindChild(transform, "UIBG/Return")
    -- Util.AddClick(btn_close.gameObject, Close)

    scrollRoot = TransformFindChild(transform, "Scroll View")
    tabContainer = TransformFindChild(scrollRoot, "Grid")
    activityContainer = TransformFindChild(transform,"ActivityContainer")
    pageDotBg = TransformFindChild(transform,"UIBG/LeftBG/PageBG")
    pageDotParent = TransformFindChild(transform,"PageDotList")
end

function InitInfo()
    ActivityData.ReqActiveInfo(CreateActiveSubUI)
end

-- 常驻性活动和限时性活动
function CreateActiveSubUI(regularActs, timeActs)
    if tabPrefab == nil then
        tabPrefab = windowComponent:GetPrefab("Tab")
    end
    subPageList = {} --清零
    for i,v in ipairs(regularActs) do
        subPageList[#subPageList + 1] = v
    end
    for i,v in ipairs(timeActs) do
        if uiTypeTable[v.uiType] ~= nil then
            local curInfo = v
            curInfo.isTimeLimit = true
            curInfo.uiConfig = timeActUIConfig[v._id]
            subPageList[#subPageList + 1] = curInfo
        end
    end

    for i,v in ipairs(subPageList) do
        --create tab
        local child = InstantiatePrefab(tabPrefab, tabContainer, "Tab_"..i)
        local child_transform = child.transform
        UIHelper.AddToggle(child_transform, OnToggleChange)
        UIHelper.SetDragScrollViewTarget(child_transform, scrollRoot)
        local uiType = v.uiType
        InitTabStyle(child_transform,uiType,v)

        InitTabPageStyle(v, uiType)
    end
    UIHelper.RepositionGrid(tabContainer, scrollRoot)
    InitPageDot(#subPageList)
    UIHelper.SetToggleState(tabContainer:Find("Tab_1"), true)
end

function InitTabStyle( transform,uiType,info )
    --根据info数据调整tab样式
    local img = TransformFindChild(transform,"IMG")
    --local title = TransformFindChild(transform,"NoLimitTime/Title")
    --判断是否是有时间限制的，是则开启limitTime
    --UIHelper.SetLabelTxt(title,activitiesTable[index].name)
    UIHelper.SetSpriteNameNoPerfect(img,"Img_"..uiTypeTable[uiType].tabIcon)
end

function InitTabPageStyle(info, uiType)
    local curTypeTable = uiTypeTable[uiType]
    if curTypeTable == nil then
        return
    end
    local prefab = windowComponent:GetPrefab(curTypeTable.prefab)
    if (prefab ~= nil) then
        local clone = InstantiatePrefab(prefab, activityContainer)
        clone:SetActive(false)
        info.prefabClone = clone
        local _module = curTypeTable.uiModule
        if (_module ~= nil and uiTypeTable[uiType].uiModule ~= "") then
            if info.isTimeLimit then
                info.scriptObj = _module:new(clone.transform, windowComponent, info)--todo
            else
                _module.Init(clone.transform,windowComponent,info) --初始化子页面，info为服务器数据
            end
        end
    end
end

function InitPageDot(count)
    local pageDotPref = windowComponent:GetPrefab("PageDot")
    local hight = UIHelper.HeightOfWidget(pageDotBg)
    local space = 20
    UIHelper.SetSizeOfWidget(pageDotBg,NewVector2(space * (count + 1),hight))
    pageDotParent.position = NewVector3(pageDotBg.position.x,pageDotParent.position.y,pageDotParent.position.z)
    local lp = pageDotParent.localPosition
    pageDotParent.localPosition = NewVector3(lp.x - (count -1) * space / 2,lp.y,lp.z)
    pageDotList = {}
    for i = 0,count-1 do
        local clone = AddChild(pageDotPref,pageDotParent)
        clone.transform.localPosition = NewVector3(i*space,0,0)
        pageDotList[i+1] = clone.transform
    end
end

function PageDotSelected(idx, selected )
    local selectObj = TransformFindChild(pageDotList[idx],"HighLight").gameObject
    selectObj:SetActive(selected)
end

function OnToggleChange(selected, trans)
    if selected then
        local idx = string.gsub(trans.name, "Tab_", "")
        --show current & hide other
        for i, v in ipairs(subPageList) do
            if v.prefabClone ~= nil then
                local beSelected = (i == tonumber(idx))
                PageDotSelected(i,beSelected)
                local mod = uiTypeTable[v.uiType].uiModule
                if beSelected then
                    if v.prefabClone.activeSelf == false then
                        v.prefabClone:SetActive(true)
                        if v.scriptObj then
                            v.scriptObj:OnShow()
                        elseif (mod ~= nil) then
                            mod.OnShow()
                        end
                    end
                else
                    if v.prefabClone.activeSelf == true then
                        v.prefabClone:SetActive(false)
                        if v.scriptObj then
                            v.scriptObj:OnHide()
                        elseif mod ~= nil then
                            mod.OnHide()
                        end
                    end
                end
            end--endif

        end -- endfor
    end
    local selectedTrans = TransformFindChild(trans, "Select")
    selectedTrans.gameObject:SetActive(selected)
end

function OnClose()
    for i, v in ipairs(subPageList) do
        if v.prefabClone ~= nil then
            v.prefabClone:SetActive(false)
        end
        if v.isTimeLimit then
            if v.scriptObj ~=nil then
                v.scriptObj:OnHide()
            end
        else
            local mod = uiTypeTable[v.uiType].uiModule
            if (mod ~= nil) then
                mod.OnHide()
            end      
        end  
    end
end

