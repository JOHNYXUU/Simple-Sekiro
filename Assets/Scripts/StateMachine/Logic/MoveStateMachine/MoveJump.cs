using Entity;
using Manager;
using StateMachine.Enum.LogicEnum;
using StateMachine.Logic.MoveStateMachine.MoveJumpStateMachine;
using UnityEngine;

namespace StateMachine.Logic.MoveStateMachine
{
    public class MoveJump : PlayerLogicHierarchyState
    {
        public override void Enter(PlayerEntity entity)
        {
            if(Config.showState)
                Debug.LogError("Jump");

            ChangeState((int)MoveJumpState.JumpStart, entity);
        }

        public MoveJump()
        {
            Regist((int)MoveJumpState.JumpStart,new MoveJumpStart());
            Regist((int)MoveJumpState.JumpLoop,new MoveJumpLoop());
            Regist((int)MoveJumpState.JumpEnd,new MoveJumpEnd());
        }

        public override int Update(PlayerEntity entity, CharacterController cc)
        {
            base.Update(entity, cc);

            // if (cc.isGrounded && HasMove() && Input.GetKey(KeyCode.LeftShift) &&
            //     MgrInput.LeftShiftHoldTime >= Config.pressClickBoundary  && IsMoveForward(cc, entity))
            //     return (int)MoveState.Sprint;
            //
            // if (cc.isGrounded && !Input.GetKey(KeyCode.LeftShift) &&
            //     MgrInput.LeftShiftHoldTime < Config.pressClickBoundary && MgrInput.LeftShiftHoldTime > 0f )
            //     return (int)MoveState.Dodge;
            
            
            if (cc.isGrounded && Input.GetKey(KeyCode.LeftShift) && !MgrInput.LeftShiftContinue &&  entity.ArmState != (int)ArmState.Defence)
                return (int)MoveState.Dodge;

            if (cc.isGrounded && HasMove())
                return (int)MoveState.Run;

            if (cc.isGrounded && (!HasMove() || entity.MoveBlock > 0))
                return (int)MoveState.Idle;

            return -1;
        }
        
        public override void ChangeState(int nextState, PlayerEntity entity)
        {
            base.ChangeState(nextState, entity);
            
            if(nextState == -1 || !SubStates.ContainsKey(nextState))
                return;

            entity.MoveJumpState = nextState;
        }
    }
}