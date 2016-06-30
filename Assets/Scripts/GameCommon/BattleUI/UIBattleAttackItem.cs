using UnityEngine;
using System.Collections;

public class UIBattleAttackItem : MonoBehaviour {

    public UISprite m_spriteLight1 = null;
    public UISprite m_spriteLight2 = null;
    public UISprite m_spriteLine = null;
    public UILabel m_itemNameLabel = null;
    public static string m_labelNormalColor = "win_b_24";
    public static string m_labelHighColor = "win_wb_24";
    void Start ()
    {
        UIEventListener.Get(gameObject).onClick = GameClick;

    }

    void GameClick(GameObject go)
    {
        if (m_choic)
            m_choic.Select(m_index+1, m_itemNameLabel.text);
    }
    public void ShowItem(bool _choice,bool _LineShow,UIBattleAttackChoice _choiceSprite,int _index)
    {
        m_NowChoice = _choice;
        m_spriteLight1.gameObject.SetActive(_choice);
        m_spriteLight2.gameObject.SetActive(_choice);
        m_spriteLine.gameObject.SetActive(_LineShow);
        if(_choice)
        {
            LabelStandard.Refresh(m_itemNameLabel, (LabelStandard.Standard)System.Enum.Parse(typeof(LabelStandard.Standard), m_labelNormalColor));
        }
        else
        {
            LabelStandard.Refresh(m_itemNameLabel, (LabelStandard.Standard)System.Enum.Parse(typeof(LabelStandard.Standard), m_labelHighColor));

        }
        m_choic = _choiceSprite;
        m_index = _index;
    }

    public bool m_NowChoice = false;
    public UIBattleAttackChoice m_choic = null;
    public int m_index = 0;
}
