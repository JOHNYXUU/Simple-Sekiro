using Entity;
using Manager;
using StateMachine.Base;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Logic.PoseStateMachine
{
    public class PoseStand : PlayerLogicState
    {
        public override void Enter(PlayerEntity entity)
        {
            if(Config.showState)
                Debug.LogError("stand");
        }

        public override int Update(PlayerEntity entity, CharacterController cc)
        {
            if (Input.GetKey(KeyCode.Q) && !MgrInput.QContinue)
                return (int)PoseState.Crouch;
            
            return -1;
        }
    }
}