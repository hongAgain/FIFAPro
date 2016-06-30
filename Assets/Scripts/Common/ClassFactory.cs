using System;
using System.Collections.Generic;
using System.Reflection;

public class ClassFactory
{
    public static readonly ClassFactory Instance = new ClassFactory();
    private ClassFactory()
    {
        m_kAssemblies.Add(Assembly.GetExecutingAssembly());
    }
    public object CreateClass(String strName)
    {
        Type kType = GetType(strName);
        if(null != kType)
            return Activator.CreateInstance(kType);
        return null;
    }
    public Type GetType(String strName)
    {
        for (int iIdx = 0; iIdx < m_kAssemblies.Count; iIdx++)
        {
            Type kType = m_kAssemblies[iIdx].GetType(strName);
            if (null != kType)
            {
                return kType;
            }
        }
        return null;
    }
    private List<Assembly> m_kAssemblies = new List<Assembly>();
}