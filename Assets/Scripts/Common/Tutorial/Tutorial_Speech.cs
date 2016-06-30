using UnityEngine;

[System.Serializable]
public class Tutorial_Speech
{
    public GameObject root;
    public UILabel speech;
    public UISprite bg;

    public Vector3 localPosition
    {
        get { return root.transform.localPosition; }
        set { root.transform.localPosition = value; }
    }

    public void Hide(bool tf)
    {
        root.SetActive(!tf);
    }

    public void SetSpeech(string txt)
    {
        speech.text = txt;
    }

    public void SetLabelSize(int width, int height)
    {
        bg.width = width;
        bg.height = height;
    }
}

[System.Serializable]
public class Tutorial_NPCSpeech : Tutorial_Speech
{
    public UITexture npcImg;
    public GameObject arrow;

    public void SetNpcImg(string img)
    {
        npcImg.mainTexture = Common.ResourceManager.Instance.LoadTexture("Textures/ScatteredImg/NPC/" + img);
        npcImg.MakePixelPerfect();
    }
}