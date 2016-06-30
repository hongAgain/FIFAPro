
require "Common/CommonScript"


-- ÊÂ¼þ×¢²á±í  shaojin.han add --
function OnValueUpdate1(key, value)
	print("value callback1 key: " .. tostring(key) .. " value: " ..tostring(value));
end

function OnValueUpdate2(key, value)
	print("value callback2 key: " .. tostring(key) .. " value: " ..tostring(value));
end


function OnValueUpdate3(key, value)
	print("value callback3 key: " .. tostring(key) .. " value: " ..tostring(value));
end



local evet = {};
--CommonScript.RegisterCallback(evet, 'name', OnValueUpdate);
CommonScript.RegisterCallback(evet, 'id', OnValueUpdate2);
CommonScript.RegisterCallback(evet, 'id', OnValueUpdate3);
CommonScript.RegisterCallback(evet, 'id', OnValueUpdate3);
CommonScript.UnregisterCallback(evet, 'id', OnValueUpdate3);

local data = {name=1, id=2,sex=3};
local trackData = CommonScript.Listener(data, evet);
--trackData['name'] = "shokin";
--trackData['id'] = 2;

local evet2 ={}
--local data2 = {w = 1, x = 3, z={a= 1,b=2}};
local data2 = {w = 1, x = 3};
data2.__mode = 'kv'

--CommonScript.RegisterCallback(evet2, 'w', OnValueUpdate);
--CommonScript.RegisterCallback(evet2, 'x', OnValueUpdate2);
--CommonScript.RegisterCallback(evet2, 'z', OnValueUpdate3);
CommonScript.RegisterCallback(evet2, 'w', OnValueUpdate3);
CommonScript.RegisterCallback(evet2, 't', OnValueUpdate3);

local trackData2 = CommonScript.Listener(data2, evet2);
trackData2.t = 4;
trackData2 = nil;

--local trackData3 = CommonScript.Listener(data2.z, evet2);

--trackData2.w = "555";
--trackData2.x = "666";
--trackData2.z = ;
--trackData3.a= 12;
--print(trackData3.a);
--trackData2 = nil;
--data2.z = nil;


collectgarbage ();
--print(trackData2);

--print(trackData2.z.a);
--trackData3 = nil;
--data2.z.a = 3;

local baseClass=CommonScript.Class()
baseClass.data = {a = 1, b = 3, c = 4};

function baseClass:ctor(x)
	self.x=x
end
 
function baseClass:printX()
end
 
function baseClass:TryPrint()
	print("baseClass TryPrint")
end


test= CommonScript.Class(baseClass)
 
function test:ctor()
	print("test ctor")
end

function test:TryPrint()
	print("test TryPrint")
end

baseInstance = baseClass.new(1)
print(baseInstance.data);
baseInstance:printX();
baseInstance:TryPrint();

drivedInstance = test:new();
drivedInstance:TryPrint();


drivedInstance2 = test:new();
drivedInstance2:TryPrint();

drivedInstance2= nil;

drivedInstance = test:new();
drivedInstance:TryPrint();
