using Entity;
using Manager;
using StateMachine.Base;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Logic.AttackStateMachine
{
    public class AttackNoAttack : PlayerLogicState
    {
        public override void Enter(PlayerEntity entity)
        {
            if(Config.showState)
                Debug.LogError("NoAttack");

            entity.LightAttackIndex = 1;
        }

        public override int Update(PlayerEntity entity, CharacterController cc)
        {
            if (entity.AttackBlock > 0)
                return -1;

            if (!Input.GetMouseButton(0) && MgrInput.LeftMouseButtonHoldTime > 0f &&
                MgrInput.LeftMouseButtonHoldTime < Config.pressClickBoundary)
                return (int)AttackState.LightAttack;

            if (Input.GetMouseButton(0) && MgrInput.LeftMouseButtonHoldTime > Config.pressClickBoundary)
                return (int)AttackState.HeavyAttack;

            
            return -1;
        }
    }
}