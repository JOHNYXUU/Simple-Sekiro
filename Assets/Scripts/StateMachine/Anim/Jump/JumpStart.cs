using Controllers;
using Entity;
using StateMachine.Anim.Data;
using StateMachine.Base;
using StateMachine.Enum.AnimEnum;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Anim.Jump
{
    public class JumpStart : PlayerAnimState
    {
        public JumpStart()
        {
            EnterMap = new AnimParamMap();

            EnterMap.BoolValues.Add(AnimParams.InAir, true);
            EnterMap.BoolValues.Add(AnimParams.IsLock,false);
        }
        
        public override void Enter(PlayerEntity entity)
        {
            base.Enter(entity);
            
            if(Config.showAnimState)
                Debug.LogError("JumpStart");
        }

        public override int UpdateAnim(PlayerEntity entity)
        {
            base.UpdateAnim(entity);
            
            // Debug.LogError("start " + (MoveJumpState)entity.MoveJumpState + " " + entity.Speed.y);
            if (entity.MoveJumpState == (int)MoveJumpState.JumpLoop && entity.Speed.y <= 0f)
                return (int)AnimState.JumpLoop;

            if (entity.MoveJumpState == (int)MoveJumpState.JumpEnd)
                return (int)AnimState.JumpEnd;
            
            return -1;
        }

        public override void UpdateEnterParam(PlayerEntity entity)
        {
            EnterMap.BoolValues[AnimParams.IsLock] = entity.CameraState == (int)CameraMoveType.Lock;
        }
    }
}