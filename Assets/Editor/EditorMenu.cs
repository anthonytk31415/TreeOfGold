using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


// here's where rwe can use the editor to apply methods 

public class EditorMenu : EditorWindow
{
    string animatorName = "Add Animator Controller Name Here (currently name does not do anything).";

    [MenuItem("Window / Custom Controls / Create Animator Controller")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EditorMenu));
    }

    [MenuItem("Window / Custom Controls / Create Character from baseCharacter")]
    public static void InitiateCreationBuildCharacter()
    {
        EditorBuildCharacter.InstantiateCharacterFromBaseCharacter("locke");
    }

    [MenuItem("Window / Custom Controls / Create baseCharacter")]
    public static void InitiateBaseCharacter()
    {
        EditorBuildCharacter.BuildCharacter("baseCharacter");
    }


    [MenuItem("Window / Custom Controls / Create Sprite")]
    public static void InitiateCreateSprite()
    {
        EditorBuildAnimatorSettings.CreateChar("locke");
    }

    [MenuItem("Window / Custom Controls / Test: UpdateAnimator ")]
    public static void InitiateUpdateAnimator()
    {
        EditorBuildAnimatorSettings.UpdateAnimator("baseCharacter");
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

            // List<string> chars = new List<string> {"celes", "sabin", "shadow", "terra", "gerad", "locke"};
            List<string> chars = new List<string> {"locke", "celes"};
            foreach (string charName in chars){
                // EditorBuildAnimatorSettings.NewCharPrefabWithAnimator(charName);

                // build char from basechar
                EditorBuildCharacter.InstantiateCharacterFromBaseCharacter(charName);
            }

        }
        EditorGUILayout.EndHorizontal();
    }


}
