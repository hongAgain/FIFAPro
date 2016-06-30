using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugTool_NGUIWidgetChecker : MonoBehaviour {

	public List<UISprite> spritesWithNoAtlas = new List<UISprite> ();
	public List<UILabel> labelsWithNoFont = new List<UILabel> ();

	public void CheckSpritesInChildren()
	{
		if (spritesWithNoAtlas == null)
			spritesWithNoAtlas = new List<UISprite> ();
		spritesWithNoAtlas.Clear ();
		UISprite[] spritesToCheck = gameObject.GetComponentsInChildren<UISprite>();
		foreach(UISprite s in spritesToCheck)
		{
			if(s.atlas == null)
			{
				spritesWithNoAtlas.Add(s);
			}
		}
	}
		
	public void CheckLabelsInChildren()
	{
		if (labelsWithNoFont == null)
			labelsWithNoFont = new List<UILabel> ();
		labelsWithNoFont.Clear ();
		UILabel[] spritesToCheck = gameObject.GetComponentsInChildren<UILabel>();
		foreach(UILabel l in spritesToCheck)
		{
			if(l.ambigiousFont == null)
			{
				labelsWithNoFont.Add(l);
			}
		}
	}
}
