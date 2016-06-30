--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIGetItemScript", package.seeall)


local window = nil;
local windowComponent = nil;

local m_grid = nil;
local m_gridItem = nil;
local bg = nil


--params
local m_itemTb = {};
local titleName = nil;
local getButtonName = nil;
local delegateFuncOnClose = nil;
local delegateFuncOnGet = nil

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    m_itemTb = params.m_itemTb;
    titleName = params.titleName;
    getButtonName = params.getButtonName;
    delegateFuncOnClose = params.OnClose;
    delegateFuncOnGet = params.OnGet

    BindUI();
end

function BindUI()
    local transform = window.transform;

    m_grid = TransformFindChild(transform,"uiScrollView");
    m_gridItem = windowComponent:GetPrefab("Item");
    bg = TransformFindChild(transform, "bgMask")

    Util.AddClick(TransformFindChild(transform,"BtnGet").gameObject,BtnGet);
    Util.AddClick(TransformFindChild(transform,"BtnClose").gameObject,BtnClose);
    Util.ChangeClick(bg.gameObject, BtnClose)

    if(titleName~=nil)then
        UIHelper.SetLabelTxt(TransformFindChild(transform,"Title/Label"),titleName);
    else
        UIHelper.SetLabelTxt(TransformFindChild(transform,"Title/Label"),Util.LocalizeString("AwardList"));
    end

    if(getButtonName~=nil)then
        UIHelper.SetLabelTxt(TransformFindChild(transform,"BtnGet/Label"),getButtonName);
    else
        UIHelper.SetLabelTxt(TransformFindChild(transform,"BtnGet/Label"),Util.LocalizeString("Yes"));
    end

    RefreshItem();
end

function RefreshItem()
    local size = 0;
    local itemTb = {};
    for k,v in pairs(m_itemTb['item']) do
        size = size + 1;
        table.insert(itemTb,v);
    end

    for i=1,size do
        local clone = InstantiatePrefab(m_gridItem,m_grid).transform;
        clone.name = i;
        AddOrChangeClickParameters(clone.gameObject,OnClickItem,{itemId = itemTb[i].id});
        UIHelper.SetLabelTxt(TransformFindChild(clone,"Num"),"x"..itemTb[i].num);
        Util.SetUITexture(TransformFindChild(clone,"Icon"),LuaConst.Const.ItemIcon,Config.GetProperty(Config.ItemTable(), itemTb[i].id, 'icon'), false);
    end
end

function OnClickItem(go)
    local listener = UIHelper.GetUIEventListener(go);
    if(listener~=nil and listener.parameter~=nil)then
        print(listener.parameter.itemId)
        WindowMgr.ShowWindow(LuaConst.Const.UIItemTips,{id = listener.parameter.itemId});
    end
end

function BtnGet()
    if delegateFuncOnGet ~= nil then
        delegateFuncOnGet()
    end
    Close();
end

function BtnClose()
    Close();
end

function Close()
    if(delegateFuncOnClose~=nil) then
        delegateFuncOnClose();
    end
    windowComponent:Close();
end

function OnDestroy()
    window = nil;
    windowComponent = nil;
    m_itemTb = {};
    titleName = nil;
    getButtonName = nil;
    delegateFuncOnClose = nil;
end

