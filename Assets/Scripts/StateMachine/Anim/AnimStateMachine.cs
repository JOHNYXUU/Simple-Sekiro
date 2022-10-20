using Entity;
using StateMachine.Anim.Attack;
using StateMachine.Anim.DefenceMove;
using StateMachine.Anim.FreeMove;
using StateMachine.Anim.Idle;
using StateMachine.Anim.Jump;
using StateMachine.Anim.LockMove;
using StateMachine.Base;
using StateMachine.Enum.AnimEnum;

namespace StateMachine.Anim
{
    public class AnimStateMachine : PlayerAnimHierarchyState
    {
        public AnimStateMachine()
        {
            Regist((int)AnimState.UnequipIdle, new UnequipIdle());
            Regist((int)AnimState.EquipIdle, new EquipIdle());
            Regist((int)AnimState.DefenceIdle, new DefenceIdle());
            Regist((int)AnimState.DefenceRun, new DefenceRun());
            Regist((int)AnimState.FreeRun, new FreeRun());
            Regist((int)AnimState.FreeRunStop, new FreeRunStop());
            Regist((int)AnimState.FreeSprint, new FreeSprint());
            Regist((int)AnimState.FreeDodge, new FreeDodge());
            Regist((int)AnimState.JumpStart, new JumpStart());
            Regist((int)AnimState.JumpLoop, new JumpLoop());
            Regist((int)AnimState.JumpEnd, new JumpEnd());
            Regist((int)AnimState.LightAttack01, new LightAttack01());
            Regist((int)AnimState.LightAttack02, new LightAttack02());
            Regist((int)AnimState.LightAttack03, new LightAttack03());
            Regist((int)AnimState.LightAttack04, new LightAttack04());
            Regist((int)AnimState.LightAttack05, new LightAttack05());
            Regist((int)AnimState.LightAttack06, new LightAttack06());
            Regist((int)AnimState.HeavyAttack, new HeavyAttack());
            Regist((int)AnimState.LockRun, new LockRun());
            Regist((int)AnimState.LockDodge, new LockDodge());
        }

        public override void Enter(PlayerEntity entity)
        {
            ChangeState((int)AnimState.UnequipIdle, entity);
        }

        public override void ChangeState(int nextState, PlayerEntity entity)
        {
            base.ChangeState(nextState, entity);
            
            if(nextState == -1 || !SubStates.ContainsKey(nextState))
                return;

            entity.AnimState = nextState;
        }
    }
}