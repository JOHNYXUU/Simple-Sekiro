using Entity;
using StateMachine.Anim.Data;
using StateMachine.Base;
using StateMachine.Enum.AnimEnum;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Anim.FreeMove
{
    public class FreeDodge : PlayerAnimState
    {
        public FreeDodge()
        {
            EnterMap = new AnimParamMap();
            EnterMap.BoolValues.Add(AnimParams.IsDodge, true);
            EnterMap.BoolValues.Add(AnimParams.IsMove, true);

            LeaveMap = new AnimParamMap();
            LeaveMap.BoolValues.Add(AnimParams.IsDodge, false);
        }

        public override void Enter(PlayerEntity entity)
        {
            base.Enter(entity);
            
            if(Config.showAnimState)
                Debug.LogError("FreeDodge");
        }

        public override int UpdateAnim(PlayerEntity entity)
        {
            base.UpdateAnim(entity);
            
            if (entity.MoveState == (int)MoveState.Idle && entity.AttackState == (int)AttackState.NoAttack)
                return (int)AnimState.EquipIdle;
            
            if (entity.MoveState == (int)MoveState.Idle && entity.AttackState == (int)AttackState.LightAttack)
                return (int)AnimState.LightAttack01;

            if (entity.MoveState == (int)MoveState.Run)
                return (int)AnimState.FreeRun;
            
            if (entity.MoveState == (int)MoveState.Sprint && entity.ArmState == (int)ArmState.Defence) 
                return (int)AnimState.FreeRun;

            if (entity.MoveState == (int)MoveState.Sprint && entity.ArmState != (int)ArmState.Defence) 
                return (int)AnimState.FreeSprint;
            
            if (entity.MoveState == (int)MoveState.Jump && entity.MoveJumpState == (int)MoveJumpState.JumpStart)
                return (int)AnimState.JumpStart;
            
            return -1;
        }
    }
}