
module("MsgIDExtend", package.seeall)

MsgIDTable= {};

local baseValue = 10;


function OnInit()
	DeclareMsg('GetRole');
	DeclareMsg('GetEmail');
end


function DeclareMsg(k)

	if MsgIDTable[k] ~= nil then
		error('msgid repeat!!!!!!!');
		return MsgIDTable[k];
	else
		MsgIDTable[k] = baseValue;
		baseValue = baseValue + 1;
	end
end






