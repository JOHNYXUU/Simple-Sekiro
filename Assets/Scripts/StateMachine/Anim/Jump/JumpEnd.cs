using Controllers;
using Entity;
using StateMachine.Anim.Data;
using StateMachine.Base;
using StateMachine.Enum.AnimEnum;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Anim.Jump
{
    public class JumpEnd : PlayerAnimState
    {
        public JumpEnd()
        {
            EnterMap = new AnimParamMap();

            EnterMap.BoolValues.Add(AnimParams.IsFalling, false);
            EnterMap.BoolValues.Add(AnimParams.InAir, false);
            EnterMap.BoolValues.Add(AnimParams.IsLock,false);

            UpdateMap = new AnimParamMap();
            UpdateMap.BoolValues.Add(AnimParams.IsLock,false);
        }
        
        public override void Enter(PlayerEntity entity)
        {
            base.Enter(entity);
            
            if(Config.showAnimState)
                Debug.LogError("JumpEnd");
        }

        public override int UpdateAnim(PlayerEntity entity)
        {
            base.UpdateAnim(entity);
            
            // Debug.LogError("end " + (MoveJumpState)entity.MoveJumpState);

            if (entity.MoveState == (int)MoveState.Jump && entity.MoveJumpState == (int)MoveJumpState.JumpStart)
                return (int)AnimState.JumpStart;

            if (entity.MoveState == (int)MoveState.Idle)
                return (int)AnimState.EquipIdle;

            if (entity.MoveState == (int)MoveState.Run && entity.CameraState == (int)CameraMoveType.Free)
                return (int)AnimState.FreeRun;
            
            if (entity.MoveState == (int)MoveState.Run && entity.CameraState == (int)CameraMoveType.Lock)
                return (int)AnimState.LockRun;

            if (entity.MoveState == (int)MoveState.Dodge && entity.CameraState == (int)CameraMoveType.Free)
                return (int)AnimState.FreeDodge;
            
            if (entity.MoveState == (int)MoveState.Dodge && entity.CameraState == (int)CameraMoveType.Lock)
                return (int)AnimState.LockDodge;
            
            return -1;
        }
        
        public override void UpdateEnterParam(PlayerEntity entity)
        {
            EnterMap.BoolValues[AnimParams.IsLock] = entity.CameraState == (int)CameraMoveType.Lock;
        }

        public override void UpdateUpdateParam(PlayerEntity entity)
        {
            UpdateMap.BoolValues[AnimParams.IsLock] = entity.CameraState == (int)CameraMoveType.Lock;
        }
    }
}