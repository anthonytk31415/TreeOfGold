using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


// here's where rwe can use the editor to apply methods 

public class EditorMenu : EditorWindow
{
    string animatorName = "Add Animator Controller Name Here.";

    [MenuItem("Window / Custom Controls / Show Menu")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EditorMenu));
    }

    [MenuItem("Window / Custom Controls / My Command")]
    public static void MyCommandFromMenu()
    {
        InitiateMyCommand();
    }

    static void InitiateMyCommand()
    {
        Debug.Log("Hello World");
    }

    // the menu with settings
    private void OnGUI()
    {
        GUILayout.Label("Apply Settings on Animator Controller", EditorStyles.boldLabel);
        animatorName = EditorGUILayout.TextField("Animator Controller Name: ", animatorName);
        //EditorGUILayout.EndToggleGroup();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Apply", GUILayout.Width(100), GUILayout.Height(30)))
        {
            Debug.Log("pressing the button!" + animatorName);
            EditorBuildAnimatorSettings.CreateTest(animatorName);
            //asdf
        }
        EditorGUILayout.EndHorizontal();
    }


}
