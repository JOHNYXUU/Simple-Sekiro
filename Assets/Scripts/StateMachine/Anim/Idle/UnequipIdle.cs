using Entity;
using StateMachine.Anim.Data;
using StateMachine.Base;
using StateMachine.Enum.AnimEnum;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Anim.Idle
{
    public class UnequipIdle : PlayerAnimState
    {
        public UnequipIdle()
        {
            EnterMap = new AnimParamMap();
            EnterMap.BoolValues.Add(AnimParams.IsEquiped, false);
        }

        public override void Enter(PlayerEntity entity)
        {
            base.Enter(entity);
            
            if(Config.showAnimState)
                Debug.LogError("UnequipIdle");
        }
        
        public override int UpdateAnim(PlayerEntity entity)
        {
            if (entity.ArmState == (int)ArmState.Equip)
                return (int)AnimState.EquipIdle;

            return -1;
        }
    }
}