using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;



/// <summary>
///  dont forget to make sure the "walks" are looped
/// </summary>

public class EditorBuildAnimatorSettings
{
    public static void BuildAnimationControllerAndPrefab(string animatorControllerName){
        UnityEditor.Animations.AnimatorController animController = CreateAnimationController(animatorControllerName); 
    
        // Add parameters (bool) to Animator controller
        List<string> stringStates = new List<string> {
            "idleDown", "idleUp", "idleLeft", "idleRight",
            "walkUp", "walkDown", "walkLeft", "walkRight"
        };

        AddAnimationControllerParameters(animController, stringStates);

        // skipping creating animation clips, since we already have them.     
        // find animation clip

        string filePath = "Assets/Resources/Animations/" + animatorControllerName + "/";
        string pathForResources = "Animations/" + animatorControllerName + "/";

        // create animation clips; CAN BE DISABLED 
        // foreach(string stringState in stringStates){
        //     CreateAnimationClip(stringState, filePath);
        // }

        List<UnityEditor.Animations.AnimatorState> animatorStates = BuildStateMachineWithStates(
            animController, stringStates, pathForResources
        );

        List<AnimatorStateTransition> animatorStateTransitions = BuildAnimatorStateTransitions(animatorStates);

        foreach(AnimatorStateTransition animatorStateTransition in animatorStateTransitions){
            ApplyTransitionProperties(animatorStateTransition);
        }

        CreateChar(animatorControllerName);
    }

    // instantiate the animation controller for a given character to trigger animations. 
    // this is save din assets/resources/animations/<character name> 
    public static UnityEditor.Animations.AnimatorController CreateAnimationController(string animatorControllerName){
        string folderPath = "Assets/Resources/Animations";
        if (!AssetDatabase.IsValidFolder(folderPath + "/" + animatorControllerName))
        {
            AssetDatabase.CreateFolder(folderPath, animatorControllerName);
        }
        string filePath = folderPath + "/" + animatorControllerName + "/";
        string animControllerName = animatorControllerName + "AnimationController.controller";
        var controller = UnityEditor.Animations.AnimatorController
                .CreateAnimatorControllerAtPath(filePath + animControllerName);
        return controller;
    }

    // add the all the parameters as defined by your list of states in bools 
    // so you can use to toggle between states
    public static void AddAnimationControllerParameters(UnityEditor.Animations.AnimatorController controller, 
            List<string> stringStates
    ){
        foreach (string animState in stringStates)
        {
            controller.AddParameter(animState + "Bool", AnimatorControllerParameterType.Bool);
        }
    }


    // create animation clip; save it to database at filepath; 
    // since we largely build animations by hand, we just want to run this code once
    // to build the shell, the build the animations, and save the animations. 
    public static AnimationClip CreateAnimationClip(string clipName, string filePath){
        AnimationClip animationClip = new AnimationClip();
        animationClip.name = clipName;
        AssetDatabase.CreateAsset(animationClip, filePath + clipName + ".anim");
        return animationClip;
    }

    // build the animate state machine that the attaches the already loaded corresponding animation
    // to the state. returns a list of the states for further use. 
    public static List<UnityEditor.Animations.AnimatorState> BuildStateMachineWithStates(
            UnityEditor.Animations.AnimatorController controller, 
            List<string> stringStates, string pathForResources        
    ){
        var rootStateMachine = controller.layers[0].stateMachine;
        List<UnityEditor.Animations.AnimatorState> animationStates = new();
        foreach (string animStateName in stringStates)
        {
            UnityEditor.Animations.AnimatorState curState = rootStateMachine.AddState(animStateName);
            AnimationClip animationClip = Resources.Load<AnimationClip>(pathForResources + animStateName);
            Debug.Log(animationClip);
            curState.motion = animationClip;
            animationStates.Add(curState);
        }
        return animationStates;
    }

    // each state can get into another state by setting the bool to true; this code builds the graph
    public static List<UnityEditor.Animations.AnimatorStateTransition> BuildAnimatorStateTransitions(
            List<UnityEditor.Animations.AnimatorState> animationStates) 
    {

        // UnityEditor.Animations.AnimatorState idleDown = animationStates[0];
        // Debug.Log(idleDown);

        List<UnityEditor.Animations.AnimatorStateTransition> allTransitions = new();

        for (int i = 0; i < animationStates.Count; i ++){
            AnimatorState toState = animationStates[i];
            for (int j = i + 1; j < animationStates.Count; j ++){
                if (i == j){
                    continue; 
                }
                AnimatorState fromState = animationStates[j];
                UnityEditor.Animations.AnimatorStateTransition defaultStateToCurStateTransition = toState.AddTransition(fromState);
                defaultStateToCurStateTransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, fromState.name + "Bool");
                allTransitions.Add(defaultStateToCurStateTransition);

                UnityEditor.Animations.AnimatorStateTransition curStateToDefaultState = fromState.AddTransition(toState);
                curStateToDefaultState.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, toState.name + "Bool");
                allTransitions.Add(curStateToDefaultState);
            }
        }

        return allTransitions; 
    }

    // applies proper StateTransitions for character movement
    public static void ApplyTransitionProperties(UnityEditor.Animations.AnimatorStateTransition transition){
        transition.hasExitTime = true;
        transition.exitTime = 0;
        transition.hasFixedDuration = true;
        transition.duration = 0.0f;
        transition.offset = 0;
    }


    // creates a sprite and a prefab with the proepr settings like sprite renderer, 
    // animator, attaches the appropriate runtimeAnimatorController to the sprite
    // and saves it to the characters folder in resources
    public static void CreateChar(string charName)
    {
        // sprites loaded using resorces.load must be in the resources folder. 
        var curSprite = Resources.Load<Sprite>("Sprites/" + charName + "/" + charName + "_walk_front0");

        GameObject newChar = new GameObject(charName);
        var spriteRenderer = newChar.AddComponent<SpriteRenderer>();

        spriteRenderer.sprite = curSprite;
        newChar.transform.position = new Vector3(3.5f, 1.5f, 0f);
        spriteRenderer.sortingOrder = 1; // define as 1 to sort on top
        var animator = newChar.AddComponent<Animator>();

        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>
                ("Animations/" + charName + "/" + charName + "AnimationController");

        string localPath = "Assets/Resources/Prefabs/Characters/" + charName + ".prefab";

        // Make sure the file name is unique, in case an existing Prefab has the same name.
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        // Create the new Prefab and log whether Prefab was saved successfully.
        bool prefabSuccess;
        PrefabUtility.SaveAsPrefabAsset(newChar, localPath, out prefabSuccess);


        if (prefabSuccess == true)
            Debug.Log("Prefab was saved successfully");
        else
            Debug.Log("Prefab failed to save" + prefabSuccess);

        /// add in order layer

    }



    public static void CreateAnimatorControllerDEPRECATED(string animatorControllerName)
    {
        // Creates the controller
        string folderPath = "Assets/Resources/Animations";
        if (!AssetDatabase.IsValidFolder(folderPath + "/" + animatorControllerName))
        {
            AssetDatabase.CreateFolder(folderPath, animatorControllerName);
        }
        string filePath = folderPath + "/" + animatorControllerName + "/";
        string animControllerName = animatorControllerName + "AnimationController.controller";
        var controller = UnityEditor.Animations.AnimatorController
                .CreateAnimatorControllerAtPath(filePath + animControllerName);

        // Add parameters (bool) to Animator controller
        List<string> stringStates = new List<string> {
            "idleDown", "idleUp", "idleLeft", "idleRight",
            "walkUp", "walkDown", "walkLeft", "walkRight"
        };

        foreach (string animState in stringStates)
        {
            controller.AddParameter(animState + "Bool", AnimatorControllerParameterType.Bool);
        }

        // Create State Machine and add AnimationStates, while also binding animation clip template
        var rootStateMachine = controller.layers[0].stateMachine;

        List<UnityEditor.Animations.AnimatorState> animationStates = new();
        foreach (string animState in stringStates)
        {
            UnityEditor.Animations.AnimatorState curState = rootStateMachine.AddState(animState);

            // Create Animation Clip for each Animation State
            AnimationClip animationClip = new AnimationClip();
            animationClip.name = animState;
            AssetDatabase.CreateAsset(animationClip, filePath + animationClip.name + ".anim");

            // add Animation Clip to the state
            curState.motion = animationClip;

            // add to list of animationstates
            animationStates.Add(curState);
        }

        // identify idledown to create states
        UnityEditor.Animations.AnimatorState idleDown = animationStates[0];

        // Build Animator State Transitions: 
        // add pairs of transitions from nonDefaultState to defaultState (which is idleDown)
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
            transition.duration = 0.0f;
            transition.offset = 0;
        }

        CreateChar(animatorControllerName);
    }

    // takes a charName and, with the charName that presumably has animationcontroller
    // with animations, binds that controller to a new char object

}








