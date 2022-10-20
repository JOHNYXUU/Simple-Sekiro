using System.Runtime.CompilerServices;
using Controllers;
using Entity;
using StateMachine.Anim.Data;
using StateMachine.Base;
using StateMachine.Enum.AnimEnum;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Anim.Jump
{
    public class JumpLoop : PlayerAnimState
    {
        public JumpLoop()
        {
            EnterMap = new AnimParamMap();

            EnterMap.BoolValues.Add(AnimParams.InAir, true);
            EnterMap.BoolValues.Add(AnimParams.IsFalling, true);
            EnterMap.BoolValues.Add(AnimParams.IsLock,false);

            UpdateMap = new AnimParamMap();
            UpdateMap.BoolValues.Add(AnimParams.IsLock,false);
        }
        
        public override void Enter(PlayerEntity entity)
        {
            base.Enter(entity);
            
            if(Config.showAnimState)
                Debug.LogError("JumpLoop");
        }

        public override int UpdateAnim(PlayerEntity entity)
        {
            base.UpdateAnim(entity);

            // Debug.LogError("loop " + (MoveJumpState)entity.MoveJumpState);

            if (entity.MoveJumpState == (int)MoveJumpState.JumpEnd ||
                (entity.MoveState != (int)MoveState.Jump && entity.MoveState != (int)MoveState.Fall))
                return (int)AnimState.JumpEnd;
            
            
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