﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System;
using System.IO;

// Durations need to be 0 
// 


/// <summary>
/// EditorBuildAnimatorSettings builds the animation controller and prefab. Will handle
/// all the state transitions, boolean/trigger properties, transitions, and perhaps
/// instantiate (maybe? still needed?) the animation clips. Will also bind clips to specific states. 
/// </summary>

public class EditorBuildAnimatorSettings
{
    /// <summary>
    /// The main Method of the class. Creates the animation controller, instantiates a gameObject from 
    /// a prefab, and then attaches the controller to the prefab. 
    /// Will probably update this so the steps go: create shell, use baseCharacter, then save. 
    /// </summary>
    /// <param name="animatorControllerName"></param>
    public static void BuildAnimationControllerAndPrefab(string animatorControllerName){
        UnityEditor.Animations.AnimatorController animController = CreateAnimationController(animatorControllerName); 

        List<string> idleStates = new List<string> {
            "idleUp", "idleDown", "idleLeft", "idleRight" 
            };

        List<string> walkStates = new List<string> {
            "walkUp", "walkDown", "walkLeft", "walkRight" 
            };    

        List<string> attackSwordStates = new List<string> {
            "attackSwordUp", "attackSwordDown", "attackSwordLeft", "attackSwordRight", 
            }; 

        List<string> idleWalkStates = new(); 
        idleWalkStates.AddRange(idleStates);
        idleWalkStates.AddRange(walkStates); 

        List<List<string>> allListStates = new List<List<string>>() {idleStates, walkStates, attackSwordStates};

        // Add parameters (bool) to Animator controller
        List<string> stringStates = new();
        foreach (List<String> listState in allListStates){
            stringStates.AddRange(listState);
        }

        AddAnimationControllerParameters(animController, stringStates);

        string filePath = "Assets/Resources/Animations/" + animatorControllerName + "/";
        string pathForResources = "Animations/" + animatorControllerName + "/";

        // skipping creating animation clips, since we already have them.     
        // find animation clip

        // create animation clips; CAN BE DISABLED 
        CreateAnimationClips(stringStates, filePath); 

        // build states and attach the already built animation clips to the corresponding state; 
        // do it for all states
        List<UnityEditor.Animations.AnimatorState> animatorStates = BuildStateMachineWithStates(
            animController, stringStates, pathForResources
        );

        
        List<UnityEditor.Animations.AnimatorState> idleAnimatorStates = new();
        List<UnityEditor.Animations.AnimatorState> walkAnimatorStates = new();
        List<UnityEditor.Animations.AnimatorState> attackAnimatorStates = new();
    
        foreach (AnimatorState animatorState in animatorStates){
            if (animatorState.name.Contains("idle")){
                idleAnimatorStates.Add(animatorState);
            } else if (animatorState.name.Contains("walk")){
                walkAnimatorStates.Add(animatorState);
            } else if (animatorState.name.Contains("attack")) {
                attackAnimatorStates.Add(animatorState);
            }
        }

        List<AnimatorState> idleWalkAnimatorStates = new(); 
        idleWalkAnimatorStates.AddRange(idleAnimatorStates);
        idleWalkAnimatorStates.AddRange(walkAnimatorStates); 

        List<AnimatorStateTransition> animatorStateTransitions = BuildAnimatorStateTransitionsForIdleWalk(idleWalkAnimatorStates);
        animatorStateTransitions.AddRange(BuildAnimatorStateTransitionsForIdleAttack(attackAnimatorStates, idleWalkAnimatorStates));

        foreach(AnimatorStateTransition animatorStateTransition in animatorStateTransitions){
            ApplyTransitionProperties(animatorStateTransition);
        }

        CreateChar(animatorControllerName);
    }

/// <summary>
/// Instantiate the animation controller for a given character to trigger animations. this is save din assets/resources/animations/<character name> 
/// </summary>
/// <param name="animatorControllerName"></param>
/// <returns></returns>
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

/// <summary>
/// Add the all the parameters to your animation controller, as defined by your list of states.  
/// Used for to triggering between states. 
/// Attacks = trigger. all else = bool. Might change later. 
/// </summary>
/// <param name="controller"></param>
/// <param name="stringStates"></param>
    public static void AddAnimationControllerParameters(UnityEditor.Animations.AnimatorController controller, 
            List<string> stringStates
    ){
        foreach (string animState in stringStates)
        {
            if (animState.Contains("attack")){
                controller.AddParameter(animState + "Trigger", AnimatorControllerParameterType.Trigger);
            }
            else {
                controller.AddParameter(animState + "Bool", AnimatorControllerParameterType.Bool);
            }
        }
    }


    // create animation clip; save it to database at filepath; 
    // since we largely build animations by hand, we just want to run this code once
    // to build the shell, the build the animations, and save the animations. 
    public static AnimationClip CreateAnimationClip(string clipName, string filePath)
    {
        AnimationClip animationClip = new AnimationClip();
        animationClip.name = clipName;
        if (clipName.Contains("walk")){
            AnimationUtility.GetAnimationClipSettings(animationClip).loopTime = true;
        }
        AssetDatabase.CreateAsset(animationClip, filePath + clipName + ".anim");
        return animationClip;
    }

    public static List<AnimationClip> CreateAnimationClips(List<string> clipNames, string filePath){
        List<AnimationClip> res = new();
        foreach(string clipName in clipNames){
            string fullpathAndFileName = filePath + clipName + ".anim"; 
            if (!File.Exists(fullpathAndFileName)){
                AnimationClip curClip = CreateAnimationClip(clipName, filePath);
                res.Add(curClip);
            }
        }
        return res; 
    }


    // build the animate state machine that then attaches the already loaded corresponding animation
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
            if(animStateName.Equals("idleDown")){
                rootStateMachine.defaultState = curState;
            }
            AnimationClip animationClip = Resources.Load<AnimationClip>(pathForResources + animStateName);
            curState.motion = animationClip;
            animationStates.Add(curState);
        }
        return animationStates;
    }

    // each state can get into another state by setting the bool to true; this code builds the graph
    public static List<UnityEditor.Animations.AnimatorStateTransition> BuildAnimatorStateTransitionsForIdleWalk(
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

/// <summary>
/// Given idle and attack state transitions (in the same animator controller), Build the idle <-> attack transitions.
/// </summary>
/// <param name="attackStates"></param>
/// <param name="idleStates"></param>
/// <returns></returns>
    public static List<UnityEditor.Animations.AnimatorStateTransition> BuildAnimatorStateTransitionsForIdleAttack(
            List<UnityEditor.Animations.AnimatorState> attackStates, List<UnityEditor.Animations.AnimatorState> idleStates) 
    {

        List<UnityEditor.Animations.AnimatorStateTransition> allTransitions = new();

        foreach (AnimatorState idleState in idleStates) {
            foreach (AnimatorState attackState in attackStates) {
                UnityEditor.Animations.AnimatorStateTransition idleToAttackTransition = attackState.AddTransition(idleState);
                idleToAttackTransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, idleState.name + "Bool");
                allTransitions.Add(idleToAttackTransition);


                UnityEditor.Animations.AnimatorStateTransition attackToIdleTransition = idleState.AddTransition(attackState);
                attackToIdleTransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, attackState.name + "Trigger");
                allTransitions.Add(attackToIdleTransition);
            }
        }
        return allTransitions; 
    }




/// <summary>
/// Applies proper StateTransitions for character movement transition. 
/// </summary>
/// <param name="transition"></param>
    public static void ApplyTransitionProperties(UnityEditor.Animations.AnimatorStateTransition transition){
        transition.hasExitTime = true;
        transition.exitTime = 0;
        transition.hasFixedDuration = true;
        transition.duration = 0.0f;
        transition.offset = 0;
    }




/// <summary>
/// creates a sprite and a prefab with the proper settings like sprite renderer, 
/// animator, attaches the appropriate runtimeAnimatorController to the sprite
/// and saves it to the characters folder in resources
/// </summary>
/// <param name="charName"></param>
    public static void CreateChar(string charName)
    {
        // sprites loaded using resources.load must be in the resources folder. 
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



}








