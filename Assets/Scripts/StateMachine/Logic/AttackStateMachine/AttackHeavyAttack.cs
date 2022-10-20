using Entity;
using Manager;
using StateMachine.Base;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Logic.AttackStateMachine
{
    public class AttackHeavyAttack : PlayerLogicState
    {
        private float _enterTime;

        private bool _hasNextAttack;

        private bool _hasClick;
        
        public override void Enter(PlayerEntity entity)
        {
            if(Config.showState)
                Debug.LogError("HeavyAttack");
            
            MgrInput.LeftMouseButtonHoldTime = 0f;

            _enterTime = Time.time;

            _hasNextAttack = false;

            _hasClick = false;

            entity.MoveBlock++;
        }
        
        public override int Update(PlayerEntity entity, CharacterController cc)
        {
            var nowTime = Time.time;

            if (Input.GetMouseButton(0) && !MgrInput.LeftMouseButtonContinue && !_hasNextAttack)
            {
                _hasNextAttack = true;

                MgrInput.LeftMouseButtonHoldTime = 0f;
            }

            if (!Input.GetMouseButton(0) && _hasNextAttack)
                _hasClick = true;

            if (_hasClick && Input.GetMouseButton(0))
                MgrInput.LeftMouseButtonHoldTime -= Time.deltaTime;

            if (_hasNextAttack && nowTime - _enterTime >= Config.heavyAttackNextTime && !Input.GetMouseButton(0))
            {
                if (MgrInput.LeftMouseButtonHoldTime > 0f &&
                    MgrInput.LeftMouseButtonHoldTime < Config.pressClickBoundary)
                    return (int)AttackState.LightAttack;

                if (MgrInput.LeftMouseButtonHoldTime > Config.pressClickBoundary)
                    return (int)AttackState.HeavyAttack;
            }

            if (nowTime - _enterTime < Config.heavyAttackEndTime)
                return -1;
            
            if (_hasNextAttack && MgrInput.LeftMouseButtonHoldTime > 0f &&
                MgrInput.LeftMouseButtonHoldTime < Config.pressClickBoundary)
                return (int)AttackState.LightAttack;

            if (_hasNextAttack && MgrInput.LeftMouseButtonHoldTime > Config.pressClickBoundary)
                return (int)AttackState.HeavyAttack;
            

            return (int)AttackState.NoAttack;
        }
        
        public override void Leave(PlayerEntity entity)
        {
            MgrInput.LeftMouseButtonHoldTime = 0f;

            entity.MoveBlock--;
        }
    }
}