using UnityEngine;
using System.Collections.Generic;
using LuaInterface;

public class UIHelper
{
    public static void SetLabelTxt(Transform transform, string txt)
    {
        transform.GetComponent<UILabel>().text = txt;
		// todo ? 
		//NGUITools.MakePixelPerfect(transform);
    }

//	public static void SetLabelColor(Transform transform, float r, float g, float b, float a)
//	{
//		transform.GetComponent<UILabel>().color = new Color (r,g,b,a);
//	}

    public static void GrayWidget(Transform transform, bool gray)
    {
        var widget = transform.GetComponent<UIWidget>();
        if (widget != null)
        {
            widget.gray = gray;
        }
    }

	public static void SetLabelFontSize(Transform transform, float size)
	{
		transform.GetComponent<UILabel>().fontSize = (int)size;
	}

    public static void SetLabelKey(Transform tran_, string key_)
    {
        UILocalize localize = tran_.GetComponent<UILocalize>();
        if (localize != null)
            localize.key = key_; 
        else
            Util.Log("No Find UILocalize!!!");
    
    }

	public static void SetLabelOmitTail(Transform tran_,string txt)
	{
		UILabel label = tran_.GetComponent<UILabel>();
		if (label != null)
		{
			int lengthFormer = txt.Length;
			label.text = txt;
			string mytext = label.processedText;
			int lengthLatter = mytext.Length;
			if(lengthFormer > lengthLatter)
				label.text = mytext.Substring(0,Mathf.Max (0,mytext.Length-1))+"..";
		}
	}
	
	public static void EnableBtn(GameObject go, bool enabled)
    {
        var collider = go.collider;
        if (collider != null)
        {
            collider.enabled = enabled;
        }
    }

    public static void EnableButton(Transform tf, bool enabled)
    {
        var btn = tf.GetComponent<UIButton>();
        if (btn != null)
        {
            btn.isEnabled = enabled;
        }
    }
    public static string InputTxt(Transform inputTransform)
    {
        UIInput input = inputTransform.GetComponent<UIInput>();
        if (input != null)
        {
            return input.value.Trim();
        }
        else
        {
            return null;
        }
    }
    public static void SetInputText(Transform tran_,string text_)
    {
        UIInput inputTemp = tran_.GetComponent<UIInput>();
        if (inputTemp != null)
        {
            inputTemp.value = text_;
        }
    }

    public static void SetProgressBar(Transform tran_, float value_)
    {
        UIProgressBar prg = tran_.GetComponent<UIProgressBar>();
        if (prg != null)
        {
            prg.value = value_;
        }
    }
    public static float GetProgressBar(Transform _trans)
    {
        UIProgressBar pb = _trans.GetComponent<UIProgressBar>();
        if(pb!=null)
        {
            return pb.value;
        }
        return 0;
    }

    public static void InstantiateStars(Transform transform_, int count_,int lightCount_)
    {
        UIGrid m_grid = transform_.GetComponent<UIGrid>();
        GameObject m_item = transform_.FindChild("Item").gameObject;
        m_item.SetActive(true);
        GameObject objGame = null;
        for (int i = 0; i < count_; ++i)
        {
            objGame = GameObject.Instantiate(m_item) as GameObject;
            objGame.transform.parent = transform_;
            objGame.transform.localPosition = Vector3.zero;
            objGame.transform.localScale = Vector3.one;
            if (i < lightCount_)
                if (objGame.transform.FindChild("StarHighlight") != null)
                    objGame.transform.FindChild("StarHighlight").gameObject.SetActive(true);
            else
                if (objGame.transform.FindChild("StarHighlight") != null)
                    objGame.transform.FindChild("StarHighlight").gameObject.SetActive(false);
        }

        m_item.SetActive(false);

        m_grid.Reposition();
    }

    public static void InstantiateGrid(Transform tranGrid_,Transform tranItem_, int count_, int subCount_)
    {
        UIGrid m_grid = tranGrid_.GetComponent<UIGrid>();
        UIScrollView m_scrollView = tranGrid_.parent.GetComponent<UIScrollView>();
        GameObject m_item = tranItem_.gameObject;
        GameObject objGame = null;

        m_item.SetActive(true);
        for (int i = 0; i < count_; ++i)
        {
            objGame = GameObject.Instantiate(m_item) as GameObject;
            objGame.name = i.ToString();
            objGame.transform.parent = tranGrid_;
            objGame.transform.localPosition = Vector3.zero;
            objGame.transform.localScale = Vector3.one;
        }

        if (objGame != null)
        {
            for (int j = subCount_; j < objGame.transform.childCount; ++j)
            {
                objGame.transform.GetChild(j).gameObject.SetActive(false);
            }
        }

        m_item.SetActive(false);

        m_scrollView.ResetPosition();
        m_grid.Reposition();
    }

    public static void ReInstantiateGrid(Transform tranGrid_, Transform tranItem_, int count_, bool reZero_ = false)
    {
        UIGrid m_grid = tranGrid_.GetComponent<UIGrid>();
        UIScrollView m_scrollView = tranGrid_.parent.GetComponent<UIScrollView>();
        GameObject m_item = tranItem_.gameObject;
        GameObject objGame = null;
        if (reZero_)
        {
            m_scrollView.transform.localPosition = Vector3.zero;
            m_grid.transform.localPosition = Vector3.zero;
        }


        m_item.SetActive(true);
        for (int i = 0; i < count_; ++i)
        {
            objGame = GameObject.Instantiate(m_item) as GameObject;
            objGame.name = i.ToString();
            objGame.transform.parent = tranGrid_;
            objGame.transform.localPosition = Vector3.zero;
            objGame.transform.localScale = Vector3.one;
        }
        m_item.SetActive(false);

        m_scrollView.ResetPosition();
        m_grid.Reposition();
    }

	public static void RepositionGrid(Transform tranGrid_, Transform tranScrollView_)
	{
		UIGrid m_grid = tranGrid_.GetComponent<UIGrid>();
		UIScrollView m_scrollView = tranScrollView_.GetComponent<UIScrollView>();
		m_scrollView.ResetPosition();
		if(m_grid!=null)
			m_grid.Reposition();
	}

    public static void RepositionGrid(Transform tranGrid_)
    {
        UIGrid m_grid = tranGrid_.GetComponent<UIGrid>();
        m_grid.Reposition();
    }

    static public int SortByNumericNameDesc(Transform a, Transform b)
    {

        decimal d1;
        decimal d2;

        if (decimal.TryParse(a.name, out d1) && decimal.TryParse(b.name, out d2))
        {
            return d1.CompareTo(d2);
        }
        else
        {
            return string.Compare(b.name, a.name);
        }
    }
    public static void GridSortByNumericName(Transform trans)
    {
        UIGrid grid = trans.GetComponent<UIGrid>();
        grid.sorting = UIGrid.Sorting.Custom;
        grid.onCustomSort = SortByNumericNameDesc;
    }

	public static void ResetScroll(Transform tranScrollView_)
	{
		UIScrollView m_scrollView = tranScrollView_.GetComponent<UIScrollView>();
		m_scrollView.ResetPosition();
	}

    public static void ResetScrollGrid(Transform tranGrid_, Transform tranScrollView_)
    {
        UIGrid m_grid = tranGrid_.GetComponent<UIGrid>();
        UIScrollView m_scrollView = tranScrollView_.GetComponent<UIScrollView>();
        m_grid.transform.localPosition = Vector3.zero;
        m_scrollView.ResetPosition();
        m_grid.Reposition();
    }

	public static void SetGridPosition(Transform tranGrid_, Transform tranScrollView_, Vector3 panelPosition, Vector3 gridPosition, bool isVerticle)
	{
		UIGrid m_grid = tranGrid_.GetComponent<UIGrid>();
		UIPanel m_panel = tranScrollView_.GetComponent<UIPanel>();
		UIScrollView m_scrollView = tranScrollView_.GetComponent<UIScrollView>();

//		Vector3 originalPos = m_panel.transform.localPosition;
		m_scrollView.ResetPosition();
		if(isVerticle)
		{
			m_panel.transform.localPosition = new Vector3 (m_panel.transform.localPosition.x,0f,m_panel.transform.localPosition.z);
		}
		else
		{
			m_panel.transform.localPosition = new Vector3 (0f,m_panel.transform.localPosition.y,m_panel.transform.localPosition.z);
		}
		m_panel.clipOffset = Vector2.zero;
		m_panel.transform.localPosition = panelPosition;
		m_grid.Reposition();
		m_grid.transform.localPosition = gridPosition;
	}

    public static void DestroyGrid(Transform tranParent_)
    {
        int size = tranParent_.childCount;
        int currIndex = 0;
        for (int i = 0; i < size; ++i)
        {
            if (tranParent_.GetChild(currIndex).name == "Item")
            {
                ++currIndex;
                continue;
            }

            GameObject.DestroyImmediate(tranParent_.GetChild(currIndex).gameObject);
        }
    }

	public static void RefreshPanel(Transform tranPanel_)
	{
		UIPanel m_panel = tranPanel_.GetComponent<UIPanel>();
		m_panel.SetDirty();
	}

	public static void SetPanelDepth(Transform tranPanel_,int depth)
	{
		UIPanel m_panel = tranPanel_.GetComponent<UIPanel>();
		m_panel.depth = depth;
	}

	public static int GetMaxDepthOfPanelInChildren(Transform root)
	{
		UIPanel[] panels = root.GetComponentsInChildren<UIPanel>(false);
		int maxDepthUnderRoot = 1;
		if (panels != null)
		{
			foreach (UIPanel panel in panels)
			{
				if(maxDepthUnderRoot < panel.depth)
					maxDepthUnderRoot = panel.depth;
			}
		}
		return maxDepthUnderRoot;
	}

	public static int GetPanelDepth(Transform tranPanel_)
	{
		UIPanel m_panel = tranPanel_.GetComponent<UIPanel>();
		return m_panel.depth;
	}

	public static void AddDragOnStarted(Transform t, System.Object luafuc)
	{
		var scrollView = t.GetComponent<UIScrollView>();		
		if (scrollView != null)
		{
			scrollView.onDragStarted = delegate()
			{
				LuaInterface.LuaFunction func = (LuaInterface.LuaFunction)luafuc;
				func.Call (t.gameObject);
			};
		}
	}

    public static void AddDragOnFinish(Transform t, System.Object luafuc)
	{
		var scrollView = t.GetComponent<UIScrollView>();		
		if (scrollView != null)
		{
			scrollView.onDragFinished = delegate()
			{
                //				LogManager.Instance.LogWarning("this delegate only works fine without UICenterOnChild component in NGUI 3.8.2");
                LuaInterface.LuaFunction func = (LuaInterface.LuaFunction)luafuc;
				func.Call (t.gameObject);
			};
		}
    }

	public static bool IsOverDragged(Transform t,bool detectUp,bool detectDown,bool detectLeft,bool detectRight)
	{
		var panel = t.GetComponent<UIPanel>();
		var scrollView = t.GetComponent<UIScrollView>();
		if(scrollView != null && panel!=null)
		{
//			scrollView.RestrictWithinBounds(false);
			Bounds b = scrollView.bounds;
			Vector3 constraint = panel.CalculateConstrainOffset(b.min, b.max);
			
			if (!detectLeft && !detectRight) 
				constraint.x = 0f;
			if (!detectUp && !detectDown) 
				constraint.y = 0f;
			
			if(detectUp && (constraint.y>0))
				return true;
			if(detectDown && (constraint.y<0))
				return true;
			if(detectLeft && (constraint.x<0))
				return true;
			if(detectRight && (constraint.x>0))
				return true;
			return false;
		}
		return false;
	}
    public static void SpringPanelBegin(Transform tf_, Vector3 targetPos_, float strength_)
    {
        SpringPosition.Begin(tf_.gameObject, targetPos_, strength_);
    }

    public static Transform CenterOnRecenter(Transform t)
    {
        var center = t.GetComponent<UICenterOnChild>();
        center.Recenter();

        return center.centeredObject.transform;
    }

    public static void OnClickScrollView(Transform tfView_,Transform tfNext_)
    {
        UIPanel panel = tfView_.GetComponent<UIPanel>();
        UIScrollView sv = panel.GetComponent<UIScrollView>();
        Vector3 offset = -panel.cachedTransform.InverseTransformPoint(tfNext_.position);
        if (!sv.canMoveHorizontally) 
            offset.x = panel.cachedTransform.localPosition.x;
        if (!sv.canMoveVertically) 
            offset.y = panel.cachedTransform.localPosition.y;

        SpringPanel.Begin(panel.cachedGameObject, offset, 6f);
    }

	public static void OnReleaseScrollView(Transform scrollViewTrans)
	{

	}

	public static void OnClickChildToCenterOn(Transform listPanel,Transform targetChild)
	{
		UIPanel panel = listPanel.GetComponent<UIPanel>();
		UICenterOnChild container = listPanel.GetComponentInChildren<UICenterOnChild>();
		
		if(container!=null)
			container.CenterOn(targetChild);
	}

    public static void OnCenterItem(Transform centerGrid, LuaFunction luaFunc)
    {
        UICenterOnChild center = centerGrid.GetComponent<UICenterOnChild>();
        if (center != null)
        {
            center.onFinished = delegate()
            {
                luaFunc.Call(center.centeredObject);
            };
        }
    }

	public static Vector3 ClipAreaCenterLocalPos(Transform listPanel)
	{
		UIPanel panel = listPanel.GetComponent<UIPanel>();
		return new Vector3(panel.cachedTransform.localPosition.x + panel.finalClipRegion.x,
		                   panel.cachedTransform.localPosition.y + panel.finalClipRegion.y,
		                   0);
	}

    public static Vector2 GetPanelSize(Transform _panel)
    {
        UIPanel panel = _panel.GetComponent<UIPanel>();
        return panel.GetViewSize();
    }

	public static Vector2 GetUIGridCellSize(Transform containerGrid)
	{
		UIGrid grid = containerGrid.GetComponent<UIGrid>();
		return new Vector2 (grid.cellWidth,grid.cellHeight);
	}

    public static void EnableWidget(GameObject go, bool tf)
    {
        UIWidget[] widgets = go.GetComponentsInChildren<UIWidget>();
        if (widgets != null)
        {
            foreach (UIWidget widget in widgets)
            {
                widget.enabled = tf;
            }
        }
    }

    public static void EnableWidget(Transform t, bool tf)
    {
        EnableWidget(t.gameObject, tf);
    }

    public static bool IsSpriteInAtlas(Transform tf_,string name_)
    {
        UIAtlas atlasTemp = tf_.GetComponent<UISprite>().atlas;
        if (atlasTemp != null)
        {
            foreach (UISpriteData temp in atlasTemp.spriteList)
            {
                if (temp.name.Equals(name_))
                {
                    return true;
                }
            }
        } 

        return false;
    }

    public static void SetSpriteName(Transform t, string spriteName)
    {
        var sprite = t.GetComponent<UISprite>();
        sprite.spriteName = spriteName;
        sprite.MakePixelPerfect();
    }

    public static void SetSpriteNameNoPerfect(Transform t, string spriteName)
    {
        var sprite = t.GetComponent<UISprite>();
        sprite.spriteName = spriteName;
    }

//	public static void SetSpriteColor(Transform t, float r, float g, float b, float a)
//	{
//		var sprite = t.GetComponent<UISprite>();
//		sprite.color = new Color(r,g,b,a);
////		sprite.MakePixelPerfect();
//	}

    public static void SetWidgetColor(Transform t, Color color)
    {
        var sprite = t.GetComponent<UIWidget>();
        if (sprite != null)
        {
            sprite.color = color;
            sprite.MarkAsChanged();
        }
    }

    public static void SetWidgetColor(Transform t, string colorEnum)
    {
        var mode = (LabelStandard.Standard)System.Enum.Parse(typeof (LabelStandard.Standard), colorEnum);
        var label = t.GetComponent<UILabel>();
        if (label != null)
        {
            LabelStandard.Refresh(label, mode);
        }
        else
        {
            SetWidgetColor(t, LabelStandard.GetFormatColor(mode));
        }
    }

	public static void SetButtonSpriteName(Transform t, string spriteName)
	{
		var sprite = t.GetComponent<UISprite>();
        if (sprite != null)
        {
            sprite.spriteName = spriteName;
            sprite.MakePixelPerfect();
        }

		var button = t.GetComponent<UIButton>();
        if (button != null)
        {
            button.normalSprite = spriteName;
            button.hoverSprite = spriteName;
            button.pressedSprite = spriteName;
        }
	}

    public static void SetSpritePolygonRate(Transform tf_,int index_,float rate_)
    {
        tf_.gameObject.SetActive(false);
        tf_.gameObject.SetActive(true);
        UISpritePolygon sp = tf_.GetComponent<UISpritePolygon>();
        if(sp != null)
        {
            sp.mRate[index_] = rate_;
        }
    }
    public static void SetSpriteFillAmount(Transform tf_,float amount_)
    {
        UISprite sprite = tf_.GetComponent<UISprite>();
        if(sprite != null)
            sprite.fillAmount = amount_;
    }
	private static Color colorActivated = Color.white;
	private static Color colorDisactive = Color.gray;

	public static void SetButtonActive(Transform t, bool isActive, bool isImmediate)
	{
		if(isImmediate)
		{
			var boxcollider = t.GetComponent<BoxCollider>();
			boxcollider.enabled = isActive;
			var sprite = t.GetComponent<UISprite>();
			if(sprite!=null)
				sprite.color = isActive?colorActivated:colorDisactive;
		}
		else
		{
			var button = t.GetComponent<UIButton>();
			button.isEnabled = isActive;
		}
	}
    public static void SetBoxCollider(Transform tf_,bool bActive_)
    {
        BoxCollider bCollider = tf_.GetComponent<BoxCollider>();
        if (bCollider != null)
        {
            bCollider.enabled = bActive_;
        }
    }

    public static void AdjustDepth(GameObject g, int depth)
    {
        foreach (var widget in g.GetComponentsInChildren<UIWidget>(true))
        {
            widget.depth += depth;
        }
    }

    public static void SetDepth(Transform tf_,int depth_)
    {
        tf_.GetComponent<UIWidget>().depth = depth_;
    }


    public static int WidthOfWidget(Transform t)
    {
        var widget = t.GetComponent<UIWidget>();
        if (widget != null)
        {
            return widget.width;
        }
        else
        {
            return 0;
        }
    }
    public static int HeightOfWidget(Transform t)
    {
        var widget = t.GetComponent<UIWidget>();
        if (widget != null)
        {
            return widget.height;
        }
        else
        {
            return 0;
        }
    }

	public static Vector2 SizeOfWidget(Transform t)
	{
		var widget = t.GetComponent<UIWidget>();
		if (widget != null)
		{
			return new Vector2(widget.width,widget.height);
		}
		else
		{
			return Vector2.zero;
		}
	}

    public static void SetSizeOfWidget(Transform t, Vector2 size)
    {
        var widget = t.GetComponent<UIWidget>();
        if (widget != null)
        {
            widget.width = (int)size.x;
            widget.height = (int)size.y;
        }
    }

    public static Vector2 LabelSize(Transform t)
    {
        var label = t.GetComponent<UILabel>();
        if (label != null)
        {
            return label.printedSize;
        }
        else
        {
            return Vector2.zero;
        }
    }

    public static void AddToggle(Transform t, System.Object onchanged)
    {
        var toggle = t.GetComponent<UIToggle>();
        toggle.onChange.Add(new EventDelegate(delegate()
        {
            LuaInterface.LuaFunction func = (LuaInterface.LuaFunction) onchanged;
            func.Call(toggle.value, t);
        }));
    }

    public static void SetToggleState(Transform t, bool tf)
    {
        UIToggle toggle = t.GetComponent<UIToggle>();
        if (toggle != null)
        {
            toggle.value = tf;
        }
    }

    public static bool GetToggleState(Transform t)
    {
        UIToggle toggle = t.GetComponent<UIToggle>();
        if (toggle != null)
        {
            return toggle.value;
        }
        else
        {
            return false;
        }
    }

	public static void SetDragScrollViewTarget(Transform dragscrollview , Transform targetscrollview)
	{
		UIDragScrollView dsv = dragscrollview.GetComponent<UIDragScrollView>();
		UIScrollView sv = targetscrollview.GetComponent<UIScrollView>();
		dsv.scrollView = sv;
	}

    public static Vector3 CalculateWidgetBoundsSize(Transform transform, bool considerActive)
    {
        Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform, transform, considerActive);
        return bounds.size;
    }

    public static void Floating(Transform tf, Vector3 from, Vector3 to, float d)
    {
        TweenPosition tp = TweenPosition.Begin(tf.gameObject, d, to);
        tp.onFinished.Clear();
        EventDelegate.Callback del = delegate()
        {
            tf.localPosition = from;
            SetLabelTxt(tf, "");
            tf.GetComponent<UILabel>().alpha = 1f;
        };
        tp.onFinished.Add(new EventDelegate(del));
      
        TweenAlpha.Begin(tf.gameObject, d, 0f);
    }

    public static void AlphaTweening(Transform tf, float from, float to, float d)
    {
        TweenAlpha ta = TweenAlpha.Begin(tf.gameObject, d, 0f);
        ta.onFinished.Clear();
        EventDelegate.Callback del = delegate()
        {
            tf.GetComponent<UILabel>().alpha = from;
        };
        ta.onFinished.Add(new EventDelegate(del));
    }

    public static void TweenPositionBegin(Transform tf_, float duration_,Vector3 pos_)
    {
        TweenPosition.Begin(tf_.gameObject, duration_, pos_);
    }
    public static void TweenPositionPlayForward(Transform trans, bool reset = true)
    {
        TweenPosition tp = trans.GetComponent<TweenPosition>();
        if (reset)
        {
            tp.ResetToBeginning();
        }
        tp.PlayForward();
    }
    public static void TweenPositionPlayReverse(Transform trans, bool reset = true)
    {
        TweenPosition tp = trans.GetComponent<TweenPosition>();
        if (reset)
        {
            tp.ResetToBeginning();
        }
        tp.PlayReverse();
    }

    public static void SetTweenPositionOnFinish(Transform t, System.Object luafunc)
    {
        TweenPosition tp = t.GetComponent<TweenPosition>();
        if (tp != null)
        {
            EventDelegate.Add(tp.onFinished, delegate() 
            {
                LuaInterface.LuaFunction f = (LuaInterface.LuaFunction)luafunc;
                f.Call();
            });
        }
    }
    public static void SetTweenerTime(Transform t, string tweenType, float delay, float duration)
    {
        UITweener tweener = null;
        switch(tweenType)
        {
            case "TP":
                tweener = t.GetComponent<TweenPosition>();
                break;
            case "TA":
                tweener = t.GetComponent<TweenAlpha>();
                break;
            case "TS":
                tweener = t.GetComponent<TweenScale>();
                break;
        }
        if(tweener != null)
        {
            tweener.delay = delay;
            tweener.duration = duration;
        }
    }

    public static void SetCirclePos(Transform tf_,int index_)
    {
        TweenCircle circle = tf_.GetComponent<TweenCircle>();
        if (circle != null)
        {
            circle.SetCirclePos(index_);
        }
    }

    public static void OnDragEndCircle(Transform tf_, System.Object luafuc)
    {
        TweenCircle circle = tf_.GetComponent<TweenCircle>();
        if (circle != null)
        {
            circle.OnDragEndCircle = delegate(int id)
            {
                LuaInterface.LuaFunction func = (LuaInterface.LuaFunction)luafuc;
                func.Call(id);
            };
        }
    }

    public static void OnDragCircle(Transform tf_, System.Object luafuc)
    {
        TweenCircle circle = tf_.GetComponent<TweenCircle>();
        if (circle != null)
        {
            circle.OnDragCircle = delegate(GameObject go, Vector2 delta)
            {
                LuaInterface.LuaFunction func = (LuaInterface.LuaFunction)luafuc;
                func.Call(go,delta);
            };
        }
    }
    public static void OnDragStartCircle(Transform tf_, System.Object luafuc)
    {
        TweenCircle circle = tf_.GetComponent<TweenCircle>();
        if (circle != null)
        {
            circle.OnDragStartCircle = delegate(GameObject go)
            {
                LuaInterface.LuaFunction func = (LuaInterface.LuaFunction)luafuc;
                func.Call(go);
            };
        }
    }


	public static UIEventListener AddPressRepeating(GameObject g, LuaInterface.LuaFunction luafuc)
    {
        var listener = UIEventListener.Get(g);
        
        listener.onPress = delegate(GameObject go, bool press)
        {
            if (press)
            {
                listener.InvokeRepeating("OnClick", 0.3f, 0.1f);
            }
            else
            {
                listener.CancelInvoke("OnClick");
            }
        };

        listener.onClick = delegate(GameObject go)
        {
            luafuc.Call(go);
        };
		return listener;
    }

    public static UIEventListener AddDragOver(GameObject g, LuaInterface.LuaFunction luafuc)
    {
        var listener = UIEventListener.Get(g);
        
        listener.onDragOver = delegate(GameObject go)
        {
            luafuc.Call(go);
        };
        return listener;
    }

    public static void SetAutoActiveCout(Transform tf_,int count_)
    {
        AutoActive aa = tf_.GetComponent<AutoActive>();
        if (aa != null)
        {
            aa.activeCount = count_;
        }
    }

	public static void SwitchSideMenuIn(LuaInterface.LuaFunction luafuc = null)
	{
		UIHeadMenuController.SwitchMenuIn(()=>{luafuc.Call();});
	}
	
	public static void SwitchSideMenuOut(LuaInterface.LuaFunction luafuc = null)
	{
		UIHeadMenuController.SwitchMenuOut(()=>{luafuc.Call();});
	}

	public static void ScrollClippedPanelTo(Transform panelTrans,Vector3 targetPosition,float strength,LuaInterface.LuaFunction onOver = null)
	{
		SpringPanel sp = panelTrans.gameObject.GetComponent<SpringPanel>();
		if (sp == null) sp = panelTrans.gameObject.AddComponent<SpringPanel>();
		sp.target = targetPosition;
		sp.strength = strength;
		sp.onFinished = ()=>{
			onOver.Call();
		};
		sp.enabled = true;
	}

	public static void PlayUIRainDropScreenAnime(Transform rainDropTransform,Vector3 origin)
	{
		UIRainDropAnim rda = rainDropTransform.GetComponent<UIRainDropAnim>();
		if(rda!=null)
		{
			rda.StartDropping(origin);
		}
	}

	public static void FadeUIWidgetColor(Transform trans, Color from, Color to, float time, LuaInterface.LuaFunction luafunc = null)
	{
		UIWidget uiw = trans.GetComponent<UIWidget>();
		if (uiw != null) 
		{
			uiw.color = from;
			TweenColor tc = TweenColor.Begin (uiw.gameObject, time, to);		
			EventDelegate callback = new EventDelegate (() => {
				if (luafunc != null)
					luafunc.Call ();
			});
			callback.oneShot = true;
			tc.SetOnFinished (callback);
		} 
		else
		{
			if (luafunc != null)
				luafunc.Call ();
		}

	}

	public static void FadeUIWidgetColorTo(Transform trans, Color to, float time)
	{
		UIWidget uiw = trans.GetComponent<UIWidget>();
		if (uiw != null) 
		{
			TweenColor tc = TweenColor.Begin (uiw.gameObject, time, to);
		}		
	}

    public static void SetPanelAlpha(GameObject trans, float alpha)
    {
        UIPanel panel = trans.GetComponent<UIPanel>();
        if (panel != null)
        {
            panel.alpha = alpha;
        }
    }

    public static UIEventListener GetUIEventListener(GameObject go)
    {
        if (go == null)
        {
            return null;
        }
        return go.GetComponent<UIEventListener>();
    }
    public static UIEventListener GetUIEventListener(Transform trans)
    {
        if (trans == null)
        {
            return null;
        }
        return trans.GetComponent<UIEventListener>();
    }
    public static void SetScrollViewRestrict(Transform trans, bool restrict)
    {
        UIScrollView sv = trans.GetComponent<UIScrollView>();
        if(sv != null)
        {
            sv.restrictWithinPanel = restrict;
        }
    }
    public static void SetPopupListItem(Transform t, System.Object strList)
    {
        UIPopupList popupList = t.GetComponent<UIPopupList>();
        popupList.items.Clear();
        LuaInterface.LuaTable table = (LuaInterface.LuaTable)strList;
        foreach (var idx in table.Keys)
        {
            string val = table[idx].ToString();
            popupList.items.Add(val);

        }
    }
    public static void AddPopupListDelegate(Transform t, System.Object onchanged)
    {
        UIPopupList popupList = t.GetComponent<UIPopupList>();
        popupList.onChange.Add(new EventDelegate(delegate()
        {
            LuaInterface.LuaFunction func = (LuaInterface.LuaFunction)onchanged;
            func.Call(popupList.value);
        }));
    }
    public static void AddPopupListItem(Transform t, string itemVal)
    {
        UIPopupList popupList = t.GetComponent<UIPopupList>();
        if(!popupList.items.Contains(itemVal))
        {
            popupList.items.Add(itemVal);
        }
    }
}