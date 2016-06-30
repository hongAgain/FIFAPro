--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIBattleDetail", package.seeall)

local window = nil;
local windowComponent = nil;
local m_upIcon = nil;
local m_downIcon = nil;

local m_scrollView = nil;
local m_grid = nil;

local m_currIndex = 1;
local m_dotTb = {};
function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();

end

function BindUI()
    local transform = window.transform;

    m_upIcon = windowComponent:GetPrefab("UpIcon");
    m_downIcon = windowComponent:GetPrefab("DownIcon");

    m_scrollView = TransformFindChild(transform, "CenterPanel/ScrollViewPanel");
    m_grid = TransformFindChild(transform, "CenterPanel/ScrollViewPanel/Grid");

    UIHelper.AddDragOnStarted(m_scrollView,OnDragFinishStart);
    UIHelper.AddDragOnFinish(m_scrollView,OnDragFinish);
    local lblMeName = TransformFindChild(transform, "CenterPanel/ScrollViewPanel/Grid/Detail2/Role/LabelName");
    local lblMeLv = TransformFindChild(transform, "CenterPanel/ScrollViewPanel/Grid/Detail2/Role/LabelLv");
    UIHelper.SetLabelTxt(lblMeName,Role.Get_name());
    UIHelper.SetLabelTxt(lblMeLv,Role.Get_lv());

    local enemyName,enemyLv,enemyIcon = TeamLegendData.GetEnemyNameLv(tostring(TeamLegendData.GetCurrLevelId()));
    local lblEnemyName = TransformFindChild(transform, "CenterPanel/ScrollViewPanel/Grid/Detail3/Role/LabelName");
    local lblEnemyLv = TransformFindChild(transform, "CenterPanel/ScrollViewPanel/Grid/Detail3/Role/LabelLv");
    UIHelper.SetLabelTxt(lblEnemyName,enemyName);
    UIHelper.SetLabelTxt(lblEnemyLv,enemyLv);

    local btnClose = TransformFindChild(transform, "Title/BtnReturn").gameObject;
    Util.AddClick(btnClose, BtnClose);

    m_dotTb = {};
    for i=1,3 do
        local tf = TransformFindChild(transform, "BottomPage/Dot"..i.."/Sprite2");
        table.insert(m_dotTb,tf);
    end

    RefreshDot();
end

function RefreshDot()
    for i=1,3 do
        if i == m_currIndex then
            GameObjectSetActive(m_dotTb[i],true);
        else
            GameObjectSetActive(m_dotTb[i],false);
        end
    end

end

function OnDragFinishStart(gameObject_)
    Util.EnableScript(m_grid.gameObject,"UICenterOnChild",false);
end

function OnDragFinish(gameObject_)
    Util.EnableScript(m_grid.gameObject,"UICenterOnChild",true);
    local centerObject = UIHelper.CenterOnRecenter(m_grid);
    if centerObject.name == "Detail1" then
        m_currIndex = 1;
    elseif centerObject.name == "Detail2" then
        m_currIndex = 2;
    elseif centerObject.name == "Detail3" then
        m_currIndex = 3;
    end

    RefreshDot();
end


function BtnClose()

    ExitUIBattleDetail();
end

function OnDestroy()
    window = nil;
    windowComponent = nil;
    m_currIndex = 1;

end



function ExitUIBattleDetail()
   windowComponent:Close();
end



