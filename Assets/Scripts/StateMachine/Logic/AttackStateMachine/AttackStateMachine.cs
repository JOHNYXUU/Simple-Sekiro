using Entity;
using StateMachine.Enum.LogicEnum;

namespace StateMachine.Logic.AttackStateMachine
{
    public class AttackStateMachine : PlayerLogicHierarchyState
    {
        public AttackStateMachine()
        {
            Regist((int)AttackState.NoAttack,new AttackNoAttack());
            Regist((int)AttackState.HeavyAttack,new AttackHeavyAttack());
            Regist((int)AttackState.LightAttack,new AttackLightAttack());
        }

        public override void Enter(PlayerEntity entity)
        {
            ChangeState((int)AttackState.NoAttack, entity);
        }
        
        public override void ChangeState(int nextState, PlayerEntity entity)
        {
            base.ChangeState(nextState, entity);
            
            if(nextState == -1 || !SubStates.ContainsKey(nextState))
                return;

            entity.AttackState = nextState;
        }
    }
}