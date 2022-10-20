using Entity;
using StateMachine.Base;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Logic.ArmStateMachine
{
    public class ArmDefence : PlayerLogicState
    {
        public override void Enter(PlayerEntity entity)
        {
            if(Config.showState)
                Debug.LogError("Defence");

            entity.SlowDownPer = 0.5f;
        }

        public override int Update(PlayerEntity entity, CharacterController cc)
        {
            if (!Input.GetMouseButton(1))
                return (int)ArmState.Equip;
            
            return -1;
        }

        public override void Leave(PlayerEntity entity)
        {
            entity.SlowDownPer = 0f;
        }
    }
}