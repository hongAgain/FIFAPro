require ("ktJson")
require "lfs"
require "IOHelp"

pathes = {};

local jsonMsgUrl = "C:/Develop/FIFA/config/msg";
local jsonConfigUrl = "C:/Develop/FIFA/config/ConfigDefine";
local GenluaConfigUrl = "C:/Develop/FIFA/Client/Assets/ConfigDefine";
local GenluaMsgUrl = "C:/Develop/FIFA/Client/Assets/Msg";

function GenLuaMsg(savePath, filter)
	for key, path in pairs(pathes) do
	    local jsonData = io.open(path, "r"):read();
	    local msgtb = json.decode(jsonData);
	    local output = ReadTable(msgtb, stripextension(strippath(path)));
	    writeFile(savePath .. "/"..stripextension(strippath(path))..tostring(filter), output);
	end
end


function ReadTable(tb, tbname)

	local tableString = "";
	local temp = "";
	local tableFuncString = "";
	for k ,v in pairs(tb) do
		if type(v) == "table" then
			temp = "\n\t" .. GetTableValue(k,v)..',';
		end
		tableString = tableString .. temp;
		tableFuncString  = tableFuncString .. GetTableValueFunction(tbname,k);
	end

	local tbPrefix = "local " .. tbname .. " = \n{";
	local tbEndPrefix = "\n};";
	local tbstring = tbPrefix .. tableString .. tbEndPrefix;
	tbstring = tbstring .. "\n" .. tableFuncString;
	print(tbstring);
	return tbstring;
end

function GetTableValue(key, v)
	local temp = "";
	if (v['type']) == "string" then
		if v['val'] == nil or string.len(v['val']) == 0 then
			temp = tostring(key) .." = ".. "''";
		else
			temp = tostring(key) .." = ".. v['val'];
		end
	elseif (v['type']) == "number" then
		if v['val'] == nil then
			temp = tostring(key) .. " = " .. tostring(0.0);
		else
			temp = tostring(key) .. " = " .. tostring(v['val']);
		end
	elseif (v['type']) == "int" then
		if v['val'] == nil then
			temp = tostring(key) .. " = " .. tostring(0);
		else
			temp = tostring(key) .. " = " .. tostring(v['val']);
		end
	end
	return temp;
end

function GetTableValueFunction(tbname, key)
	local funString = "local function ".." Get"..tostring(key).."() \n";
	funString = funString .. "	".."return ".. tbname .. "['"..tostring(key).."'];" .. "\n";
	funString = funString .. "end\n";
	return funString;
end

getpathes(jsonMsgUrl, pathes);
GenLuaMsg(GenluaMsgUrl, ".lua");