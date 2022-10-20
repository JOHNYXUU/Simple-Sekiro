using Entity;
using Manager;
using StateMachine.Base;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Logic.ArmStateMachine
{
    public class ArmEquip : PlayerLogicState
    {
        public override void Enter(PlayerEntity entity)
        {
            if(Config.showState)
                Debug.LogError("Equip");

            // MgrInput.LeftShiftHoldTime = 0f;
        }

        public override int Update(PlayerEntity entity, CharacterController cc)
        {
            if (entity.MoveState == (int)MoveState.Idle && Input.GetKey(KeyCode.Alpha1) && !MgrInput.Alpha1IsContinue)
                return (int)ArmState.Unequip;


            if (Input.GetMouseButton(1))
                return (int)ArmState.Defence;
            
            return -1;
        }
    }
}