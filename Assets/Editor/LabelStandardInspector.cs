using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LabelStandard))]
public class LabelStandardInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Use"))
        {
            var _target = (target as LabelStandard);
            var label = _target.GetComponent<UILabel>();
            LabelStandard.Refresh(label, _target.value);
        }
    }
}