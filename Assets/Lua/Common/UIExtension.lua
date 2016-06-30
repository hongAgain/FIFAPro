function SetLabelText(uiLabel,strDesc)
    if nil == uiLabel then
        error("Lable 节点不存在")
        return
    end

    if nil == strDesc then
        error("描述字符不存在")
        return
    end
    local _kTextComponent = uiLabel:GetComponent("Text")
    if nil == _kTextComponent then
        error("不包含Text组件")
        return
    end
    _kTextComponent.Text = strDesc
end

function FindChild(uiParent,strNodeName)
    return AuxiliaryFuncs.Instance:FindChild(uiParent,strNodeName)
end

function AttachUI(uiNode,strPrefab)
    return AuxiliaryFuncs.Instance:AttachUI(uiNode,strPrefab)	
end

function AttachUIAnsy(uiNode,strPrefab,callbackFunc)
 --   return AuxiliaryFuncs.Instance:AttachUI(uiNode,strPrefab)
end

function RegisterClickFunc(uiBtn,kFunc)
    AuxiliaryFuncs.Instance:RegisterClickEvent(uiBtn,kFunc)
end

function RegisterPressDownFunc(uiBtn,kFunc)
    AuxiliaryFuncs.Instance:RegisterPressedDownEvent(uiBtn,kFunc)
end
function RegisterPressUpFunc(uiBtn,kFunc)
    AuxiliaryFuncs.Instance:RegisterPressedUpEvent(uiBtn,kFunc)
end