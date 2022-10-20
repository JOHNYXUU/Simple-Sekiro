using Controllers;
using Entity;
using StateMachine.Anim.Data;
using StateMachine.Base;
using StateMachine.Enum.AnimEnum;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Anim.LockMove
{
    public class LockDodge : PlayerAnimState
    {
        public LockDodge()
        {
            EnterMap = new AnimParamMap();
            EnterMap.BoolValues.Add(AnimParams.IsDodge, true);
            EnterMap.BoolValues.Add(AnimParams.IsLock, true);
            EnterMap.FloatValues.Add(AnimParams.Forward2WantDirAngle, 0);
            
            LeaveMap = new AnimParamMap();
            LeaveMap.BoolValues.Add(AnimParams.IsDodge, false);
        }
        
        public override void Enter(PlayerEntity entity)
        {
            base.Enter(entity);
            
            if(Config.showAnimState)
                Debug.LogError("LockDodge");
        }

        public override int UpdateAnim(PlayerEntity entity)
        {
            if (entity.MoveState == (int)MoveState.Run && entity.CameraState == (int)CameraMoveType.Lock)
                return (int)AnimState.LockRun;

            if (entity.MoveState == (int)MoveState.Idle)
                return (int)AnimState.EquipIdle;

            if (entity.MoveState == (int)MoveState.Jump && entity.MoveJumpState == (int)MoveJumpState.JumpStart)
                return (int)AnimState.JumpStart;
            
            if (entity.MoveState == (int)MoveState.Sprint && entity.ArmState != (int)ArmState.Defence) 
                return (int)AnimState.FreeSprint;

            return -1;
        }

        public override void UpdateEnterParam(PlayerEntity entity)
        {
            EnterMap.FloatValues[AnimParams.Forward2WantDirAngle] = entity.Forward2WantDirAngle;
        }
    }
}