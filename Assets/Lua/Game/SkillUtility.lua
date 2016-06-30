module("SkillUtility", package.seeall)

local SkillColor = {
        ["1"]=Color.New(186/255,186/255,186/255,1),
        ["2"]=Color.New(112/255,193/255,108/255,1),
        ["3"]=Color.New(53/255,139/255,183/255,1),
        ["4"]=Color.New(147/255,98/255,184/255,1),
        ["5"]=Color.New(196/255,141/255,22/255,1),
        ["6"]=Color.New(235/255,66/255,62/255,1),
    }

function GetSkillColor(idx)
    return SkillColor[tostring(idx)];
end