using Entity;
using StateMachine.Base;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Logic.MoveStateMachine.MoveJumpStateMachine
{
    public class MoveJumpStart : PlayerLogicState
    {
        public override void Enter(PlayerEntity entity)
        {
            if(Config.showState)
                Debug.LogError("Enter Jump");

            entity.Speed.y = Config.jumpStartSpeed;
        }

        public override int Update(PlayerEntity entity, CharacterController cc)
        {
            entity.Speed.y -= Config.gravity * Time.deltaTime;

            var dis = entity.Speed * Time.deltaTime;

            var realDis =  Move(cc, entity, dis);
            
            entity.Speed = FixSpeed(realDis, Time.deltaTime, entity.Speed);
            
            return (int)MoveJumpState.JumpLoop;
        }
    }
}