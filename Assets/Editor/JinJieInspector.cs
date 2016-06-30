using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JinJie))]
public class JinJieInspector : Editor
{
    private JinJie _target = null;
    private string[] contents = null;

    private void Awake()
    {
        _target = (JinJie)target;
        contents = new string[_target.stages.Length];

        for (int j = 0; j < contents.Length; j++)
        {
            contents[j] = j.ToString();
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        _target.stage = GUILayout.SelectionGrid(_target.stage, contents, 3);
        if (GUILayout.Button("Use"))
        {
            _target.Use();
        }
    }
}