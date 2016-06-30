using Common;
using Common.Log;
using LuaInterface;
using UnityEngine;

[RequireComponent(typeof (UIPanel))]
public class Tutorial : MonoBehaviour
{
    private UIPanel mPanel;
    private Tutorial_FocusObj mCurFocus;

    public GameObject blockInput;
    public UITexture transparent;
    public UISprite arrow;
    public UISprite frame;
    public UISprite finger;
    public GameObject hand;
    public GameObject dragFXRoot;
    public UISprite dragLine;
    public GameObject dragHand;
    public Transform offset;
    public Tutorial_NPCSpeech talk1 = new Tutorial_NPCSpeech();
    public Tutorial_NPCSpeech talk2 = new Tutorial_NPCSpeech();
    public Tutorial_Speech talk3 = new Tutorial_Speech();

    private const int STANDANDW = 1136;
    private const int STANDANDH = 640;
    private const float ASPECT = 1.5f; // 960 X 640
    private float curAspect = 0;

    private static Tutorial mInstance;

    public static Tutorial Instance()
    {
        if (mInstance == null)
        {
            var prefab = ResourceManager.Instance.Load("Tutorial/", "TutorialMgr", ResType.ResType_GameObject) as GameObject;
            mInstance = NGUITools.AddChild(WindowMgr.UIParent.gameObject, prefab).GetComponent<Tutorial>();
            mInstance.mPanel = mInstance.gameObject.GetComponent<UIPanel>();

            mInstance.curAspect = 1f * Screen.width / Screen.height;
        }
        return mInstance;
    }

    public enum TalkType
    {
        Hide,
        NPC1,
        NPC2,
        NO_NPC,
    }

    private void OnDestroy()
    {
        if (mInstance == this)
        {
            mInstance = null;
        }
    }

    public void FocusAt(GameObject go, bool force)
    {
        RevertFocus();
        mCurFocus = go.AddComponent<Tutorial_FocusObj>();
        mCurFocus.Init(mPanel, force);
        transparent.gameObject.SetActive(force);
        blockInput.SetActive(false);
        transparent.centerType = UIBasicSprite.AdvancedType.Sliced;
    }

    public void WatchAnotherDragDrop(GameObject go, LuaFunction onDragStart, LuaFunction onDragEnd, LuaFunction onDrop)
    {
        var focus = go.AddComponent<Tutorial_FocusObj>();
        focus.Init(mPanel, true);
        focus.WatchDragDrop(onDragStart, onDragEnd, onDrop);

        dragFXRoot.SetActive(true);

        Vector3 v1 = mCurFocus.transform.localPosition;
        Vector3 v2 = go.transform.localPosition;

        var x = v1.x - v2.x;
        var y = v1.y - v2.y;
        var z = Mathf.Sqrt(x * x + y * y);
        dragLine.width = (int)z;
        dragLine.transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Acos(x / z) * Mathf.Rad2Deg);
        dragLine.transform.localPosition = (v1 + v2) / 2;

        TweenPosition tp = TweenPosition.Begin(dragHand, 2f, v2 + new Vector3(16, -56));
        tp.from = v1 + new Vector3(16, -56);
        tp.style = UITweener.Style.Loop;

        TweenAlpha ta = TweenAlpha.Begin(dragHand, 2f, 0f);
        ta.from = 1f;
        ta.style = UITweener.Style.Loop;
    }

    public void FocusFullScreen()
    {
        var fullScreenFocus = transparent.GetComponent<Tutorial_FocusObj>();
        if (mCurFocus != fullScreenFocus)
        {
            RevertFocus();
        }
        if (fullScreenFocus == null)
        {
            fullScreenFocus = transparent.gameObject.AddComponent<Tutorial_FocusObj>();
        }
        mCurFocus = fullScreenFocus;
        mCurFocus.Init(mPanel, false);
        transparent.gameObject.SetActive(true);
        blockInput.SetActive(false);
        transparent.centerType = UIBasicSprite.AdvancedType.Sliced;
        frame.gameObject.SetActive(false);
    }

    public void ActiveArrow(bool active)
    {
        arrow.gameObject.SetActive(active);
    }

    public void SetArrow(Vector2 pos, bool horizonal, float angle)
    {
        arrow.transform.localPosition = ToWorldPos(pos);
        arrow.flip = horizonal ? UIBasicSprite.Flip.Horizontally : UIBasicSprite.Flip.Nothing;
        arrow.transform.localEulerAngles = new Vector3(0, 0, -angle);
    }

    public void SetFinger(Vector2 pos, bool horizonal, bool vertical, float angle)
    {
        finger.transform.localPosition = pos + new Vector2(16, 2);
        if (horizonal && vertical)
        {
            finger.flip = UIBasicSprite.Flip.Both;
        }
        else if (horizonal)
        {
            finger.flip = UIBasicSprite.Flip.Horizontally;
        }
        else if (vertical)
        {
            finger.flip = UIBasicSprite.Flip.Vertically;
        }
        else
        {
            finger.flip = UIBasicSprite.Flip.Nothing;
        }
        finger.transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    public void ReleaseFocus(GameObject go)
    {
        var focus = go.GetComponent<Tutorial_FocusObj>();
        if (focus != null)
        {
            focus.Release();
        }
    }

    public void RevertFocus()
    {
        if (mCurFocus != null)
        {
            mCurFocus.Release();
            mCurFocus = null;
        }
    }

    public void Highlight(Vector3 configPos, int width, int height)
    {
        Vector3 center = ToWorldPos(configPos);
        frame.gameObject.SetActive(true);
        frame.transform.localPosition = center;
        frame.width = width + 50;
        frame.height = height + 50;
        hand.transform.localPosition = center;

        Vector4 highlight = Vector4.zero;
        highlight.x = configPos.x - width / 2;
        highlight.z = STANDANDW - configPos.x - width / 2;
        highlight.w = configPos.y - height / 2;
        highlight.y = STANDANDH - configPos.y - height / 2;

        transparent.border = highlight;
        transparent.centerType = UIBasicSprite.AdvancedType.Invisible;
    }

    public void HandleAlignment(int alignx, int aligny)
    {
        float width = STANDANDW;
        float height = STANDANDH;
        if (curAspect <= ASPECT)
        {
            width = 960;
            height = 960 / curAspect;
        }
        else
        {
            width = curAspect * 640;
            height = 640;
        }
        Vector4 highlight = transparent.border;
        Vector3 _offset = Vector3.zero;
        switch (alignx)
        {
            case 5:
                highlight.x += (width - STANDANDW) / 2;
                highlight.z += (width - STANDANDW) / 2;
                break;
            case 4:
                highlight.x += width - STANDANDW;
                _offset.x = -(STANDANDW - width) / 2;
                break;
            case 3:
                highlight.z += width - STANDANDW;
                _offset.x = (STANDANDW - width) / 2;
                break;
        }

        switch (aligny)
        {
            case 5:
                highlight.w += (height - STANDANDH) / 2;
                highlight.y += (height - STANDANDH) / 2;
                break;
            case 2:
                highlight.w += (height - STANDANDH);
                _offset.y = -(height - STANDANDH) / 2;
                break;
            case 1:
                highlight.y += (height - STANDANDH);
                _offset.y = (height - STANDANDH) / 2;
                break;
        }
        transparent.border = highlight;
        offset.localPosition = _offset;
    }

    public void WatchClick(LuaFunction onClick, bool cover)
    {
        if (mCurFocus != null)
        {
            mCurFocus.WatchClick(onClick, cover);
        }
    }

    public void WatchDragDrop(LuaFunction onDragStart, LuaFunction onDragEnd, LuaFunction onDrop)
    {
        if (mCurFocus != null)
        {
            mCurFocus.WatchDragDrop(onDragStart, onDragEnd, onDrop);
        }
    }

    public void Hide(bool keepFullScreenBlock)
    {
        if (mCurFocus != null)
        {
            mCurFocus.Release();
            mCurFocus = null;
        }

        frame.gameObject.SetActive(false);
        hand.SetActive(false);
        arrow.gameObject.SetActive(false);
        transparent.gameObject.SetActive(false);
        talk1.Hide(true);
        talk2.Hide(true);
        talk3.Hide(true);
        blockInput.SetActive(keepFullScreenBlock);
    }

    public void ShowTalk(int nTalkType, string npcImg, string txt, Vector4 v4)
    {
        var pos = new Vector3(v4.x, v4.y);
        switch ((TalkType)nTalkType)
        {
            case TalkType.NPC1:
                {
                    talk1.Hide(false);
                    talk2.Hide(true);
                    talk3.Hide(true);
                    talk1.SetNpcImg(npcImg);
                    talk1.SetSpeech(txt);
                }
                break;
            case TalkType.NPC2:
                {
                    talk1.Hide(true);
                    talk2.Hide(false);
                    talk3.Hide(true);
                    talk2.SetNpcImg(npcImg);
                    talk2.SetSpeech(txt);
                    talk2.localPosition = ToWorldPos(pos);
                }
                break;
            case TalkType.NO_NPC:
                {
                    talk1.Hide(true);
                    talk2.Hide(true);
                    talk3.Hide(false);
                    talk3.SetSpeech(txt);
                    talk3.localPosition = ToWorldPos(pos);
                    talk3.SetLabelSize((int)v4.z, (int)v4.w);
                }
                break;
            default:
                LogManager.Instance.YellowLog("Tutorial.ShowTalk ErrorType {0}", nTalkType);
                break;
        }
    }

    private Vector3 ToWorldPos(Vector3 configPos)
    {
        Vector3 pos1136_640 = Vector3.zero;
        pos1136_640.x = (configPos.x - STANDANDW / 2);
        pos1136_640.y = (STANDANDH / 2 - configPos.y);

        return pos1136_640;
    }
}