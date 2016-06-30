module("RainScreenEffect",package.seeall);

local effectSettings = {
	RegionTL = NewVector3(-530,200,0),
	RegionBR = NewVector3(280,-100,0),

	rainDropPrefabName = "UIScreenRainDrop",
	rainDropPrefab = nil,
	rainDropNumMax = 7,
	rainDropNumMin = 4,
	rainDropTimeRangeMax = 15,
	rainDropTimeRangeMin = 7
}

local uiContainer = nil;
local rainDrops = {};
local rainDropNum = nil;
local rainDropRegion = {};

function CreateRainScreenEffect( uicontainer, RegionTL, RegionBR )

	uiContainer = uicontainer;
	effectSettings.RegionTL = RegionTL;
	effectSettings.RegionBR = RegionBR;
	-- get prefab
	if(effectSettings.rainDropPrefab==nil)then
		effectSettings.rainDropPrefab = Util.GetGameObject(effectSettings.rainDropPrefabName);
	end
	--destroy old
	for k,v in pairs(rainDrops) do
		if(v~=nil and v.timerID ~= nil)then
			LuaTimer.RmvTimer(v.timerID);
		end
	end
	DestroyUIListItemGameObjects(rainDrops);

	rainDropNum = math.random(effectSettings.rainDropNumMin,effectSettings.rainDropNumMax);
	rainDropRegion = {};
	for i=1,rainDropNum do
		rainDropRegion[i] = {};
		rainDropRegion[i].maxX = RegionTL.x+(RegionBR.x-RegionTL.x)/rainDropNum*i;
		rainDropRegion[i].minX = RegionTL.x+(RegionBR.x-RegionTL.x)/rainDropNum*(i-1);
		rainDropRegion[i].maxY = RegionTL.y;
		rainDropRegion[i].minY = RegionBR.y;
	end
	--create new
	CreateUIListItemGameObjects(uiContainer, rainDropRegion, effectSettings.rainDropPrefab, OnInitItem);
end

function OnInitItem(randomIndex, key, value, cloneGameObject)
	--bind ui
	rainDrops[randomIndex] = {};
	rainDrops[randomIndex].gameObject = cloneGameObject;
	rainDrops[randomIndex].transform = cloneGameObject.transform;
	rainDrops[randomIndex].transform.name = string.format("%03d",tonumber(key));

	GameObjectSetActive(rainDrops[randomIndex].transform,false);

	PrepareForNextEffect(randomIndex);
end

function PrepareForNextEffect(index)
	--rmv old
	if(rainDrops[index].timerID~=nil)then
		LuaTimer.RmvTimer(rainDrops[index].timerID);
	end
	--add new
	rainDrops[index].timerID = LuaTimer.AddTimer(
		false,
		math.random(effectSettings.rainDropTimeRangeMin,effectSettings.rainDropTimeRangeMax),
		function ()
			PlayRainDropEffect(index);
		end
	);
end

function PlayRainDropEffect(index)
	--play it
	GameObjectSetActive(rainDrops[index].transform,true);
	UIHelper.PlayUIRainDropScreenAnime(rainDrops[index].transform,GenerateRandomPos(index));
	--prepare for next
	PrepareForNextEffect(index);
end

function GenerateRandomPos(index)
	return NewVector3(
			math.random(rainDropRegion[index].minX,rainDropRegion[index].maxX),
			math.random(rainDropRegion[index].minY,rainDropRegion[index].maxY),
			0);
end

function OnDestroy()
	for k,v in pairs(rainDrops) do
		if(v.timerID ~= nil)then
			LuaTimer.RmvTimer(v.timerID);
		end
	end
	uiContainer = nil;
	rainDrops = {};
	rainDropNum = nil;
	rainDropRegion = {};
end