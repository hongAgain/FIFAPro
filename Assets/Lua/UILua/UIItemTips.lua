--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion


module("UIItemTips", package.seeall)

local window = nil;
local windowComponent = nil;

local tex_icon = nil;
local lbl_title = nil;
local lbl_content = nil;
local bg = nil

local m_itemTb = {};
function OnStart(gameObject, params)
    m_itemTb.id = params.id;
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();
end

function BindUI()
    local transform = window.transform;

    lbl_title = TransformFindChild(transform, "Describe/Title");
    lbl_content = TransformFindChild(transform, "Describe/Content");
    tex_icon = TransformFindChild(transform, "Icon/Icon");
    bg = TransformFindChild(transform,"Bg1")
    if m_itemTb.id ~= nil then
        local icon = Config.GetProperty(Config.ItemTable(),tostring(m_itemTb.id),'icon') or " ";
        local name = Config.GetProperty(Config.ItemTable(),tostring(m_itemTb.id),'name') or "Error";
        local desc = Config.GetProperty(Config.ItemTable(),tostring(m_itemTb.id),'desc') or " ";

        UIHelper.SetLabelTxt(lbl_title,"【".. name .."】");
        UIHelper.SetLabelTxt(lbl_content,desc);
        Util.SetUITexture(tex_icon,LuaConst.Const.ItemIcon,icon, true);
    end

    Util.AddClick(TransformFindChild(transform, "Close").gameObject, BtnClose);
    Util.AddClick(bg.gameObject, BtnClose)
end


function BtnClose()
    ExitUIItemTips();
end

function OnDestroy()
    window = nil;
    windowComponent = nil;

end

function ExitUIItemTips()
   windowComponent:Close();

end




