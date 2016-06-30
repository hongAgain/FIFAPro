using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleListFactorData
{
	public int maxDistrictToEffect = 8;

	int fontSizeDefault = 22;
	List<int> itemFontSizes = new List<int>{30, 22, 22, 22, 22, 22, 22, 22, 22};

	Color colorDefault = new Color(125/255f,128/255f,137/255f,255/255f);
	List<Color> itemColors = new List<Color>
	{
		new Color(102/255f,204/255f,255/255f,255/255f),
		new Color(171/255f,173/255f,185/255f,255/255f),
		new Color(171/255f,173/255f,185/255f,255/255f),
		new Color(171/255f,173/255f,185/255f,255/255f),
		new Color(125/255f,128/255f,137/255f,255/255f),
		new Color(125/255f,128/255f,137/255f,255/255f),
		new Color(125/255f,128/255f,137/255f,255/255f),
		new Color(125/255f,128/255f,137/255f,0/255f),
		new Color(125/255f,128/255f,137/255f,0/255f)
	};
	
	Vector3 scaleDefault = new Vector3 (1f,1f,1f);
	List<Vector3> itemScales = new List<Vector3>
	{
		new Vector3(1f,1f,1f),
		new Vector3(1f,1f,1f),
		new Vector3(1f,0.8f,1f),
		new Vector3(1f,0.6f,1f),
		new Vector3(1f,0.4f,1f),
		new Vector3(1f,0.2f,1f),
		new Vector3(1f,0.1f,1f),
		new Vector3(1f,0.1f,1f),
		new Vector3(1f,1f,1f)
	};
	Vector3 offsetDefault = new Vector3 (0,0,0);
	List<Vector3> itemOffset = new List<Vector3>
	{ 
		new Vector3(0,0,0),
		new Vector3(0,0,0),
		new Vector3(0,3f,0),
		new Vector3(0,14f,0),
		new Vector3(0,31f,0),
		new Vector3(0,56f,0),
		new Vector3(0,88f,0),
		new Vector3(0,126f,0),
		new Vector3(0,0,0)
	};

	public bool IsTargetEffected(float district)
	{
		return district <= maxDistrictToEffect && district >= -maxDistrictToEffect;
	}

	public float AdjustPercent(float percentage)
	{
		if(percentage>0.95f)
			return 1f;
		else if(percentage < 0.05)
			return 0f;
		return percentage;
	}

	Vector3 GetIndexedOffset(int index)
	{
		if(index > maxDistrictToEffect || index < -maxDistrictToEffect)
			return offsetDefault;
		if(index>=0)
			return itemOffset[index];
		else
			return -itemOffset[-index];
	}

	Vector3 GetIndexedScale(int index)
	{
		if(index > maxDistrictToEffect || index < -maxDistrictToEffect)
			return scaleDefault;
		return itemScales[Mathf.Abs(index)];
	}

	Color GetIndexedColor(int index)
	{
		if(index > maxDistrictToEffect || index < -maxDistrictToEffect)
			return colorDefault;
		return itemColors[Mathf.Abs(index)];
	}

	int GetIndexedFontSize(int index)
	{
		if(index > maxDistrictToEffect || index < -maxDistrictToEffect)
			return fontSizeDefault;
		return itemFontSizes[Mathf.Abs(index)];
	}

	public Vector3 GetDefaultOffset()
	{
		return offsetDefault;
	}
	
	public Vector3 GetDefaultScale()
	{
		return scaleDefault;
	}
	
	public Color GetDefaultColor()
	{
		return colorDefault;
	}
	
	public int GetDefaultFontSize()
	{
		return fontSizeDefault;
	}

	public Vector3 GetLerpedOffset(int minIndex,int maxIndex, float percentage)
	{
		Vector3 minPos = GetIndexedOffset(minIndex);
		Vector3 maxPos = GetIndexedOffset(maxIndex);
		return minPos+(maxPos-minPos)*percentage;
	}

	public Vector3 GetLerpedScale(int minIndex,int maxIndex, float percentage)
	{
		Vector3 minScale = GetIndexedScale(minIndex);
		Vector3 maxScale = GetIndexedScale(maxIndex);
		return minScale+(maxScale-minScale)*percentage;
	}

	public Color GetLerpedColor(int minIndex,int maxIndex, float percentage)
	{
		Color minColor = GetIndexedColor(minIndex);
		Color maxColor = GetIndexedColor(maxIndex);
		return Color.Lerp(minColor, maxColor, percentage);
	}

	public int GetLerpedFontSize(int minIndex,int maxIndex, float percentage)
	{
		int minSize = GetIndexedFontSize(minIndex);
		int maxSize = GetIndexedFontSize(maxIndex);
		return (int)(minSize+(maxSize-minSize)*percentage);
	}
}

public class UICircleListItem : MonoBehaviour {

	public Transform transformNode = null;
	public UILabel nameLabel = null;

	private bool initialized = false;

	private CircleListFactorData data = new CircleListFactorData ();
	private UIScrollView uiScrollView = null;
	private UIGrid uiContainer = null;

	private Vector3 targetCenterPos = new Vector3 ();
	private float itemInterval = 38f;
	private float district = 0f;

	private enum status{Default,Lerping,AtPoint};
	private status currentStatus = status.Default;
	
	private int minIndex = 0;
	private int maxIndex = 0;

	public void Initialize()
	{
		uiContainer = transform.parent.GetComponent<UIGrid>();
		uiScrollView = transform.parent.parent.GetComponent<UIScrollView>();
		targetCenterPos = new Vector3(	uiScrollView.panel.cachedTransform.localPosition.x + uiScrollView.panel.finalClipRegion.x,
		                              	uiScrollView.panel.cachedTransform.localPosition.y + uiScrollView.panel.finalClipRegion.y,
							            0);
		itemInterval = uiContainer.cellHeight;
		initialized = true;
	}

	private void MakeCircleEffect()
	{
		district = (targetCenterPos.y - (uiScrollView.transform.localPosition + uiContainer.transform.localPosition + transform.localPosition).y)/itemInterval;

		if(data.IsTargetEffected(district))
		{
			minIndex = Mathf.FloorToInt(district);
			maxIndex = Mathf.CeilToInt(district);
			if(minIndex != maxIndex)
			{
				//core factor
				float percentage = data.AdjustPercent((district-minIndex)/(maxIndex-minIndex));
				transformNode.localPosition = data.GetLerpedOffset(minIndex,maxIndex,percentage);
				transformNode.localScale = data.GetLerpedScale(minIndex,maxIndex,percentage);
				nameLabel.color = data.GetLerpedColor(minIndex,maxIndex,percentage);
				nameLabel.fontSize = data.GetLerpedFontSize(minIndex,maxIndex,percentage);
				nameLabel.MarkAsChanged();
				currentStatus = status.Lerping;
			}
			else if(currentStatus!=status.AtPoint)
			{
				//core factor
				float percentage = 0f;
				transformNode.localPosition = data.GetLerpedOffset(minIndex,maxIndex,percentage);
				transformNode.localScale = data.GetLerpedScale(minIndex,maxIndex,percentage);
				nameLabel.color = data.GetLerpedColor(minIndex,maxIndex,percentage);
				nameLabel.fontSize = data.GetLerpedFontSize(minIndex,maxIndex,percentage);
				nameLabel.MarkAsChanged();
				currentStatus = status.AtPoint;
			}
		}
		else if(currentStatus!=status.Default)
		{
			//set it to default
			transformNode.localPosition = data.GetDefaultOffset();
			transformNode.localScale = data.GetDefaultScale();
			nameLabel.color = data.GetDefaultColor();
			nameLabel.fontSize = data.GetDefaultFontSize();
			nameLabel.MarkAsChanged();
			currentStatus=status.Default;
		}
	}

	void Update () 
	{
		if(initialized)
			MakeCircleEffect();
	}
}
