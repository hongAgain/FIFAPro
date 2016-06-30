using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(DebugTool_NGUIWidgetChecker))]
public class DebugTool_SpriteAtlasCheckerInspector : Editor {

	public override void OnInspectorGUI ()
	{
		DebugTool_NGUIWidgetChecker captureDatas = target as DebugTool_NGUIWidgetChecker;
		
		if(captureDatas != null)
		{
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Check Sprites in Children"))
			{
				captureDatas.CheckSpritesInChildren();
			}
			if (GUILayout.Button("Check Labels in Children"))
			{
				captureDatas.CheckLabelsInChildren();
			}
			GUILayout.EndHorizontal();
		}
		this.DrawDefaultInspector();
	}
}
