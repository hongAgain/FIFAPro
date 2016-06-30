module("UITeamLegendDrag", package.seeall)

local children = {};
local a = 200;
local b = 300;

function PosScaleInEclipse(angle)
    local rad = math.rad(angle);
    local k = math.tan(rad);
    local x = a * b / math.sqrt(b * b + a * a * k * k);
    local y = k * x;
    
    local scale = Vector3.Lerp(Vector3.New(0.5, 0.5, 0.5), Vector3.one, math.cos(rad));
    return Vector3.New(x, y, 0), scale;
end

function TestInit()
    local parent = GameObject.Find("Container");
    local prefab = GameObject.Find("3");
    local angle = 180 / 8;
    
    for i = -90, 90, angle do
        local newPos, scale = PosScaleInEclipse(i);
        
        local clone = GameObject.Instantiate(prefab);
        clone.name = tostring(i);
        clone.transform.parent = parent.transform;
        clone.transform.localScale = scale;
        clone.transform.localPosition = newPos;
        
        table.insert(children, clone);
    end
    
    GameObject.Destroy(prefab);
end

TestInit();

function OnInit()
    
end

function OnRelease()
    
end

function onClick(go)
    print("onClick "..go.name);
end

function onDragStart(go)
    --print("onDragStart "..go.name);
end

function onDrag(go, delta)
    --print("onDrag "..go.name);
    --print(delta.x.." "..delta.y);
    
    local add = delta.y / b * 90;
    Drag(add);
end

function Drag(add)
    for i = 1, #children do
        local pos = children[i].transform.localPosition;
        local angle = math.deg(math.atan(pos.y / pos.x));
        angle = angle + add;
        
        local newPos, scale = PosScaleInEclipse(angle);
        
        children[i].transform.localScale = scale;
        children[i].transform.localPosition = newPos;
    end
end

function onDragEnd(go)
    --print("onDragEnd "..go.name);
    local centerIdx = 0;
    local minY = 99999;
    for i = 1, #children do
        local pos = children[i].transform.localPosition;
        if (math.abs(pos.y) < minY) then
            centerIdx = i;
            minY = math.abs(pos.y);
        end
    end
    
    local centerPos = children[centerIdx].transform.localPosition;
    local minus = math.deg(math.atan(centerPos.y / centerPos.x));
    Drag(-minus);
end