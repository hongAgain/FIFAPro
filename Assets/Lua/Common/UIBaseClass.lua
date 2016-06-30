UIBaseClass = class("UIBaseClass")

UIBaseClass.m_uiRoot = nil

function UIBaseClass:Show(uiFileName)
    self.m_uiRoot = AuxiliaryFuncs.Instance:attachUI(GameMain.Instance.UIAttach,uiFileName)	
    self:Init()
end

function UIBaseClass:Hide()
    if nil ~= self.m_uiRoot then
        GameObject.Destroy(self.m_uiRoot)
    end

    self.m_uiRoot = nil
end

--Ðéº¯Êý
function UIBaseClass:Init()
end