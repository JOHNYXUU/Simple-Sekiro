using Entity;
using StateMachine.Enum.LogicEnum;

namespace StateMachine.Logic
{
    public class LogicStateMachine : PlayerLogicParallelState
    {
        public LogicStateMachine()
        {
            Regist((int)LogicState.Move,new MoveStateMachine.MoveStateMachine());
            Regist((int)LogicState.Arm,new ArmStateMachine.ArmStateMachine());
            Regist((int)LogicState.Attack,new AttackStateMachine.AttackStateMachine());
            Regist((int)LogicState.Pose,new PoseStateMachine.PoseStateMachine());
        }
    }
}