module("HintManager",package.seeall);

require "UILua/UIHintSettings"

--this one handles delegate activations for any hintType

--table of delegate tables: hintDelegates = { type - { key - {OnHintActive,OnCheckHintContent,OnCheckHintCache} } }
--delegate is called with a param called directShowHintFlag, pass nil or (true/false)
local hintDelegates = {};
local currentKey = 0;

local currentFuncTipsData = nil;

function RegisterOnHintActiveDelegate( hintType, delegateOnHintActive, delegateOnCheckHintContent, delegateOnCheckHintCache )
	if(hintDelegates==nil)then
		hintDelegates = {};
	end
	if(hintDelegates[hintType]==nil)then
		hintDelegates[hintType] = {};
	end
	currentKey = currentKey+1;

	hintDelegates[hintType][currentKey] = {
		OnHintActive = delegateOnHintActive,
		OnCheckHintContent = delegateOnCheckHintContent,
		OnCheckHintCache = delegateOnCheckHintCache
	};
	return currentKey;	
end

function UnregisterOnHintActiveDelegate( hintType, keyToRemove)	
	if(hintDelegates~=nil and hintDelegates[hintType]~=nil)then
		hintDelegates[hintType][keyToRemove] = nil;
	end	
end

function CheckHintStatus(typeToCheck , directShowHintFlag)
	if(hintDelegates[typeToCheck]~=nil)then
		for k,v in pairs(hintDelegates[typeToCheck]) do
			if(v.OnHintActive~=nil)then
				v.OnHintActive(nil, typeToCheck, directShowHintFlag);
			end
		end
	end
end

--==================== detect relative data changes  ====================
--called on req msgs
function HandleMsgCache(cache)
	for k,v in pairs(hintDelegates) do
		--find related hint type
		for kk,vv in pairs(v) do
			if(vv.OnCheckHintCache~=nil)then
				vv.OnCheckHintCache(nil,k,cache);
			end
		end		
	end
end

--called on req msgs
function HandleMsgContent(data, msgid)
	-- k is type, v is { key - {OnHintActive,OnCheckHintContent,OnCheckHintCache} }
	for k,v in pairs(hintDelegates) do
		--find related hint type
		if(UIHintSettings.HintSettings[k]~=nil )then
			for kk,vv in pairs(UIHintSettings.HintSettings[k].RelatedMsgID) do
				print(vv);
				if(vv == msgid)then
					for kkk,vvv in pairs(v) do
						if(vvv.OnCheckHintContent~=nil)then
							--first parameter for calling the function by . operation
							vvv.OnCheckHintContent(nil, k, data, msgid);
						end
					end
					break;
				end				
			end
		end
	end
end
--==================== detect relative data changes  ====================


--==================== detect relative data Msgs  ====================
function OnInit()
    DataSystemScript.RegisterMsgHandler(MsgID.tb.GetFuncTips, OnReqFuncTips);
end

function OnRelease()
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.GetFuncTips, OnReqFuncTips);
end

function RequestToGetFuncTips()
	DataSystemScript.RequestWithParams(LuaConst.Const.GetFuncTips, nil, MsgID.tb.GetFuncTips, true);
end

function OnReqFuncTips(code_, data_)
	--hint manager is detecting the low level msging logic 
	--no need to inform from here any more
    currentFuncTipsData = data_;
    
    -- inform all registered delegates who has key in current msg
    -- if ((not IsTableEmpty(hintDelegates))and(not IsTableEmpty(currentFuncTipsData))) then        
    --     for k,v in pairs(hintDelegates) do
    --     	-- k is type, v is { key - {OnHintActive, OnCheckHintContent, OnCheckHintCache} }
    --     	if( MsgDataContainsKey(UIHintSettings.HintSettings[k].HintDataKey) )then
    --     		local showHint = tonumber(currentFuncTipsData[UIHintSettings.HintSettings[k].HintDataKey]) > 0;
    --     		for kk,vv in pairs(v) do
    --     			if(vv~=nil and vv.OnHintActive~=nil)then
    --     				vv.OnHintActive(k,showHint);
    --     			end        			
    --     		end
    --     	end
    --     end
    -- end
end

function MsgDataContainsKey(hintDataKey)
	return ( hintDataKey~=nil and currentFuncTipsData[hintDataKey]~=nil );
end

function MsgDataContainValidKey(hintDataKey)
	if(currentFuncTipsData~=nil)then
		return ( hintDataKey~=nil and currentFuncTipsData[hintDataKey]~=nil and tonumber(currentFuncTipsData[hintDataKey])>0);
	end
	return false;	
end
--==================== detect relative data Msgs  ====================