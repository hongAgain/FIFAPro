module("UIHint",package.seeall);

require("UILua/UIHintSettings");
require("Game/HintManager");

local instanceTable = {};
-- local mt = {__mode = "k" };
local mt = {};
setmetatable(instanceTable, mt);

function InitializeTable()
	return {};
end

-- paramtable ： {type-{param,param,...}}
function Start(gameObject,instanceKey,paramtable)
	print("UIHint.Start")

	-- local hint = New();
	instanceTable[instanceKey] = New();
    instanceTable[instanceKey]:Start(gameObject,paramtable);

    if(instanceTable[instanceKey]~=nil)then
		print("UIHint.Start : "..instanceKey.."not nil")
	else		
		print("UIHint.Start : "..instanceKey.."is nil")
	end

    print(instanceKey);
end

function OnEnable(iKey)
	if(instanceTable[iKey] == nil or instanceTable[iKey] == {})then
		print("its empty")
	end
	instanceTable[iKey]:OnEnable();
end

function OnDestroy(iKey)
	print("UIHint.OnDestroy : "..iKey)
	if(instanceTable[iKey]~=nil)then
		print("UIHint.OnDestroy : "..iKey.."not nil")
	else		
		print("UIHint.OnDestroy : "..iKey.."is nil")
	end

	instanceTable[iKey]:OnDestroy(iKey);
	instanceTable[iKey] = nil;
end

function New()

	local hint = {
		window = nil,
		RootTransform = nil,
		HintDot = nil,
		hintDataTable = {},
		hintRegisterKeys = {},
	};
	
	--entrance
	function hint:Start(gameObject,paramtable)
		-- local hint
		hint.window = gameObject;
		hint:ProcessData(paramtable);
		hint:BindUI();
		hint:RegisterHintCheck();
		hint:CheckHintStatus();
	end

	function hint:OnEnable()
		hint:CheckHintStatus();
	end

	-- paramtable ： {type-{0:param,1:param,...}}
	function hint:ProcessData(paramtable)		
		-- if(paramtable == nil)then
		-- 	return
		-- end
		--transform {type-{0:param,1:param,...}} into {type-{1:param,2:param,...}}
		for k,v in pairs(paramtable) do
			hint.hintDataTable[k] = {};
			for i=#v,-1,1 do
				table.insert(hint.hintDataTable[k], v[i-1]);
			end
		end
	end

	function hint:BindUI()
		hint.RootTransform = hint.window.transform;
		hint.HintDot = TransformFindChild(hint.RootTransform,"HintDot");
	end

	function hint:RegisterHintCheck()
		-- k is type, v is param list
		for k,v in pairs(hint.hintDataTable) do
			hint.hintRegisterKeys[k] = HintManager.RegisterOnHintActiveDelegate( 
				k, 
				hint.CheckHintStatus, 
				hint.CheckMsgContent, 
				hint.CheckMsgCache);
		end
	end

	function hint:CheckHintStatus(typeToCheck , directShowHintFlag)
		if(directShowHintFlag~=nil)then
			hint:SetHintDotActive(directShowHintFlag);
			return;
		end
		if(typeToCheck~=nil)then
			if(UIHintSettings.HintSettings[typeToCheck]==nil)then
				hint:SetHintDotActive(false);
			else
				--check this type
				if(UIHintSettings.HintSettings[typeToCheck].CheckHintStatus~=nil)then
					local needShowHint = UIHintSettings.HintSettings[typeToCheck].CheckHintStatus(hint.hintDataTable[typeToCheck]);
					hint:SetHintDotActive(needShowHint);
				else
					hint:SetHintDotActive(false);
				end
			end
		else
			--check all type
			for k,v in pairs(hint.hintDataTable) do
				if(UIHintSettings.HintSettings[k]~=nil
					 and UIHintSettings.HintSettings[k].CheckHintStatus~=nil
					 and UIHintSettings.HintSettings[k].CheckHintStatus(v,UIHintSettings.HintSettings[k].HintDataKey))then
					hint:SetHintDotActive(true);
					return;
				end			
			end
			hint:SetHintDotActive(false);
		end
	end

	function hint:CheckMsgContent(typeToCheck, _data, msgID)		
		if(UIHintSettings.HintSettings[typeToCheck]==nil)then
			hint:SetHintDotActive(false);
		elseif(UIHintSettings.HintSettings[typeToCheck].CheckMsgContent~=nil)then
			local needShowHint = UIHintSettings.HintSettings[typeToCheck].CheckMsgContent(
				_data,
				msgID,
				hint.hintDataTable[typeToCheck],
				UIHintSettings.HintSettings[typeToCheck].HintDataKey);
			hint:SetHintDotActive(needShowHint);
		end
	end

	function hint:CheckMsgCache(typeToCheck, cache)
		if(UIHintSettings.HintSettings[typeToCheck]==nil)then
			hint:SetHintDotActive(false);
		elseif(UIHintSettings.HintSettings[typeToCheck].CheckMsgCache~=nil)then
			local needShowHint = UIHintSettings.HintSettings[typeToCheck].CheckMsgCache(
				cache,
				hint.hintDataTable[typeToCheck]);
			hint:SetHintDotActive(needShowHint);
		end
	end

	function hint:UnregisterHintCheck()
		for k,v in pairs(hint.hintDataTable) do
			HintManager.UnregisterOnHintActiveDelegate( k, hint.hintRegisterKeys[k]);
		end
	end

	function hint:SetHintDotActive(willActivate)
		GameObjectSetActive(hint.HintDot,willActivate);
	end

	--exit
	function hint:OnDestroy(iKey)
		print("hint:OnDestroy:"..iKey);
		hint:UnregisterHintCheck();
		hint = nil
	end

	return hint;
end
