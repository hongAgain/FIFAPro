module("AutoReqMsg", package.seeall)

--require("Game/DataSystemScript");

local tt = { sequenceMsg = {} };
tt.__index = tt;

function New()
    local t = {};
    setmetatable(t, tt);
    
    function t:EnqueueMsg(msgId, msgHandler)
        table.insert(t.sequenceMsg, { id = msgId, handler = msgHandler });
        DataSystemScript.RegisterHookMsgHandler(msgId, Loop);
    end

    function t:DequeueMsg()
        local msg = t:PeekMsg();
        table.remove(t.sequenceMsg, 1);
    end

    function t:PeekMsg()
        if (#t.sequenceMsg ~= 0) then
            local tb = t.sequenceMsg[1];
            return tb;
        else
            return nil;
        end
    end

    function t:OnDone(doOnAllRequested)
        t.doOnAllRequested = doOnAllRequested;
    end

    function t:DoMsgReq()
        local tb = t:PeekMsg();
        if (tb ~= nil) then
            tb.handler();
        else
            if (self.doOnAllRequested ~= nil) then
                self.doOnAllRequested();
            end
        end
    end

    function Loop(msgId, data)
        local top = t:PeekMsg();
        t:DequeueMsg();
        DataSystemScript.UnRegisterHookMsgHandler(top.id, top.handler);
        t:DoMsgReq();
    end

    return t;
end