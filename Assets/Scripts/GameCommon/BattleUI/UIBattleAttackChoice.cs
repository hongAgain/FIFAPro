using UnityEngine;
using System.Collections;
using Common;
using System.Collections.Generic;

/// <summary>
/// 战术UI配置
/// </summary>
public class UIBattleAttackChoice : MonoBehaviour
{

    // Use this for initialization
    public enum CurrentAttckControl
    {
        NULL = 0,
        Choice,
        Type,
        Diretion,

    }

    void Start()
    {
        UIEventListener.Get(m_AttackChoicLabel.gameObject).onClick = AttackChoiceClick;
        UIEventListener.Get(m_AttackDirLabel.gameObject).onClick = AttackDirClick;
        UIEventListener.Get(m_AttackTypeLabel.gameObject).onClick = AttackTypeClick;
        m_labelList.Add(m_AttackChoicLabel);
        m_labelList.Add(m_AttackTypeLabel);
        m_labelList.Add(m_AttackDirLabel);
    }
    public void Show()
    {
        m_kCurrentControl = CurrentAttckControl.NULL;
        m_AttackDetailPanel.SetActive(true);
        for (int i = 0; i < m_AttackDetailList.Count; i++)
        {
            m_AttackDetailList[i].SetActive(false);
        }
        m_AttackChoic = LLDirector.Instance.Scene.RedTeam.AttackChoice;
        m_AttackType = LLDirector.Instance.Scene.RedTeam.AttackType;
        m_AttackDir = LLDirector.Instance.Scene.RedTeam.AttackDir;
        //写入默认的战术///
        m_AttackChoicLabel.text = Localization.Get((m_kAttackChoiName + "_" + (int)m_AttackChoic));
        m_AttackDirLabel.text = Localization.Get((m_kAttackDirName + "_" + (int)m_AttackDir));
        m_AttackTypeLabel.text = Localization.Get((m_kAttackTypeName + "_" + (int)m_AttackType));
        m_lists.Clear();
        m_lists.Add(m_AttackChoiceList);
        m_lists.Add(m_AttackTypeList);
        m_lists.Add(m_AttackDirectionList);
    }

    private void AttackChoiceClick(GameObject go)
    {
        ShowDetail(CurrentAttckControl.Choice, (int)m_AttackChoic - 1);
    }
    private void AttackDirClick(GameObject go)
    {
        ShowDetail(CurrentAttckControl.Diretion, (int)m_AttackDir - 1);
    }
    private void AttackTypeClick(GameObject go)
    {
        ShowDetail(CurrentAttckControl.Type, (int)m_AttackType - 1);
    }

    public void ShowDetail(CurrentAttckControl _enum, int _index)
    {
        if (m_kCurrentControl == _enum)
        {
            m_AttackDetailList[(int)m_kCurrentControl - 1].SetActive(false);
            m_kCurrentControl = CurrentAttckControl.NULL;
            return;
        }
        if (m_kCurrentControl != CurrentAttckControl.NULL)
        {
            m_AttackDetailList[(int)m_kCurrentControl - 1].SetActive(false);
        }
        m_kCurrentControl = _enum;
        m_AttackDetailList[(int)m_kCurrentControl - 1].SetActive(true);
        m_AttackDetailList[(int)m_kCurrentControl - 1].GetComponent<TweenAlpha>().ResetToBeginning();
        m_AttackDetailList[(int)m_kCurrentControl - 1].GetComponent<TweenAlpha>().PlayForward();
        m_AttackDetailList[(int)m_kCurrentControl - 1].GetComponent<TweenPosition>().ResetToBeginning();
        m_AttackDetailList[(int)m_kCurrentControl - 1].GetComponent<TweenPosition>().PlayForward();
        List<UIBattleAttackItem> _list = m_lists[(int)_enum - 1];
        for (int i = 0; i < _list.Count; i++)
        {
            if (i == _index)
            {
                if (i == _list.Count - 1)
                {
                    _list[i].ShowItem(true, true, this, i);
                }
                else
                {
                    _list[i].ShowItem(true, false, this, i);
                }
            }
            else
            {
                if (i == _list.Count - 1)
                {
                    _list[i].ShowItem(false, true, this, i);
                }
                else
                {
                    _list[i].ShowItem(false, false, this, i);
                }
            }
        }
    }
    public void Select(int _index, string _name)
    {
        m_labelList[(int)m_kCurrentControl - 1].text = _name;
        switch (m_kCurrentControl)
        {
            case CurrentAttckControl.Type:
                if (m_AttackType == (AttackType)_index)
                    return;
                m_AttackType = (AttackType)_index;
                break;
            case CurrentAttckControl.Choice:
                if (m_AttackChoic == (AttackChoice)_index)
                    return;
                m_AttackChoic = (AttackChoice)_index;
                break;
            case CurrentAttckControl.Diretion:
                if (m_AttackDir == (AttackDirection)_index)
                    return;
                m_AttackDir = (AttackDirection)_index;
                break;
        }
        m_AttackDetailList[(int)m_kCurrentControl - 1].SetActive(false);
        m_kCurrentControl = CurrentAttckControl.NULL;
        LLDirector.Instance.Scene.RedTeam.UpdateAttackTactical(m_AttackType, m_AttackChoic, m_AttackDir);
    }

    public List<UIBattleAttackItem> m_AttackTypeList = new List<UIBattleAttackItem>();
    public List<UIBattleAttackItem> m_AttackDirectionList = new List<UIBattleAttackItem>();
    public List<UIBattleAttackItem> m_AttackChoiceList = new List<UIBattleAttackItem>();

    private List<List<UIBattleAttackItem>> m_lists = new List<List<UIBattleAttackItem>>();
    public UILabel m_AttackChoicLabel = null;
    public UILabel m_AttackDirLabel = null;
    public UILabel m_AttackTypeLabel = null;
    private List<UILabel> m_labelList = new List<UILabel>();
    public AttackChoice m_AttackChoic = AttackChoice.Long_Short_Combine;
    public AttackDirection m_AttackDir = AttackDirection.Side_Middle;
    public AttackType m_AttackType = AttackType.All_Attack;
    public CurrentAttckControl m_kCurrentControl = CurrentAttckControl.NULL;

    public GameObject m_AttackDetailPanel = null;
    public List<GameObject> m_AttackDetailList = new List<GameObject>();
    public static string m_kAttackTypeName = "UIBattle_AttackType";
    public static string m_kAttackDirName = "UIBattle_AttackDirection";
    public static string m_kAttackChoiName = "UIBattle_AttackChoice";
}
