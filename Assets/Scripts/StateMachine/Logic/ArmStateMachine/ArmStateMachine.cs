using System;
using Entity;
using StateMachine.Enum.LogicEnum;

namespace StateMachine.Logic.ArmStateMachine
{
    public class ArmStateMachine : PlayerLogicHierarchyState
    {
        public ArmStateMachine()
        {
            Regist((int)ArmState.Unequip,new ArmUnequip());
            Regist((int)ArmState.Equip,new ArmEquip());
            Regist((int)ArmState.Defence,new ArmDefence());
        }

        public override void Enter(PlayerEntity entity)
        {
            ChangeState((int)ArmState.Unequip, entity);
        }
        
        public override void ChangeState(int nextState, PlayerEntity entity)
        {
            base.ChangeState(nextState, entity);
            
            if(nextState == -1 || !SubStates.ContainsKey(nextState))
                return;

            entity.ArmState = nextState;
        }
    }
}