using Entity;
using Manager;
using StateMachine.Base;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Logic.ArmStateMachine
{
    public class ArmUnequip : PlayerLogicState
    {
        public override void Enter(PlayerEntity entity)
        {
            if(Config.showState)
                Debug.LogError("Unequip");

            entity.MoveBlock++;
            entity.AttackBlock++;
        }

        public override int Update(PlayerEntity entity, CharacterController cc)
        {
            // Debug.LogError(entity.MoveState == (int)MoveState.Idle && Input.GetKey(KeyCode.Alpha1) && !MgrInput.Alpha1IsContinue);
            if (entity.MoveState == (int)MoveState.Idle && Input.GetKey(KeyCode.Alpha1) && !MgrInput.Alpha1IsContinue)
                return (int)ArmState.Equip;
            
            return -1;
        }

        public override void Leave(PlayerEntity entity)
        {
            entity.MoveBlock--;
            entity.AttackBlock--;
        }
    }
}