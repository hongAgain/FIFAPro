module("UILadderMatchQualifierHint", package.seeall);

require "Common/UnityCommonScript"

local hintPrefabName = "LadderMatchQualifierHint";
local hintBox = nil;

local delegateOnHintOver = nil;

local uiStartTitle = nil;
local uiQualifyCondition = nil;

local uiResultTitle = nil;
local uiResult1 = nil;
local uiResult2 = nil;
local uiResult3 = nil;
local uiResult = nil;

local strLocalizeQualifier = "Qualifier";
local strLocalizeRelegation = "Relegation";

local strQualify = "Qualify";
local strRelegate = "Relegate";

local strQualifyCondition = "QualifierCondition";

local strLocalizeStart = "start";
local strLocalizeOver = "over"
local strLocalizeSuccess = "success";
local strLocalizeFail = "fail";

local strLocalizeWon = "Win";
local strLocalizeTie = "Tie";
local strLocalizeLoss = "Loss";

local colorGreenLabel = Color.New(159/255,255/255,159/255,255/255);
local colorRedLabel = Color.New(255/255,102/255,102/255,255/255);
local colorWhiteLabel = Color.New(255/255,255/255,255/255,255/255);

local lastTimerId = nil;

--state : 1 qualifier start
--state : 2 qualifier won
--state : 3 qualifier lose
--state : 4 relegation start
--state : 5 relegation won
--state : 6 relegation lose
--result: 1 for lose , 3 for win ,0 for none
function CreateQualifierHint( containerTransform, windowComponent, hintstate, result1, result2, result3, delegate )
	if(hintBox == nil) then
		--instantiate one
		local hintBoxPrefab = windowComponent:GetPrefab(hintPrefabName);
		--instantiate prefab and initialize it
		hintBox = GameObjectInstantiate(hintBoxPrefab);
		hintBox.transform.parent = containerTransform;
    	hintBox.transform.localPosition = Vector3.zero;
    	hintBox.transform.localScale = Vector3.one;

    	uiStartTitle = TransformFindChild(hintBox.transform,"AnimationNode/StartTitle");
    	uiQualifyCondition = TransformFindChild(uiStartTitle,"Condition");
    	uiResultTitle = TransformFindChild(hintBox.transform,"AnimationNode/ResultTitle");
    	uiResult1 = TransformFindChild(uiResultTitle,"Result1");
    	uiResult2 = TransformFindChild(uiResultTitle,"Result2");
    	uiResult3 = TransformFindChild(uiResultTitle,"Result3");
    	uiResult = TransformFindChild(uiResultTitle,"Result");
    else
    	print("hintBox.transform is null");
	end

	if(not GameObjectActiveSelf(hintBox)) then
    	GameObjectSetActive(hintBox.transform,true);
    end

	delegateOnHintOver = delegate;

	local titlePrefix = nil;
	local titleSufix = nil;
	if(hintstate == 1) then
		GameObjectSetActive(uiStartTitle,true);
		GameObjectSetActive(uiResultTitle,false);
		local titleContent = GetLocalizedString(strLocalizeQualifier)..GetLocalizedString(strLocalizeStart);
		UIHelper.SetLabelTxt(uiStartTitle,titleContent);
		UIHelper.SetLabelTxt(uiQualifyCondition,GetLocalizedString(strQualifyCondition)..GetLocalizedString(strQualify));
	elseif(hintstate == 2) then
		GameObjectSetActive(uiStartTitle,false);
		GameObjectSetActive(uiResultTitle,true);
		local titleContent = GetLocalizedString(strLocalizeQualifier)..GetLocalizedString(strLocalizeOver);
		UIHelper.SetLabelTxt(uiResultTitle,titleContent);
		SetResults(result1, result2, result3, true, true);
	elseif(hintstate == 3) then
		GameObjectSetActive(uiStartTitle,false);
		GameObjectSetActive(uiResultTitle,true);
		local titleContent = GetLocalizedString(strLocalizeQualifier)..GetLocalizedString(strLocalizeOver);
		UIHelper.SetLabelTxt(uiResultTitle,titleContent);
		SetResults(result1, result2, result3, true, false);
	elseif(hintstate == 4) then
		GameObjectSetActive(uiStartTitle,true);
		GameObjectSetActive(uiResultTitle,false);
		local titleContent = GetLocalizedString(strLocalizeRelegation)..GetLocalizedString(strLocalizeStart);
		UIHelper.SetLabelTxt(uiStartTitle,titleContent);
		UIHelper.SetLabelTxt(uiQualifyCondition,GetLocalizedString(strQualifyCondition)..GetLocalizedString(strRelegate));
	elseif(hintstate == 5) then
		GameObjectSetActive(uiStartTitle,false);
		GameObjectSetActive(uiResultTitle,true);
		local titleContent = GetLocalizedString(strLocalizeRelegation)..GetLocalizedString(strLocalizeOver);
		UIHelper.SetLabelTxt(uiResultTitle,titleContent);
		SetResults(result1, result2, result3, false, true);
	elseif(hintstate == 6) then
		GameObjectSetActive(uiStartTitle,false);
		GameObjectSetActive(uiResultTitle,true);
		local titleContent = GetLocalizedString(strLocalizeRelegation)..GetLocalizedString(strLocalizeOver);
		UIHelper.SetLabelTxt(uiResultTitle,titleContent);
		SetResults(result1, result2, result3, false, false);
	end

	--temp functions to make this over
	if(lastTimerId~=nil) then
		LuaTimer.RmvTimer(lastTimerId);
	end
	lastTimerId = LuaTimer.AddTimer(false,3,OnHintAnimeOver);

end

function SetResults( result1, result2, result3, isQualify, isSuccess )
	SetMatchResult(uiResult1,result1);
	SetMatchResult(uiResult2,result2);
	SetMatchResult(uiResult3,result3);
	-- if(result1 > 0 and result2 > 0 and result3 > 0 )then
		GameObjectSetActive(uiResult,true);
		if(isSuccess)then
			if(isQualify)then
				UIHelper.SetLabelTxt(uiResult,GetLocalizedString(strQualify)..GetLocalizedString(strLocalizeSuccess).."!");
			else
				UIHelper.SetLabelTxt(uiResult,GetLocalizedString(strRelegate)..GetLocalizedString(strLocalizeSuccess).."!");
			end
		else
			if(isQualify)then
				UIHelper.SetLabelTxt(uiResult,GetLocalizedString(strQualify)..GetLocalizedString(strLocalizeFail));
			else
				UIHelper.SetLabelTxt(uiResult,GetLocalizedString(strRelegate)..GetLocalizedString(strLocalizeFail));
			end
		end
	-- else
		-- GameObjectSetActive(uiResult,false);
	-- end
end

function SetMatchResult( uilabel, matchState )
	if(matchState == nil)then
		GameObjectSetActive(uilabel,false);
	else
		GameObjectSetActive(uilabel,true);		
		if(matchState == 1) then
			--you won
			UIHelper.SetLabelTxt(uilabel,GetLocalizedString(strLocalizeWon));
			UIHelper.SetWidgetColor(uilabel,colorGreenLabel);				
		elseif(matchState == -1) then
			--you lose
			UIHelper.SetLabelTxt(uilabel,GetLocalizedString(strLocalizeLoss));
			UIHelper.SetWidgetColor(uilabel,colorRedLabel);
		elseif(matchState == 0) then
			--tie
			UIHelper.SetLabelTxt(uilabel,GetLocalizedString(strLocalizeTie));
			UIHelper.SetWidgetColor(uilabel,colorWhiteLabel);
		end
	end
end

function OnHintAnimeOver()
	-- body

	if(GameObjectActiveSelf(hintBox)) then
    	GameObjectSetActive(hintBox.transform,false);
    end

	if(delegateOnHintOver~=nil) then
		delegateOnHintOver();
	end
end

function OnDestroy()
	-- body
	hintBox = nil;
	delegateOnHintOver = nil;
	uiStartTitle = nil;
	uiQualifyCondition = nil;
	uiResultTitle = nil;
	uiResult1 = nil;
	uiResult2 = nil;
	uiResult3 = nil;
	uiResult = nil;
	lastTimerId = nil;
end