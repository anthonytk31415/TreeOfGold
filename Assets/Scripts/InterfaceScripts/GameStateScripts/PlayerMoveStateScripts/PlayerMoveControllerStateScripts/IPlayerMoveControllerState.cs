using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for the PlayerMoveControllerState for interface...
/// </summary>
public interface IPlayerMoveControllerState {
    public void Enter();
    public void Update();
    public void Exit();
    public void Process(Coordinate w);


}