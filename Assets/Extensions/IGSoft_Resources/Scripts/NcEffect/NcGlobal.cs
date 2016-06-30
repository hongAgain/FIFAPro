using UnityEngine;

class NcGlobal
{
    public static float GetEngineTime()
    {
        if (Time.time == 0)
            return 0.000001f;

        if (TimeScaleEnable)
            return Time.time;
        else
            return (Time.time / Time.timeScale);
    }

    public static float GetEngineDeltaTime()
    {
        if (TimeScaleEnable)
            return Time.deltaTime;
        else
            return (Time.deltaTime/Time.timeScale);
    }
    public static bool TimeScaleEnable = false;
}