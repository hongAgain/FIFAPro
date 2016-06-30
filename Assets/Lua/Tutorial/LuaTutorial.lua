module("LuaTutorial", package.seeall)
require "Tutorial/Tutorial_UICreateRole"
require "Tutorial/Tutorial_UIScout"
require "Tutorial/Tutorial_UIFormation"
require "Tutorial/Tutorial_UITeamLegend"
require "Tutorial/Tutorial_UICreateRole2"

function OnInit()
    DataSystemScript.RegisterMsgHandler(MsgID.tb.SetTutorialStep, OnSteped);
end

function OnRelease()
    DataSystemScript.RegisterMsgHandler(MsgID.tb.SetTutorialStep, OnSteped);
end

function Create()
    local step = Role.Get_step();
    if (step == 2) then
        Tutorial_UIScout.Start();
    elseif (step == 3) then
        Tutorial_UIFormation.Start();
    elseif (step == 4) then
        Tutorial_UITeamLegend.Start();
    elseif (step == 5) then
        Tutorial_UICreateRole2.Start();
    end
end

function ReadConfig(id, param)
    local data = Config.GetTemplate(Config.tutorialTB)[id];
    
    local talkFramePos = Vector4.zero;
    if (data.framePos ~= "") then
        local split = data.framePos:split(',');
        talkFramePos.x = split[1];
        talkFramePos.y = split[2];
        talkFramePos.z = split[3];
        talkFramePos.w = split[4];
    end
    
    local highlightPos = Vector3.one;
    local highlightW = 1;
    local highlightH = 1;
    
    if (data.highlightPos ~= "") then
        local split = data.highlightPos:split(',');
        highlightPos.x = split[1];
        highlightPos.y = split[2];
        highlightW = split[3];
        highlightH = split[4];
    end
    
    local showArrow = (data.showArrow == 1);
    Tutorial.Instance():ActiveArrow(showArrow);
    
    local arrowInfo = data.arrowInfo;
    if (arrowInfo ~= "") then
        local split = arrowInfo:split(',');
        Tutorial.Instance():SetArrow(Vector2.New(split[1], split[2]), split[3] == "1", split[4]);
    else
        Tutorial.Instance():SetArrow(Vector2.zero, false, 0);
    end
    
    local strMsg = data.npcSpeech;
    if (param ~= nil) then
        for i = 1, #param do
            strMsg = string.gsub(strMsg, "{"..(i - 1).."}", param[i]);
        end
    end

    Tutorial.Instance():ShowTalk(data.npcTalkType, data.npcImg, strMsg, talkFramePos);
    
    Tutorial.Instance().hand:SetActive(data.showFinger == 1);
    
    if (data["tutorialType"] == "1") then
        Tutorial.Instance():FocusFullScreen();
        Tutorial.Instance().animation:Rewind("Tutorial_Type1");
        Tutorial.Instance().animation:Play("Tutorial_Type1");
    elseif (data["tutorialType"] == "2") then
        Tutorial.Instance():FocusFullScreen();
        Tutorial.Instance():Highlight(highlightPos, highlightW, highlightH);
    elseif (data["tutorialType"] == "3") then
        Tutorial.Instance():Highlight(highlightPos, highlightW, highlightH);
        Tutorial.Instance().animation:Rewind("Tutorial_Type2");
        Tutorial.Instance().animation:Play("Tutorial_Type2");
    elseif (data["tutorialType"] == "4") then
        Tutorial.Instance():Highlight(highlightPos, highlightW, highlightH);
    end
    
    local align = Vector2.one;
    local alignment = data["alignment"];
    if (alignment ~= "") then
        local split = alignment:split(',');
        align.x = split[1];
        align.y = split[2];
    end
    Tutorial.Instance():HandleAlignment(align.x, align.y);
    
    Tutorial.Instance().dragFXRoot:SetActive(false);
end

function SetStep(id)
    Role.Set_step(id);
    --Told Server
    local param = {};
    param['step'] = id;
    DataSystemScript.RequestWithParams(LuaConst.Const.SetTutorialStep, param, MsgID.tb.SetTutorialStep);
end

local mTempCB = nil;
local mUIName = nil;
function RegisterTempOnOpenWindowCB(uiName, cb)
    mUIName = uiName;
    mTempCB = cb;
end

function OnOpenWindow(gameObject)
    if (mTempCB ~= nil and (mUIName == gameObject.name)) then
        mTempCB(gameObject);
        mTempCB = nil;
    end
end

function OnSteped(code, data)
    
end
