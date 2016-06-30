--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIUpgradeCoach", package.seeall)

local window = nil;
local windowComponent = nil;
local tex_tipsText = nil;

local lbl_leftLv = nil;
local lbl_rightLv = nil;
local Callback = nil;
function OnStart(gameObject, params)
    Callback = params.Callback;
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();
end

function BindUI()
    local transform = window.transform;

    tex_tipsText = TransformFindChild(transform, "Center/UpgradeTips");
    UIHelper.SetLabelTxt(TransformFindChild(transform, "Center/LvTeam/LabelLeft"),"Lv.".. Role.Get_lvBack());
    UIHelper.SetLabelTxt(TransformFindChild(transform, "Center/LvTeam/LabelRight"),"Lv.".. Role.Get_lv());
    UIHelper.SetLabelTxt(TransformFindChild(transform, "Center/Power/LabelLeft"),Config.GetProperty(Config.LevelTable(),tostring(Role.Get_lvBack()),"Power"));
    UIHelper.SetLabelTxt(TransformFindChild(transform, "Center/Power/LabelRight"),Config.GetProperty(Config.LevelTable(),tostring(Role.Get_lv()),"Power"));
    UIHelper.SetLabelTxt(TransformFindChild(transform, "Center/PowerCurr/LabelLeft"),Role.Get_power()-Config.GetProperty(Config.LevelTable(),tostring(Role.Get_lvBack()),"Power_reward"));
    UIHelper.SetLabelTxt(TransformFindChild(transform, "Center/PowerCurr/LabelRight"),Role.Get_power());
    UIHelper.SetLabelTxt(TransformFindChild(transform, "Center/LvPlayer/LabelLeft"),"Lv.".. Config.GetProperty(Config.LevelTable(),tostring(Role.Get_lvBack()),"heroLv"));
    UIHelper.SetLabelTxt(TransformFindChild(transform, "Center/LvPlayer/LabelRight"),"Lv.".. Config.GetProperty(Config.LevelTable(),tostring(Role.Get_lv()),"heroLv"));


    Util.AddClick(TransformFindChild(transform, "Center/BtnSure").gameObject, BtnSure);
    Util.AddClick(TransformFindChild(transform, "Center/BtnClose").gameObject, BtnClose);
    Util.SetShaderPropertiesInt(tex_tipsText.gameObject,"Custom/Shine","_BoolZero",1);
    LuaTimer.AddTimer(true,0.8,PlayShine);
end

function PlayShine()
    Util.SetShaderPropertiesInt(tex_tipsText.gameObject,"Custom/Shine","_BoolZero",0);
    GameObjectSetActive(tex_tipsText,false);
    GameObjectSetActive(tex_tipsText,true);
end

function BtnSure()
    ExitUIUpgradeCoach();
end

function BtnClose()
    ExitUIUpgradeCoach();
end


function OnDestroy()
    window = nil;
    windowComponent = nil;

end


function ExitUIUpgradeCoach()
    if Callback ~= nil then
        Callback();
        Callback = nil;
    end
   windowComponent:Close();
end



