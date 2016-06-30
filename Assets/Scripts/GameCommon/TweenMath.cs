using UnityEngine;
using System.Collections;

public class TweenMath{

	public enum TweenType{LINEAR,QUAD,CUBIC,QUART,QUINT,SINE,EXPO,CIRC,ELASTIC,BACK,BOUNCE}
	public enum EaseType{EASEIN,EASEOUT,EASEINOUT};

	public static float Lerp(float timeProgress,float beforeMove,float changeDistance,float targetTime,TweenMath.TweenType ttype,TweenMath.EaseType etype)
	{
		float currentValue = beforeMove;
		//LINEAR, QUAD, CUBIC, QUART, QUINT, SINE, EXPO, CIRC, ELASTIC, BACK, BOUNCE
		if(ttype == TweenMath.TweenType.LINEAR)
			currentValue = TweenMath.Linear(timeProgress,beforeMove,changeDistance,targetTime);
		else if(ttype == TweenMath.TweenType.QUAD)				
			currentValue = TweenMath.Quad(timeProgress,beforeMove,changeDistance,targetTime,etype);
		else if(ttype == TweenMath.TweenType.CUBIC)				
			currentValue = TweenMath.Cubic(timeProgress,beforeMove,changeDistance,targetTime,etype);
		else if(ttype == TweenMath.TweenType.QUART)				
			currentValue = TweenMath.Quart(timeProgress,beforeMove,changeDistance,targetTime,etype);
		else if(ttype == TweenMath.TweenType.QUINT)				
			currentValue = TweenMath.Quint(timeProgress,beforeMove,changeDistance,targetTime,etype);
		else if(ttype == TweenMath.TweenType.SINE)				
			currentValue = TweenMath.Sine(timeProgress,beforeMove,changeDistance,targetTime,etype);
		else if(ttype == TweenMath.TweenType.EXPO)				
			currentValue = TweenMath.Expo(timeProgress,beforeMove,changeDistance,targetTime,etype);
		else if(ttype == TweenMath.TweenType.CIRC)				
			currentValue = TweenMath.Circ(timeProgress,beforeMove,changeDistance,targetTime,etype);
		else if(ttype == TweenMath.TweenType.ELASTIC)			
			currentValue = TweenMath.Linear(timeProgress,beforeMove,changeDistance,targetTime);
		//				currentValue = TweenMath.Elastic(timeProgress,beforeMove,changeDistance,targetTime,,,etype);
		else if(ttype == TweenMath.TweenType.BACK)				
			currentValue = TweenMath.Back(timeProgress,beforeMove,changeDistance,targetTime,0,etype);
		else if(ttype == TweenMath.TweenType.BOUNCE)				
			currentValue = TweenMath.Bounce(timeProgress,beforeMove,changeDistance,targetTime,etype);
		else
			currentValue = TweenMath.Linear(timeProgress,beforeMove,changeDistance,targetTime);
		
		return currentValue;
	}

	public static float Linear(float t,float b,float c,float d)
	{
		return c*t/d + b;
	}

	public static float Quad(float t,float b,float c,float d,EaseType type = EaseType.EASEIN)
	{
		if(type == EaseType.EASEIN)
		{
			return c*(t/=d)*t + b;
		}
		else if(type == EaseType.EASEOUT)
		{
			return -c *(t/=d)*(t-2) + b;
		}
		else
		{
			if ((t/=d/2) < 1) 
				return c/2*t*t + b;
			return -c/2 * ((--t)*(t-2) - 1) + b;
		}
	}

	public static float Cubic(float t,float b,float c,float d,EaseType type = EaseType.EASEIN)
	{
		if(type == EaseType.EASEIN)
		{
			return c*(t/=d)*t*t + b;
		}
		else if(type == EaseType.EASEOUT)
		{
			return c*((t=t/d-1)*t*t + 1) + b;
		}
		else
		{
			if ((t/=d/2) < 1) 
				return c/2*t*t*t + b;
			return c/2*((t-=2)*t*t + 2) + b;
		}
	}
	
	public static float Quart(float t,float b,float c,float d,EaseType type = EaseType.EASEIN)
	{
		if(type == EaseType.EASEIN)
		{
			return c*(t/=d)*t*t*t + b;
		}
		else if(type == EaseType.EASEOUT)
		{
			return -c * ((t=t/d-1)*t*t*t - 1) + b;
		}
		else
		{
			if ((t/=d/2) < 1) 
				return c/2*t*t*t*t + b;
			return -c/2 * ((t-=2)*t*t*t - 2) + b;
		}
	}

	public static float Quint(float t,float b,float c,float d,EaseType type = EaseType.EASEIN)
	{
		if(type == EaseType.EASEIN)
		{
			return c*(t/=d)*t*t*t*t + b;
		}
		else if(type == EaseType.EASEOUT)
		{
			return c*((t=t/d-1)*t*t*t*t + 1) + b;
		}
		else
		{
			if ((t/=d/2) < 1) 
				return c/2*t*t*t*t*t + b;
			return c/2*((t-=2)*t*t*t*t + 2) + b;
		}
	}
		
	public static float Sine(float t,float b,float c,float d,EaseType type = EaseType.EASEIN)
	{
		if(type == EaseType.EASEIN)
		{
			return -c * Mathf.Cos(t/d * (Mathf.PI/2)) + c + b;
		}
		else if(type == EaseType.EASEOUT)
		{
			return c * Mathf.Sin(t/d * (Mathf.PI/2)) + b;
		}
		else
		{
			return -c/2 * (Mathf.Cos(Mathf.PI*t/d) - 1) + b;
		}
	}
	
	public static float Expo(float t,float b,float c,float d,EaseType type = EaseType.EASEIN)
	{
		if(type == EaseType.EASEIN)
		{
			return (t==0) ? b : c * Mathf.Pow(2, 10 * (t/d - 1)) + b;
		}
		else if(type == EaseType.EASEOUT)
		{
			return (t==d) ? b+c : c * (-Mathf.Pow(2, -10 * t/d) + 1) + b;
		}
		else
		{
			if (t==0) 
				return b;
			if (t==d) 
				return b+c;
			if ((t/=d/2) < 1) 
				return c/2 * Mathf.Pow(2, 10 * (t - 1)) + b;
			return c/2 * (-Mathf.Pow(2, -10 * --t) + 2) + b;
		}
	}

	public static float Circ(float t,float b,float c,float d,EaseType type = EaseType.EASEIN)
	{
		if(type == EaseType.EASEIN)
		{
			return -c * (Mathf.Sqrt(1 - (t/=d)*t) - 1) + b;
		}
		else if(type == EaseType.EASEOUT)
		{
			return c * Mathf.Sqrt(1 - (t=t/d-1)*t) + b;
		}
		else
		{
			if ((t/=d/2) < 1) 
				return -c/2 * (Mathf.Sqrt(1 - t*t) - 1) + b;
			return c/2 * (Mathf.Sqrt(1 - (t-=2)*t) + 1) + b;
		}
	}

	public static float Elastic(float t,float b,float c,float d,float a,float p,EaseType type = EaseType.EASEIN)
	{
		if(type == EaseType.EASEIN)
		{
			if (t==0) 
				return b;  
			if ((t/=d)==1) 
				return b+c;  
			if (p!=0) 
				p=d*.3f;
			float s;
			if (a!=0 || a < Mathf.Abs(c)) 
			{ 
				a=c; 
				s=p/4; 
			}
			else 
				s = p/(2*Mathf.PI) * Mathf.Asin (c/a);
			return -(a*Mathf.Pow(2,10*(t-=1))*Mathf.Sin((t*d-s)*(2*Mathf.PI)/p))+b;
		}
		else if(type == EaseType.EASEOUT)
		{
			if (t==0) 
				return b;  
			if ((t/=d)==1) 
				return b+c;  
			if (p!=0) 
				p=d*.3f;
			float s;
			if (a!=0 || a < Mathf.Abs(c)) 
			{ 
				a=c; 
				s=p/4; 
			}
			else 
				s = p/(2*Mathf.PI) * Mathf.Asin (c/a);
			return (a*Mathf.Pow(2,-10*t)*Mathf.Sin((t*d-s)*(2*Mathf.PI)/p)+c+b);
		}
		else
		{
			if (t==0) 
				return b;  
			if ((t/=d/2)==2) 
				return b+c;
			float s;
			if (p!=0) 
				p=d*(.3f*1.5f);
			if (a!=0 || a < Mathf.Abs(c)) 
			{ 
				a=c;
				s=p/4; 
			}
			else
				s = p/(2*Mathf.PI) * Mathf.Asin (c/a);
			if (t < 1) 
				return -.5f*(a*Mathf.Pow(2,10*(t-=1))*Mathf.Sin((t*d-s)*(2*Mathf.PI)/p))+b;
			return a*Mathf.Pow(2,-10*(t-=1)) * Mathf.Sin((t*d-s)*(2*Mathf.PI)/p)*.5f+c+b;
		}
	}

	public static float Back(float t,float b,float c,float d,float s=0,EaseType type = EaseType.EASEIN)
	{
		if(type == EaseType.EASEIN)
		{
			if (s == 0) 
				s = 1.70158f;
			return c*(t/=d)*t*((s+1)*t-s)+b;
		}
		else if(type == EaseType.EASEOUT)
		{
			if (s == 0) 
				s = 1.70158f;
			return c*((t=t/d-1)*t*((s+1)*t+s)+1)+b;
		}
		else
		{
			if (s == 0) 
				s = 1.70158f; 
			if ((t/=d/2) < 1) 
				return c/2*(t*t*(((s*=(1.525f))+1)*t-s))+b;
			return c/2*((t-=2)*t*(((s*=(1.525f))+1)*t+s)+2)+b;
		}
	}

	public static float Bounce(float t,float b,float c,float d,EaseType type = EaseType.EASEIN)
	{
		if(type == EaseType.EASEIN)
		{
			return BounceEaseIn(t,b,c,d);
		}
		else if(type == EaseType.EASEOUT)
		{
			return BounceEaseOut(t,b,c,d);
		}
		else
		{
			if (t < d/2)
				return BounceEaseIn(t*2,0,c,d)*.5f+b;
			else 
				return BounceEaseOut(t*2-d,0,c,d)*.5f+c*.5f+b;
		}
	}

	private static float BounceEaseIn(float t,float b,float c,float d)
	{
		return c-BounceEaseOut(d-t,0,c,d)+b;
	}
	
	private static float BounceEaseOut(float t,float b,float c,float d)
	{
		if ((t/=d)<(1/2.75f))
			return c*(7.5625f*t*t)+b;
		else if (t < (2/2.75f))
			return c*(7.5625f*(t-=(1.5f/2.75f))*t+.75f)+b;
		else if (t<(2.5f/2.75f))
			return c*(7.5625f*(t-=(2.25f/2.75f))*t+.9375f)+b;
		else
			return c*(7.5625f*(t-=(2.625f/2.75f))*t+.984375f)+b;
	}
}
