using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TheSTAR.GUI;

[CustomEditor(typeof(GuiScreen), true)]
public class GuiScreenEditor : Editor
{
    private GuiScreen _target => (GuiScreen)target;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(10);
        DrawUE();
    }

    private void DrawUE()
    {
        _target.useUniversalElements = GUILayout.Toggle(_target.useUniversalElements, "Universal Elements");

        if (_target.useUniversalElements)
        {
            var allUeTypes = ReflectiveEnumerator.GetEnumerableOfType<GuiUniversalElement>();

            for (int i = 0; i < allUeTypes.Count; i++)
            {
                if (!_target.universalElementsSettings.ContainsKey(i)) _target.universalElementsSettings.Add(i, false);

                _target.universalElementsSettings.Set(i, GUILayout.Toggle(_target.universalElementsSettings.Get(i), GetSimpleTypeName(allUeTypes[i].ToString())));
            }
        }

        if (_target.universalElementsSettings.ContainsKey(0) && _target.universalElementsSettings.Get(0))
        {
            _target.useSoftCounter = GUILayout.Toggle(_target.useSoftCounter, "UseSoftCounter");
            _target.useHardCounter = GUILayout.Toggle(_target.useHardCounter, "UseHardCounter");
        }
    }

    private string GetSimpleTypeName(string fullTypeName)
    {
        StringBuilder builder = new("");

        char symbol;

        for (int i = fullTypeName.Length - 1; i >= 0; i--)
        {
            symbol = fullTypeName[i];
            if (symbol != '.') builder.Insert(0, symbol);
            else break;
        }

        return builder.ToString();
    }
}