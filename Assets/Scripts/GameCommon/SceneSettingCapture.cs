using UnityEngine;
using System.Collections;

[System.Serializable]
public class LightMapDataRecord
{
	public Texture2D m_Far;
	public Texture2D m_Near;
	
	public LightmapData GetData()
	{
		LightmapData data = new LightmapData();
		data.lightmapFar = this.m_Far;
		data.lightmapNear = this.m_Near;
		return data;
	}
}

[System.Serializable]
public class LightmapSettingRecord
{
	public LightMapDataRecord[] lightMapDatas;
	public LightmapsMode			m_Mode;
	public void UseSetting()
	{
		if(lightMapDatas != null && lightMapDatas.Length > 0)
		{
			LightmapData[] array = new LightmapData[lightMapDatas.Length];
			for(int i = 0; i < lightMapDatas.Length; ++i)
			{
				array[i] = lightMapDatas[i].GetData();
			}

			//Debuger.LogError("LightmapSettings.lightmapsMode: "+LightmapSettings.lightmapsMode);
			LightmapSettings.lightmapsMode = m_Mode;
			LightmapSettings.lightmaps  = array;
		}
	}
	
	public static LightmapSettingRecord  GetLightmapSettingRecord()
	{
		if(LightmapSettings.lightmaps == null) return null;
	
		LightmapSettingRecord data = new LightmapSettingRecord();
		data.lightMapDatas = new LightMapDataRecord[LightmapSettings.lightmaps.Length];
		for(int i = 0; i < LightmapSettings.lightmaps.Length; ++i)
		{
			LightMapDataRecord mapData = new LightMapDataRecord();
			mapData.m_Far = LightmapSettings.lightmaps[i].lightmapFar;
			mapData.m_Near = LightmapSettings.lightmaps[i].lightmapNear;
			data.lightMapDatas[i] = mapData;
		}
		
		return data;
	}
}

[System.Serializable]
public class FogDataRecord {
	public bool fog = false;
	public FogMode fogMode = FogMode.Linear;
	public Color fogColor = Color.gray;
	public float fogDensity = 0;
	public float fogStartDistance = 0;
	public float fogEndDistance = 300;	
	
	public void UseSetting()
	{
		RenderSettings.fog = this.fog;
		RenderSettings.fogMode = this.fogMode;
		RenderSettings.fogColor = this.fogColor;
		RenderSettings.fogDensity = this.fogDensity;
		RenderSettings.fogStartDistance = this.fogStartDistance;
		RenderSettings.fogEndDistance = this.fogEndDistance;
	}
	
	public static FogDataRecord GetFogRecord()
	{
		FogDataRecord data = new FogDataRecord();
		data.fog = RenderSettings.fog;
		data.fogMode = RenderSettings.fogMode;
		data.fogColor = RenderSettings.fogColor;
		data.fogDensity = RenderSettings.fogDensity;
		data.fogStartDistance = RenderSettings.fogStartDistance;
		data.fogEndDistance = RenderSettings.fogEndDistance;
		
		return data;
	}
}

[System.Serializable]
public class SkyBoxRecord
{
    public Color ambientLight;
    public Material skyBox;

	public void UseSetting()
	{
	    RenderSettings.ambientLight = ambientLight;
	    RenderSettings.skybox = skyBox;
	}

    public static SkyBoxRecord Capture()
    {
        SkyBoxRecord record = new SkyBoxRecord();
        record.ambientLight = RenderSettings.ambientLight;
        record.skyBox = RenderSettings.skybox;

        return record;
    }
}

[System.Serializable]
public class OneSceneSetting
{
	public LightmapSettingRecord	lightMapCapture;
	public SkyBoxRecord				skyCapture;
	public FogDataRecord			fogCapture;
}

public class SceneSettingCapture : MonoBehaviour
{
	public int defaultUseIndex = 0;
	public OneSceneSetting sceneData;

    //private bool bSettingEnd = false;
	
	// Use this for initialization
	void OnEnable () 
	{
        UseSceneData();
	}
	
	public void UseSceneData()
	{
		UseLightMapData(sceneData);
		UseSkyTexture(sceneData);
		UseFogSetting(sceneData);
	}
	
	void UseLightMapData(OneSceneSetting sceneData)
	{
		if (sceneData.lightMapCapture != null )
		{
			sceneData.lightMapCapture.UseSetting();
		}
	}
	
	public void UseSkyTexture(OneSceneSetting sceneData)
	{
		if (sceneData.skyCapture != null)
		{
			sceneData.skyCapture.UseSetting();
		}
	}
	
	public void UseFogSetting(OneSceneSetting sceneData)
	{
		if (sceneData.fogCapture != null)
		{
			sceneData.fogCapture.UseSetting();
		}
	}
}
