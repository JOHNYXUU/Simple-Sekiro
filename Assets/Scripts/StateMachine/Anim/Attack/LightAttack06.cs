using Entity;
using StateMachine.Anim.Data;
using StateMachine.Base;
using StateMachine.Enum.AnimEnum;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Anim.Attack
{
    public class LightAttack06 : PlayerAnimState
    {
        private float _enterTime;

        private bool _hasPlayAudio;
        
        public LightAttack06()
        {
            EnterMap = new AnimParamMap();

            EnterMap.BoolValues.Add(AnimParams.IsLightAttack, true);
            EnterMap.BoolValues.Add(AnimParams.IsHeavyAttack, false);

            EnterMap.IntValues.Add(AnimParams.LightAttackIndex, 6);
            
            LeaveMap = new AnimParamMap();
            
            LeaveMap.BoolValues.Add(AnimParams.IsLightAttack, false);
        }
        
        public override void Enter(PlayerEntity entity)
        {
            base.Enter(entity);
            
            if(Config.showAnimState)
                Debug.LogError("LightAttack06");
            
            _enterTime = Time.time;

            _hasPlayAudio = false;
        }
        
        public override int UpdateAnim(PlayerEntity entity)
        {
            base.UpdateAnim(entity);
            
            if (Time.time - _enterTime >= Config.lightAttack06AudioBeginTime && !_hasPlayAudio)
            {
                GameLoop.MgrAudio.SetAudioClip(
                    GameLoop.MgrAudio.GetRandomAudioClip(Config.bladeWooshAudioPath, Config.bladeWooshAudioNum));
                GameLoop.MgrAudio.Play();
                _hasPlayAudio = true;
            }

            if (entity.AttackState == (int)AttackState.LightAttack && entity.LightAttackIndex == 1)
                return (int)AnimState.LightAttack01;

            if (entity.AttackState == (int)AttackState.HeavyAttack)
                return (int)AnimState.HeavyAttack;

            if (entity.AttackState == (int)AttackState.NoAttack)
                return (int)AnimState.EquipIdle;

            return -1;
        }
    }
}