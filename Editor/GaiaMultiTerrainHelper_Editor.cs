using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GaiaMultiTerrainHelper))]
public class GaiaMultiTerrainHelper_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Refresh"))
            (target as GaiaMultiTerrainHelper).Refresh();

        if (GUILayout.Button("Flatten All"))
            (target as GaiaMultiTerrainHelper).FlattenAll();

        if (GUILayout.Button("Reset to Terrain Grid"))
            (target as GaiaMultiTerrainHelper).SetGaiaMatrix();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Set Gaia Active"))
            (target as GaiaMultiTerrainHelper).SetGaiaActive(serializedObject.FindProperty("activeTerrain").intValue);
        if (GUILayout.Button("Set & Play Gaia Active"))
        {
            (target as GaiaMultiTerrainHelper).SetGaiaActive(serializedObject.FindProperty("activeTerrain").intValue);
            (target as GaiaMultiTerrainHelper).PlaySession();
        }
        //serializedObject.FindProperty("activeTerrain").intValue = EditorGUILayout.IntField(new GUIContent("Active Terrain"), serializedObject.FindProperty("activeTerrain").intValue, GUILayout.Width(300));
        serializedObject.FindProperty("activeTerrain").intValue = EditorGUILayout.IntSlider(serializedObject.FindProperty("activeTerrain").intValue, 0, serializedObject.FindProperty("terrains").arraySize - 1);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
}
