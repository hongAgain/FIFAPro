using System;
public class PFConfig
{
	/*!
	 * @Platform Type for Simulation in plugin editor
	 */
	public enum PFTYPE {
		DEFAULT,
		IOS,
		ANDROID,
	};
	public static PFTYPE PFType = PFTYPE.ANDROID;
	public static void SetPFType(PFTYPE type)
	{
		PFType = type;
	}
	public static PFTYPE GetPFType ()
	{
		return PFType;
	}
}

