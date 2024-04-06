using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class EditorBuildAnimatorSettings
{

    public static void CreateTest(string animatorControllerName)
    {
        // Creates the controller


        string folderPath = "Assets/Animations";
        if (!AssetDatabase.IsValidFolder(folderPath + "/" + animatorControllerName))
        {
            AssetDatabase.CreateFolder(folderPath, animatorControllerName);
        }
        string filePath = folderPath + "/"+ animatorControllerName + "/";
        string animControllerName = animatorControllerName + "AnimationController.controller";
        //Debug.Log("filePath: " + filePath + animControllerName);
        var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(filePath + animControllerName);


        //Add parameters
        List<string> stringStates = new List<string> { "idleDown", "idleUp", "idleLeft", "idleRight", "walkUp", "walkDown", "walkLeft", "walkRight" };

        // Instantiate animationState

        foreach (string animState in stringStates)
        {
            controller.AddParameter(animState + "Bool", AnimatorControllerParameterType.Bool);
        }

        // Add StateMachines
        var rootStateMachine = controller.layers[0].stateMachine;

        List<UnityEditor.Animations.AnimatorState> animationStates = new();
        foreach (string animState in stringStates)
        {
            UnityEditor.Animations.AnimatorState curState = rootStateMachine.AddState(animState);

            // create clip to later update; save it
            AnimationClip animationClip = new AnimationClip();
            animationClip.name = animState;
            AssetDatabase.CreateAsset(animationClip, filePath + animationClip.name + ".anim");

            // add clip to the state
            curState.motion = animationClip;

            // add to list of animationstates
            animationStates.Add(curState);
        }

        UnityEditor.Animations.AnimatorState idleDown = animationStates[0];

        //// add pairs of transitions from nonDefaultState to defaultState (which is idleDown)
        List<UnityEditor.Animations.AnimatorStateTransition> allTransitions = new();
        foreach (UnityEditor.Animations.AnimatorState curState in animationStates)
        {
            if (curState.name != "idleDown")
            {
                UnityEditor.Animations.AnimatorStateTransition defaultStatetoCurStateTransition = idleDown.AddTransition(curState);
                defaultStatetoCurStateTransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, curState.name + "Bool");
                allTransitions.Add(defaultStatetoCurStateTransition);

                UnityEditor.Animations.AnimatorStateTransition curStateToDefaultState = curState.AddTransition(idleDown);
                curStateToDefaultState.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "idleDownBool");
                allTransitions.Add(curStateToDefaultState);

                // add in condition

            }
        }

        // apply properties to each transition
        foreach (UnityEditor.Animations.AnimatorStateTransition transition in allTransitions)
        {
            transition.hasExitTime = true;
            transition.exitTime = 0;
            transition.hasFixedDuration = true;
            transition.duration = 0.25f;
            transition.offset = 0;
        }


    }
}








