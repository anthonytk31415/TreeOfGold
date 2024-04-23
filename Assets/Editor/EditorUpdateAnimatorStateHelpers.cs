using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System;
using System.IO;



public class EditorBuildAnimatorStateHelpers
{

    /// <summary>
    /// remove all parameters from the controller
    /// </summary>
    /// <param name="charName"></param>
    public static void DeleteAllParameters(string charName){
        AnimatorController controller = GetAnimatorController(charName); 
        // var rootStateMachine = controller.layers[0].stateMachine; 

        foreach(AnimatorControllerParameter parameter in controller.parameters){
            controller.RemoveParameter(parameter);
        }        
    }

    /// <summary>
    ///  Returns a list of _Up, _Down, etc. strings 
    /// </summary>
    /// <param name="inputString"></param>
    /// <returns></returns>
    public static List<string> CreateDirectionalStrings(string inputString){
        List<string> res = new(); 
        List<string> directions = new () {"Up", "Down", "Left", "Right"};
        foreach (string direction in directions){
            res.Add(inputString + direction);
        }
        return res; 
    } 


    /// <summary>
    ///  Given a string list of states, create the state and then bind the 
    ///  already created clip to it. 
    /// </summary>
    public static List<UnityEditor.Animations.AnimatorState> CreateStates(List<string> listStates, string charName){
        AnimatorController controller = GetAnimatorController(charName); 
        string pathForResources = "Animations/" + charName + "/";
        List<UnityEditor.Animations.AnimatorState> res = new(); 

        var rootStateMachine = controller.layers[0].stateMachine; 
        // need the clip name 
        foreach (string state in listStates){
            List<string> stateDirs = CreateDirectionalStrings(state);
            foreach (string stateName in stateDirs){
                UnityEditor.Animations.AnimatorState animState = rootStateMachine.AddState(stateName);
                AnimationClip animationClip = Resources.Load<AnimationClip>(pathForResources + stateName);
                animState.motion = animationClip;
                res.Add(animState); 
            }
        }
        return res; 
    }

    /// <summary>
    ///  Bind stateName in the controller 
    /// </summary>
    /// <param name="stateName"></param>
    /// <param name="controller"></param>
    public static bool BindDefaultState(string stateName, AnimatorController controller){
        var rootStateMachine = controller.layers[0].stateMachine; 
        foreach (ChildAnimatorState state in rootStateMachine.states){
            if (state.state.name.Equals(stateName)){
                rootStateMachine.defaultState = state.state;
                return true;
            }
        }
        return false; 
    }
    

    public static void ApplyStateProperties(UnityEditor.Animations.AnimatorState animState){
        // animState.
    }


    /// <summary>
    ///  given a string list of states, parameter triggers;
    /// </summary>
    public static void CreateTriggers(List<string> listStates, string charName){
        AnimatorController controller = GetAnimatorController(charName); 

        foreach (string state in listStates){
            List<string> stateDirs = CreateDirectionalStrings(state);
            foreach (string stateDir in stateDirs){
                controller.AddParameter(stateDir, AnimatorControllerParameterType.Trigger);
            }
        }
    }

    /// <summary>
    /// Given a list of strings or so, create transitions and bind
    /// the trigger to the transition
    /// </summary>
    public static void BuildTransitions () {

    }





    // given 2 lists of AnimatorStates: (1) idleStates list and the (2) fromIdleStates, build the state transitions, 
    // apply Bool for fromIdle -> Idle transition, create a trigger for idle --> fromIdle, and return allTransitions.    
    public static List<UnityEditor.Animations.AnimatorStateTransition> BuildAnimatorStateTransitionsForIdleBlink(
            List<UnityEditor.Animations.AnimatorState> transitionStates, List<UnityEditor.Animations.AnimatorState> idleStates) 
    {
        List<UnityEditor.Animations.AnimatorStateTransition> allTransitions = new();
        foreach (AnimatorState idleState in idleStates) {
            foreach (AnimatorState transitionState in transitionStates) {
                UnityEditor.Animations.AnimatorStateTransition idleToAttackTransition = transitionState.AddTransition(idleState);
                idleToAttackTransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, idleState.name + "Bool");
                allTransitions.Add(idleToAttackTransition);

                UnityEditor.Animations.AnimatorStateTransition attackToIdleTransition = idleState.AddTransition(transitionState);
                attackToIdleTransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, transitionState.name + "Trigger");
                allTransitions.Add(attackToIdleTransition);
            }
        }
        return allTransitions; 
    }


    /// <summary>
    /// return animator states based on string conditions 
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="containsCondition"></param>
    /// <param name="doesNotContainCondition"></param>
    /// <param name="doesNotContainRequires"></param>
    /// <returns></returns>
    public static List<UnityEditor.Animations.AnimatorState> ListOfAnimatorStates(
                AnimatorController controller, 
                string containsCondition, 
                string doesNotContainCondition, 
                bool doesNotContainRequires)
    {
        
        List<UnityEditor.Animations.AnimatorState> res = new();
        var rootStateMachine = controller.layers[0].stateMachine; 

        foreach(ChildAnimatorState state in rootStateMachine.states){
            if (doesNotContainRequires){
                if (state.state.name.Contains(containsCondition) && !state.state.name.Contains(doesNotContainCondition))
                { 
                    res.Add(state.state);
                }
            }
            else {
                if (state.state.name.Contains(containsCondition)){
                    res.Add(state.state);
                }
            }
        }
        return res; 
    }

    /// <summary>
    /// Find the animator controller
    /// </summary>
    /// <param name="charName"></param>
    /// <returns></returns>
    public static AnimatorController GetAnimatorController(string charName){
        string filePath = "Animations/" + charName + "/" + charName; 
        AnimatorController controller = Resources.Load<AnimatorController>(filePath);
        return controller; 
    }

    /// <summary>
    /// given an animator controller, do stuff
    /// applied this to black and red blink
    /// </summary>
    /// <param name="charName"></param>
    public static void ApplyChangesToAnimatorController(string charName){
        AnimatorController controller = GetAnimatorController(charName); 

        // find pure idles 
        List<UnityEditor.Animations.AnimatorState>  pureIdleStates = ListOfAnimatorStates(controller, "idle", "Blink", true); 
        AuditDebug.DebugIter(pureIdleStates);
        
        // find blinkRed
        List<UnityEditor.Animations.AnimatorState> idleBlinkBlackStates = ListOfAnimatorStates(controller, "idleBlinkBlackUp", "", false); 
        AuditDebug.DebugIter(idleBlinkBlackStates);

        // build the transitions 
        BuildAnimatorStateTransitionsForIdleBlink(idleBlinkBlackStates, pureIdleStates);
    }



    // for each state, for each transitoin: apply settings
    public static void ApplyTransitionProperties(string charName)
    {
        AnimatorController controller = GetAnimatorController(charName); 

        var rootStateMachine = controller.layers[0].stateMachine; 
        foreach (ChildAnimatorState state in rootStateMachine.states){
            foreach (UnityEditor.Animations.AnimatorStateTransition transition in state.state.transitions)
            {
                EditorBuildAnimatorSettings.ApplyTransitionProperties(transition); 
            }
        }

    }
}
    