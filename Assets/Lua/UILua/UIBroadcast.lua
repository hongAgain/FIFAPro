module("UIBroadcast", package.seeall)

local rootWidget = nil;
local label = nil;

local moveSpeed = 100;
local isHidden = true;
local hideAnimating = nil;
local isScrolling = false;

local msgQueue = {};

local reqInterview = -90;    --second
local timerId = nil;
local default = { id = "1", args = { "1", "2" } };

function Start(go)
    DataSystemScript.RegisterMsgHandler(MsgID.tb.GetBroadCast, OnMsg);
    
    rootWidget = go.transform;
    label = TransformFindChild(rootWidget, "Panel/Label");

    HideImmediately();
    
    if (timerId == nil) then
        timerId = LuaTimer.AddTimer(true, reqInterview, ReqMsg);
    end
    
    ReqMsg();
end

function OnEnable()
    if (timerId == nil) then
        timerId = LuaTimer.AddTimer(true, reqInterview, ReqMsg);
    end
end

function Update(deltaTime)
    if (#msgQueue > 0 and isHidden) then
        ShowImmediately();
    elseif (#msgQueue == 0 and isHidden == false and isScrolling == false) then
        hideAnimating = 0;
        isHidden = true;
    end
    
    function FormatUserInfo(uName, uVip)
        return "[FFFFFF](VIP"..uVip..")[-]"..uName;
    end
    
    if (isHidden == false) then
        if (isScrolling == false) then
            
            if (#msgQueue > 0) then
                isScrolling = true;

                local data = msgQueue[1] or default;
                table.remove(msgQueue, 1);

                if (data.type == 1) then
                    local str = Config.GetProperty(Config.broadcastTB, data.id, "content") or "";
                    for i = 1, #data.args do
                        -- local param = "[66CCFF]"..data.args[i].."[-]";
                        -- local param = data.args[i];
                        -- if (data['uinfo'] ~= nil) then
                        --     local info = data['uinfo'][data.args[i]];
                        --     param = FormatUserInfo(info.name, info.vip);
                        -- end
                        -- str = string.gsub(str, "{"..i.."}", param);

                        local param = data.args[i];
                        if (data['uinfo'] ~= nil and data['uinfo'][data.args[i]]~=nil) then
                            param = FormatUserInfo(data['uinfo'][data.args[i]].name,data['uinfo'][data.args[i]].vip);
                        end
                        str = string.gsub(str, "{"..i.."}", param);
                    end
                    UIHelper.SetLabelTxt(label, str);
                elseif (data.type == 2) then
                    local str = FormatUserInfo(data.sender_name, data.sender_vip)..":"..data.msg;
                    UIHelper.SetLabelTxt(label, str);
                else
                    UIHelper.SetLabelTxt(label, data);
                end

                label.localPosition = Vector3.New(300, 0, 0);
            end
        end
        
        if (isScrolling == true) then
            label.localPosition = label.localPosition + Vector3.left * moveSpeed * deltaTime;
            local width = UIHelper.WidthOfWidget(label);
            if (label.localPosition.x <= -300 - width) then
                isScrolling = false;
            end
        end
    end
    
    if (hideAnimating ~= nil) then
        local alpha = math.lerp(1, 0, hideAnimating / 2);
        UIHelper.SetWidgetColor(rootWidget, Color.New(1, 1, 1, alpha));
        
        hideAnimating = hideAnimating + deltaTime;
        if (hideAnimating / 2 >= 1) then
            hideAnimating = nil;
        end
    end
end

function OnDestroy()
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.GetBroadCast, OnMsg);
    rootWidget = nil;
    label = nil;
    
    isHidden = true;
    hideAnimating = nil;
    isScrolling = false;
    msgQueue = {};
end

function OnDisable()
    LuaTimer.RmvTimer(timerId);
    timerId = nil;
end

function ShowImmediately()
    UIHelper.SetWidgetColor(rootWidget, Color.New(1, 1, 1, 1));
    isHidden = false;
    hideAnimating = nil;
end

function HideImmediately()
    UIHelper.SetWidgetColor(rootWidget, Color.New(1, 1, 1, 0));
    isHidden = true;
end

function ReqMsg()
    local param = {};
    param['num'] = 10;
    
    DataSystemScript.RequestWithParams(LuaConst.Const.GetBroadCast, param, MsgID.tb.GetBroadCast, false);
end

function OnMsg(code, data)
    local function SortBySequence(a, b)
        if (a.time > b.time) then
            return -1;
        elseif (a.time == b.time) then
            return 0;
        else
            return 1;
        end
    end
    
    local sorted = CommonScript.QuickSort(data, SortBySequence);
    for i, v in ipairs(sorted) do
        local insert = true;
        local lastOne = nil;
        if (#msgQueue > 0) then
            lastOne = msgQueue[#msgQueue];
            local time1 = lastOne.time;
            local time2 = v.time;
            if (time1 ~= nil and time1 >= time2) then
                insert = false;
            end
        end
        
        if (insert) then
            table.insert(msgQueue, v);
        end
    end
    
    while (#msgQueue > 10) do
        table.remove(msgQueue, 1);
    end
    
    local tb = Config.GetTemplate(Config.gameTipsTB);
    while (#msgQueue < 10) do
        local key = tostring(math.random(1, 10));
        table.insert(msgQueue, tb[key]["content"]);
    end
end