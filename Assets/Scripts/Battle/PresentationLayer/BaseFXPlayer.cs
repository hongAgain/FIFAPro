using Common;
using Common.Log;
using Common.Tables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SFxItem
{
    public PLPlayer Player = null;
    public float ElapseTime = 0.0f;
    public bool Moveable = false; // 是否可以移动
    public BaseFxItem Item = null;
    public GameObject FxObj = null;
    public bool NeedRemove = false;
    public string SkillName;
}

public class BaseFxPlayer : MonoBehaviour
{

    public static BaseFxPlayer Instance;
    public void Awake()
    {
        Instance = this;
    }

    public void BeginBaseFx(PLPlayer kPlayer,int iBaseFxID, string strSkillName)
    {
        BaseFxItem kItem = TableManager.Instance.BaseFxTbl.GetItem(iBaseFxID);
        if (null == kItem)
            return;
        SFxItem kFxItem = new SFxItem();
        kFxItem.Player = kPlayer;
        kFxItem.ElapseTime = 0;
        kFxItem.Item = kItem;
        kFxItem.Moveable = true;
        kFxItem.FxObj = null;
        kFxItem.SkillName = strSkillName;
        int iIdx = 0;
        if(EFxType.Right_UI == kItem.FxType )
        {
            for (int i = m_kFxItemList.Count - 1; i >= 0; i--)
            {
                m_kFxItemList[i].Moveable = true;
                if (EFxType.Right_UI == m_kFxItemList[i].Item.FxType)
                {
                    iIdx++;
                    if (iIdx >= 4)
                        kFxItem.NeedRemove = true;
                }
            }
        }
        
        m_kFxItemList.Add(kFxItem);
    }

    public void EndBaseFx(PLPlayer kPlayer,int iBaseFxID)
    {
        for (int i = m_kFxItemList.Count - 1; i >= 0; i--)
        {
            SFxItem kItem = m_kFxItemList[i];
            if (kItem.Item.ID == iBaseFxID && kItem.Player == kPlayer)
            {
                GameObject.Destroy(kItem.FxObj);
                kItem.FxObj = null;
                m_kFxItemList.Remove(kItem);
            }
        }
    }

    private void UpdateBottomUIObject(SFxItem kItem)
    {
        kItem.ElapseTime += Time.deltaTime;
        if (null == kItem.FxObj)
        {
            if (kItem.ElapseTime > kItem.Item.DelayTime)
            {
                if(null != UIBattle.Instance && null == kItem.FxObj)
                {
                    kItem.FxObj = UIBattle.Instance.SkillRoot.FindChild("BottomUISkill").gameObject;
                    if(kItem.FxObj!=null)
                    {
                        kItem.FxObj.SetActive(true);
                        InitSkillBottomUI(kItem);
                    }
                    else
                    {
                        LogManager.Instance.RedLog("BaseFXPlayer===>UpdateBottomUIObject is not find the BottonUISkill");
                    }
                    
                }
            }
        }
        else
        {
            if (false == kItem.Item.Loop)
            {
                if (kItem.Item.LiveTime < kItem.ElapseTime - kItem.Item.DelayTime)
                {
                    kItem.FxObj.SetActive(false);
                    kItem.FxObj = null;
                    m_kFxItemList.Remove(kItem);
                }
            }
        }
    }

    private void UpdateRightUIObject(SFxItem kItem)
    {
        //#if !GAME_AI_ONLY
        if (false == UIBattle.Instance)
            return;
        if (null == m_kRightUIObj)
        {
            Transform kTs = UIBattle.Instance.SkillRoot.FindChild("RightUISkill");
            if (null == kTs)
                return;
            m_kRightUIObj = kTs.gameObject;
        }

        if (null == m_kRightUIObj)
            return;
            
        kItem.ElapseTime += Time.deltaTime;
        if (null == kItem.FxObj)
        {
            if (kItem.ElapseTime > kItem.Item.DelayTime)
            {

                kItem.FxObj = UnityEngine.GameObject.Instantiate(m_kRightUIObj) as GameObject;
                kItem.FxObj.transform.parent = m_kRightUIObj.transform.parent;
                kItem.FxObj.transform.localPosition = m_kRightUIObj.transform.localPosition;
                kItem.FxObj.transform.localScale = m_kRightUIObj.transform.localScale;
                kItem.FxObj.transform.localRotation = m_kRightUIObj.transform.localRotation;
                TweenAlpha kTweenAlpha = kItem.FxObj.GetComponentInChildren<TweenAlpha>();
                kTweenAlpha.enabled = true;
                kItem.Moveable = false;
                kItem.FxObj.SetActive(true);
                InitSkillRightUI(kItem);
            }
        }
        else
        {
            if (kItem.NeedRemove)
            {
                GameObject.Destroy(kItem.FxObj);
                kItem.FxObj = null;
                m_kFxItemList.Remove(kItem);
            }
            else
            {
                if (false == kItem.Item.Loop)
                {
                    if (kItem.Item.LiveTime < kItem.ElapseTime - kItem.Item.DelayTime)
                    {
                        GameObject.Destroy(kItem.FxObj);
                        kItem.FxObj = null;
                        m_kFxItemList.Remove(kItem);
                    }
                    else
                    {
                        if (true == kItem.Moveable)
                        {
                            TweenPosition kTweenPos = kItem.FxObj.GetComponentInChildren<TweenPosition>();
                            if (null != kTweenPos && false == kTweenPos.enabled)
                            {
                                Transform kTransformObj = kItem.FxObj.transform.FindChild("BG");
                                UISprite kUISprit = kTransformObj.GetComponent<UISprite>();
                                kTweenPos.from = kTweenPos.to;
                                kTweenPos.to.y += kUISprit.height;
                                // TweenPosition.Begin(kItem.FxObj, (float)kItem.Item.VerticalOffsetTime, new Vector3(kTweenPos.value.x,kUISprit.height, 0 ));
                                StartCoroutine(DelayStartUISkill(kTweenPos));
                                kItem.Moveable = false;
                            }
                        }
                    }
                }
            }
        }
        //#endif
    }
    private IEnumerator DelayStartUISkill(TweenPosition kObj)
    {
        yield return new WaitForEndOfFrame();
        if (null == kObj)
            yield break;
        kObj.enabled = true;
        kObj.PlayForward();
        yield return null;
    }
    private void InitSkillRightUI(SFxItem kItem)
    {
        if (null == kItem || null == kItem.FxObj 
            || null == kItem.Player 
            || null == kItem.Player.Player 
            || null == kItem.Player.Player.Team)
            return;
        
        TweenPosition kTweenPos = kItem.FxObj.GetComponentInChildren<TweenPosition>();
        Transform kTransformObj = kItem.FxObj.transform.FindChild("BG");
        StartCoroutine(DelayStartUISkill(kTweenPos));

        switch (kItem.Player.Player.Team.TeamColor)
        {
            case ETeamColor.Team_Red:
                UIHelper.SetSpriteName(kTransformObj, "War_Skill_B_Bg");
                break;
            case ETeamColor.Team_Blue:
                UIHelper.SetSpriteName(kTransformObj, "War_Skill_O_Bg");
                break;
        }

        Transform kHeadTransform = kTransformObj.FindChild("Head");
        if (null != kHeadTransform)
        {
            HeroItem kHeroItem = TableManager.Instance.HeroTbl.GetItem((int)kItem.Player.Player.PlayerBaseInfo.HeroID);
            UITexture kTexture = kHeadTransform.GetComponent<UITexture>();
            kTexture.mainTexture = ResourceManager.Instance.Load(string.Format("Textures/ScatteredImg/PlayerHeadIcon/{0}", kHeroItem.HeadID)) as Texture2D;
        }
        Transform kDescTransform = kTransformObj.FindChild("Desc");
        if (null != kDescTransform)
            UIHelper.SetLabelTxt(kDescTransform, string.Format("{0}", kItem.SkillName));

        Transform kLVTransform = kTransformObj.FindChild("LV");
        if (null != kLVTransform)
            UIHelper.SetLabelTxt(kLVTransform, string.Format("LV.{0}", kItem.Player.Player.PlayerBaseInfo.Attri.lv));
    }

    private void InitSkillBottomUI(SFxItem kItem)
    {
        if (null == kItem || null == kItem.FxObj 
            || null == kItem.Player
            || null == kItem.Player.Player
            || null == kItem.Player.Player.Team)
            return;

        //初始progressbar
        
        Transform kTransform = kItem.FxObj.transform.FindChild("BG/LeftProgress");
        SetBottomBGColor(kTransform, kItem.Player.Player.Team.TeamColor);
        kTransform = kItem.FxObj.transform.FindChild("BG/RightProgress");
        SetBottomBGColor(kTransform, kItem.Player.Player.Team.TeamColor);

        Transform kHeadTransform = kItem.FxObj.transform.FindChild("Head");
        if (null != kHeadTransform)
        {
            HeroItem kHeroItem = TableManager.Instance.HeroTbl.GetItem((int)kItem.Player.Player.PlayerBaseInfo.HeroID);
            UITexture kTexture = kHeadTransform.GetComponent<UITexture>();
            kTexture.mainTexture = ResourceManager.Instance.Load(string.Format("Textures/ScatteredImg/PlayerHeadIcon/{0}", kHeroItem.HeadID)) as Texture2D;
        }
        Transform kDescTransform = kItem.FxObj.transform.FindChild("SkillName");
        if (null != kDescTransform)
            UIHelper.SetLabelTxt(kDescTransform, string.Format("{0}", kItem.SkillName));

        Transform kLVTransform = kItem.FxObj.transform.FindChild("LV");
        if (null != kLVTransform)
            UIHelper.SetLabelTxt(kLVTransform, string.Format("LV.{0}", kItem.Player.Player.PlayerBaseInfo.Attri.lv));
        Animation kAnimation = kItem.FxObj.GetComponent<Animation>();
        kAnimation.Play("Skill_in");
    }

    private void SetBottomBGColor(Transform kTransform,ETeamColor kTeamColor)
    {
        Transform kTs= kTransform.FindChild("foreground");
        if (null == kTs)
            return;
        Color kColor = new Color(32.0f / 255, 155.0f / 255, 1, 0.8f);  
        switch (kTeamColor)
        {
            case ETeamColor.Team_Red:
                kColor = new Color(32.0f / 255, 155.0f / 255, 1, 0.8f);
                break;
            case ETeamColor.Team_Blue:
                kColor = new Color(1.0f, 169.0f / 255, 0, 0.8f);
                break;
        }
        UIHelper.SetWidgetColor(kTs, kColor);
    }

    private void UpdateFxObject(SFxItem kItem)
    {
        kItem.ElapseTime += Time.deltaTime;
        if (null == kItem.FxObj)
        {
            if (kItem.ElapseTime > kItem.Item.DelayTime)
            {
                kItem.FxObj = ResourceManager.Instance.Load(kItem.Item.Name, true) as GameObject;
                if (null != kItem.FxObj)
                {
                    kItem.FxObj.transform.parent = kItem.Player.AniObj.transform.FindChild("All_001");
                    kItem.FxObj.transform.localPosition = MathUtil.ConverToVector3(kItem.Item.Pos);
                    kItem.FxObj.SetActive(true);
                }
            }
        }
        else
        {
            if (false == kItem.Item.Loop)
            {
                if (kItem.Item.LiveTime < kItem.ElapseTime - kItem.Item.DelayTime)
                {
                    GameObject.Destroy(kItem.FxObj);
                    kItem.FxObj = null;
                    m_kFxItemList.Remove(kItem);
                }
            }
        }
    }

    public void Update()
    {
        for (int i = m_kFxItemList.Count - 1; i >= 0; i--)
        {
            SFxItem kItem = m_kFxItemList[i];
            if (null == kItem || null == kItem.Item || null == kItem.Player)
                continue;
            if (EFxType.Right_UI == kItem.Item.FxType)
            {
                UpdateRightUIObject(kItem);
            }
            else if(EFxType.Bottom_UI == kItem.Item.FxType)
            {
                UpdateBottomUIObject(kItem);
            }
            else if(EFxType.Fx == kItem.Item.FxType)
            {
                UpdateFxObject(kItem);
            }
        }
    }

    private List<SFxItem> m_kFxItemList = new List<SFxItem>();
    private GameObject m_kRightUIObj = null;
}