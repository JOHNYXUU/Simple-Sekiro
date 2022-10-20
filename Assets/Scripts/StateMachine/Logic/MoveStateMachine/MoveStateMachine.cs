using Entity;
using StateMachine.Enum.LogicEnum;

namespace StateMachine.Logic.MoveStateMachine
{
    public class MoveStateMachine : PlayerLogicHierarchyState
    {
        public MoveStateMachine()
        {
            Regist((int)MoveState.Idle,new MoveIdle());
            Regist((int)MoveState.Run,new MoveRun());
            Regist((int)MoveState.Sprint,new MoveSprint());
            Regist((int)MoveState.Dodge,new MoveDodge());
            Regist((int)MoveState.Jump,new MoveJump());
            Regist((int)MoveState.Fall,new MoveFall());
        }

        public override void Enter(PlayerEntity entity)
        {
            ChangeState((int)MoveState.Idle, entity);
        }

        public override void ChangeState(int nextState, PlayerEntity entity)
        {
            base.ChangeState(nextState, entity);
            
            if(nextState == -1 || !SubStates.ContainsKey(nextState))
                return;

            entity.MoveState = nextState;
        }
    }
}
