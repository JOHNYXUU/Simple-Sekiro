using Entity;
using StateMachine.Enum.LogicEnum;

namespace StateMachine.Logic.PoseStateMachine
{
    public class PoseStateMachine: PlayerLogicHierarchyState
    {
        public PoseStateMachine()
        {
            Regist((int)PoseState.Stand,new PoseStand());
            Regist((int)PoseState.Crouch,new PoseCrouch());
        }

        public override void Enter(PlayerEntity entity)
        {
            ChangeState((int)PoseState.Stand, entity);
        }
        
        public override void ChangeState(int nextState, PlayerEntity entity)
        {
            base.ChangeState(nextState, entity);
            
            if(nextState == -1 || !SubStates.ContainsKey(nextState))
                return;

            entity.PoseState = nextState;
        }
    }
}