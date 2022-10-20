using System.Runtime.CompilerServices;
using UnityEngine;
using Controllers;
using Entity;
using StateMachine.Anim;
using StateMachine.Base;
using StateMachine.Logic;
using Unity.VisualScripting;

public class MgrPlayer
{
    #region go
    public GameObject MyPlayer => GameObject.Find("Player");
    
    public GameObject MyCamera => GameObject.FindWithTag("MainCamera");

    public GameConfig Config => GameObject.Find("Config").GetComponent<GameConfig>();

    private CharacterController CharacterController => MyPlayer.GetComponent<CharacterController>();

    #endregion

    #region controllers

    public readonly CameraController CameraController;
    
    public LogicStateMachine LogicStateMachine;

    public AnimStateMachine AnimStateMachine;

    #endregion

    #region entity

    public PlayerEntity MyPlayerEntity;

    #endregion
    
    public MgrPlayer()
    {
        LogicStateMachine = new LogicStateMachine();
        AnimStateMachine = new AnimStateMachine();

        PlayerAnimState.AnimController = new AnimatorController(MyPlayer.GetComponent<Animator>());
        
        MyPlayerEntity = new PlayerEntity();

        CameraController = new CameraController(MyPlayer, MyPlayerEntity);
        
        LogicStateMachine.Enter(MyPlayerEntity);
        
        
        AnimStateMachine.Enter(MyPlayerEntity);
    }

    public void Update()
    {
        CameraController.Update(MyPlayer,MyPlayerEntity);
        
        LogicStateMachine.Update(MyPlayerEntity, CharacterController);

        AnimStateMachine.UpdateAnim(MyPlayerEntity);
    }

}
