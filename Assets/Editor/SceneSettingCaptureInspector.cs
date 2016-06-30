using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SceneSettingCapture))]
public class SceneSettingCaptureInspector : Editor {

	public override void OnInspectorGUI ()
	{
		this.DrawDefaultInspector();
		SceneSettingCapture captureDatas = target as SceneSettingCapture;
		
		if(captureDatas.sceneData != null)
		{
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Import"))
            {
                captureDatas.sceneData = CaptureSceneData();
            }
            if (GUILayout.Button("Use"))
            {
                captureDatas.UseSceneData();
            }
            GUILayout.EndHorizontal();
		}
		
		if(GUILayout.Button("Update New", GUILayout.Height(30)))
		{
			captureDatas.sceneData = CaptureSceneData();
		}
	}
	
	public OneSceneSetting CaptureSceneData()
	{
		OneSceneSetting newData  = new OneSceneSetting();
		newData.lightMapCapture = LightmapSettingRecord.GetLightmapSettingRecord();
		newData.lightMapCapture.m_Mode = LightmapSettings.lightmapsMode;
	    newData.skyCapture = SkyBoxRecord.Capture();
		newData.fogCapture = FogDataRecord.GetFogRecord();

		return newData;
	}
}