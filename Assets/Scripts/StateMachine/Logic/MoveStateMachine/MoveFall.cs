using Entity;
using Manager;
using StateMachine.Base;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Logic.MoveStateMachine
{
    public class MoveFall : PlayerLogicState
    {
        public override void Enter(PlayerEntity entity)
        {
            if(Config.showState)
                Debug.LogError("fall");

            entity.Speed.y = 0f;
        }

        public override int Update(PlayerEntity entity, CharacterController cc)
        {
            base.Update(entity, cc);

            entity.Speed.y -= Config.gravity * Time.deltaTime;

            var dis = entity.Speed * Time.deltaTime;

            var realDis =  Move(cc, entity, dis);
            
            entity.Speed = FixSpeed(realDis, Time.deltaTime, entity.Speed);

            // if (cc.isGrounded && HasMove() && Input.GetKey(KeyCode.LeftShift) &&
            //     MgrInput.LeftShiftHoldTime >= Config.pressClickBoundary  && IsMoveForward(cc, entity))
            //     return (int)MoveState.Sprint;
            //
            // if (cc.isGrounded && !Input.GetKey(KeyCode.LeftShift) &&
            //     MgrInput.LeftShiftHoldTime < Config.pressClickBoundary && MgrInput.LeftShiftHoldTime > 0f)
            //     return (int)MoveState.Dodge;
            
            if(Input.GetKey(KeyCode.LeftShift) && !MgrInput.LeftShiftContinue && entity.ArmState != (int)ArmState.Defence)
                return (int)MoveState.Dodge;

            if (cc.isGrounded && HasMove())
                return (int)MoveState.Run;

            if (cc.isGrounded && !HasMove())
                return (int)MoveState.Idle;

            if (cc.isGrounded && Jump() && entity.ArmState == (int)ArmState.Equip)
                return (int)MoveState.Jump;
            
            return -1;
        }

        public override void Leave(PlayerEntity entity)
        {
            entity.Speed.y = 0f;
        }
    }
}