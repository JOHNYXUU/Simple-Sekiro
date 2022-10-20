using Entity;
using StateMachine.Base;
using UnityEngine;

namespace StateMachine.Logic.MoveStateMachine.MoveJumpStateMachine
{
    public class MoveJumpEnd : PlayerLogicState
    {
        public override void Enter(PlayerEntity entity)
        {
            if(Config.showState)
                Debug.LogError("Jump End");

            entity.Speed.y = 0f;
        }
    }
}