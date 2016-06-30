using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RainDropCalculator{

	public class RainDropPeriodData{
		public float periodTime;
		public Vector3 periodFromMoveDelta;
		public Vector3 periodToMoveDelta;
		public Color periodFromColor;
		public Color periodToColor;
		public TweenMath.TweenType tweenType;
		public TweenMath.EaseType easeType;
		public RainDropPeriodData(float pTime, Vector3 pFromMoveDelta, Vector3 pToMoveDelta, Color pFromColor, Color pToColor, TweenMath.TweenType tType = TweenMath.TweenType.LINEAR, TweenMath.EaseType eType = TweenMath.EaseType.EASEIN)
		{
			periodTime = pTime;
			tweenType = tType;
			easeType = eType;
			periodFromMoveDelta = pFromMoveDelta;
			periodToMoveDelta = pToMoveDelta;
			periodFromColor = pFromColor;
			periodToColor = pToColor;
		}

		public bool PeriodFunction(Transform rainDropTransform,UIWidget rainDropWidget,Vector3 originPos,float timePassedSinceThisPeriod)
		{
			if(timePassedSinceThisPeriod < periodTime)
			{
				rainDropTransform.localPosition = new Vector3 (
					TweenMath.Linear(timePassedSinceThisPeriod,originPos.x+periodFromMoveDelta.x,periodToMoveDelta.x-periodFromMoveDelta.x,periodTime),
					TweenMath.Linear(timePassedSinceThisPeriod,originPos.y+periodFromMoveDelta.y,periodToMoveDelta.y-periodFromMoveDelta.y,periodTime),
					TweenMath.Linear(timePassedSinceThisPeriod,originPos.z+periodFromMoveDelta.z,periodToMoveDelta.z-periodFromMoveDelta.z,periodTime));

				rainDropWidget.color = Color.Lerp(periodFromColor,periodToColor,timePassedSinceThisPeriod/periodTime);
				rainDropWidget.MarkAsChanged();
				return true;
			}
			else
			{
				rainDropTransform.localPosition = originPos+periodToMoveDelta;
				rainDropWidget.color = periodToColor;
				rainDropWidget.MarkAsChanged();
				return false;
			}
		}
	}

	public int currentPeriod = -1;
	public int PeriodCount = 2;

	public List<RainDropPeriodData> periodDatas = new List<RainDropPeriodData> {
		new RainDropPeriodData(1f,		new Vector3(0,0,0),		new Vector3(0,-40,0),	new Color(1,1,1,1),	new Color(1,1,1,1)),
		new RainDropPeriodData(0.3f,	new Vector3(0,-40,0),	new Vector3(0,-60,0),	new Color(1,1,1,1),	new Color(1,1,1,0))
	};

	public bool StartFirstPeriodSuccessful()
	{
		currentPeriod = -1;
		return SwitchNextPeriodSuccessful();
	}

	public bool SwitchNextPeriodSuccessful()
	{
		currentPeriod++;
		if(currentPeriod>=PeriodCount)
		{
			currentPeriod = -1;
			return false;
		}
		return true;
	}

	public bool EffectInCurrentPeriod(Transform rainDropTransform,UIWidget rainDropWidget,Vector3 originPos,float timePassedSinceThisPeriod)
	{
		if(currentPeriod>-1)
		{
			periodDatas[currentPeriod].PeriodFunction(rainDropTransform,rainDropWidget,originPos,timePassedSinceThisPeriod);
			if(timePassedSinceThisPeriod>periodDatas[currentPeriod].periodTime)
				return false;
			return true;
		}
		return false;
	}
}

public class UIRainDropAnim : MonoBehaviour {

	public UIWidget dropWidget = null;

	private RainDropCalculator calculator = new RainDropCalculator();
	private Vector3 originPos = new Vector3 ();
	private bool IsDropping = false;
	private bool PeriodNotOver = true;
	private float timeSinceThisPeriod = 0f;
	//called by lua code
	public void StartDropping(Vector3 Origin)
	{
		originPos = Origin;
		transform.localPosition = originPos;
		if(calculator.StartFirstPeriodSuccessful())
		{
			IsDropping = true;
			PeriodNotOver = true;
			timeSinceThisPeriod = 0f;
		}
	}

	// Update is called once per frame
	void Update () {
		if(IsDropping)
		{
			IsDropping = Drop();
		}
	}

	//return false if dropping is over
	private bool Drop()
	{
		if(PeriodNotOver)
		{
			//count time
			timeSinceThisPeriod += Time.deltaTime;
			PeriodNotOver = calculator.EffectInCurrentPeriod(transform,dropWidget,originPos,timeSinceThisPeriod);
			return true;
		}
		else
		{
			//to next period
			timeSinceThisPeriod = 0f;
			PeriodNotOver = true;
			return calculator.SwitchNextPeriodSuccessful();
		}
	}
}
