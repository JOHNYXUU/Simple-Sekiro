using System.Runtime.CompilerServices;
using Entity;
using StateMachine.Anim.Data;
using StateMachine.Base;
using StateMachine.Enum.AnimEnum;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Anim.Attack
{
    public class HeavyAttack : PlayerAnimState
    {
        private float _enterTime;

        private bool _hasPlayAudio;
        
        public HeavyAttack()
        {
            EnterMap = new AnimParamMap();

            EnterMap.BoolValues.Add(AnimParams.IsLightAttack, false);
            EnterMap.BoolValues.Add(AnimParams.IsHeavyAttack, true);

            LeaveMap = new AnimParamMap();
            LeaveMap.BoolValues.Add(AnimParams.IsHeavyAttack, false);
        }
        
        public override void Enter(PlayerEntity entity)
        {
            base.Enter(entity);
            
            if(Config.showAnimState)
                Debug.LogError("HeavyAttack");
            
            _enterTime = Time.time;

            _hasPlayAudio = false;
        }
        
        public override int UpdateAnim(PlayerEntity entity)
        {
            base.UpdateAnim(entity);
            
            if (Time.time - _enterTime >= Config.heavyAttackAudioBeginTime && !_hasPlayAudio)
            {
                GameLoop.MgrAudio.SetAudioClip(
                    GameLoop.MgrAudio.GetRandomAudioClip(Config.bladeWooshAudioPath, Config.bladeWooshAudioNum));
                GameLoop.MgrAudio.Play();
                _hasPlayAudio = true;
            }

            // if (entity.AttackState == (int)AttackState.HeavyAttack && entity.HeavyAttackTime - entity.LastHeavyAttackTime > Config.heavyAttackNextTime)
            //     return (int)AnimState.HeavyAttack;
            
            if (entity.AttackState == (int)AttackState.LightAttack)
                return (int)AnimState.LightAttack01;

            if (entity.AttackState == (int)AttackState.NoAttack)
                return (int)AnimState.EquipIdle;

            if (entity.MoveState == (int)MoveState.Run)
                return (int)AnimState.FreeRun;
            
            return -1;
        }

        public override void UpdateEnterParam(PlayerEntity entity)
        {
            // EnterMap.BoolValues[AnimParams.HasNextHeavyAttack] = entity.HeavyAttackNum > entity.LastHeavyAttackNum;
        }
    }
}