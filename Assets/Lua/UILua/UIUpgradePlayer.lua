--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIUpgradePlayer", package.seeall)

local window = nil;
local windowComponent = nil;

local m_item = nil;
local m_scrollView = nil;
local tex_tipsText = nil;

local m_playerDataTb = {};
local Callback = nil;

function OnStart(gameObject, params)
    m_playerDataTb = params.Data;
    Callback = params.Callback;
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();
end

function BindUI()
    local transform = window.transform;

    m_item = windowComponent:GetPrefab("Item");
    m_scrollView = TransformFindChild(transform, "Center/ScrollView");
    tex_tipsText = TransformFindChild(transform, "Center/UpgradeTips");

    Util.AddClick(TransformFindChild(transform, "Center/BtnSure").gameObject, BtnSure);
    Util.AddClick(TransformFindChild(transform, "Center/BtnClose").gameObject, BtnClose);

    InitUpgradePlayer();
    Util.SetShaderPropertiesInt(tex_tipsText.gameObject,"Custom/Shine","_BoolZero",1);
    LuaTimer.AddTimer(true,0.8,PlayShine)
end

function PlayShine()
    Util.SetShaderPropertiesInt(tex_tipsText.gameObject,"Custom/Shine","_BoolZero",0);
    GameObjectSetActive(tex_tipsText,false);
    GameObjectSetActive(tex_tipsText,true);
end

function InitUpgradePlayer()
    if #m_playerDataTb < 1 then
        return;
    end

    for k,v in pairs(m_playerDataTb) do
        local clone = InstantiatePrefab(m_item,m_scrollView);

        local sprIcon = TransformFindChild(clone.transform,"Icon/Sprite");
        UIHelper.SetLabelTxt(TransformFindChild(clone.transform,"Name"),Hero.ColorHeroName(Hero.GetCurrQuality(v.id),HeroData.GetHeroName(v.id)));
        UIHelper.SetWidgetColor(TransformFindChild(clone.transform,"Line"),Hero.GetHeroQualityBannerBg(Hero.GetCurrQuality(v.id)));
--        UIHelper.SetSpriteName(sprIcon,HeroData.GetHeroIcon(v.id));
--        if IsFullLv(v.lv) then
--            GameObjectSetActive(TransformFindChild(clone.transform,"Full"),true);
--            GameObjectSetActive(TransformFindChild(clone.transform,"NoFull"),false);
--            UIHelper.SetLabelTxt(TransformFindChild(clone.transform,"Full/Lv"),"Lv.".. v.lv);
--        else
        GameObjectSetActive(TransformFindChild(clone.transform,"Full"),false);
        GameObjectSetActive(TransformFindChild(clone.transform,"NoFull"),true);
        UIHelper.SetLabelTxt(TransformFindChild(clone.transform,"NoFull/Lv/Label"),"Lv.".. v.lv);
        UIHelper.SetSpriteFillAmount(TransformFindChild(clone.transform,"NoFull/Lv/Forward"),v.lvRatio);
      --  end
    end
end


function IsFullLv(playerLv_)
    local limitPlayerLv = Config.GetProperty(Config.LevelTable(), tostring(Role.Get_lv()),"heroLv");
    if playerLv_ > limitPlayerLv then
        return true;
    end

    return false;
end

function SetPlayerDataTb(tb_)
    m_playerDataTb = tb_;
end

function BtnSure()
    ExitUIUpgradePlayer();
end

function BtnClose()
    ExitUIUpgradePlayer();
end

function OnDestroy()
    window = nil;
    windowComponent = nil;
end


function ExitUIUpgradePlayer()
    if Callback ~= nil then
        Callback();
        Callback = nil;
    end
    
    windowComponent:Close();
end






