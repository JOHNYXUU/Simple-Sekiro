using System.Runtime.CompilerServices;
using Entity;
using StateMachine.Anim.Data;
using StateMachine.Base;
using StateMachine.Enum.AnimEnum;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Anim.FreeMove
{
    public class FreeRunStop : PlayerAnimState
    {
        private float _enterTime;
        
        public FreeRunStop()
        {
            EnterMap = new AnimParamMap();
            
            EnterMap.BoolValues.Add(AnimParams.IsMove,false);
            EnterMap.BoolValues.Add(AnimParams.IsDodge,false);
            EnterMap.BoolValues.Add(AnimParams.IsFalling,false);
            EnterMap.BoolValues.Add(AnimParams.IsSprint,false);
            EnterMap.BoolValues.Add(AnimParams.InAir,false);
            EnterMap.FloatValues.Add(AnimParams.SpeedValue, 0f);

            UpdateMap = new AnimParamMap();
            UpdateMap.FloatValues.Add(AnimParams.SpeedValue, 0f);
        }

        public override void Enter(PlayerEntity entity)
        {
            base.Enter(entity);

            if(Config.showAnimState)
                Debug.LogError("freeRunStop");
            
            _enterTime = Time.time;
        }

        public override int UpdateAnim(PlayerEntity entity)
        {
            base.UpdateAnim(entity);

            float nowTime = Time.time;

            if (entity.MoveState == (int)MoveState.Jump && entity.MoveJumpState == (int)MoveJumpState.JumpStart)
                return (int)AnimState.JumpStart;

            if (entity.MoveState == (int)MoveState.Run)
                return (int)AnimState.FreeRun;
            
            if (entity.MoveState == (int)MoveState.Dodge)
                return (int)AnimState.FreeDodge;

            if (nowTime - _enterTime >= 0.7222222f)
                return (int)AnimState.EquipIdle;
            
            if (entity.MoveState == (int)MoveState.Idle && entity.AttackState == (int)AttackState.LightAttack)
                return (int)AnimState.LightAttack01;
            
            return -1;
        }

        public override void UpdateEnterParam(PlayerEntity entity)
        {
            var speedOnGround = new Vector2(entity.Speed.x, entity.Speed.z);

            EnterMap.FloatValues[AnimParams.SpeedValue] = speedOnGround.magnitude * 100f;
        }

        public override void UpdateUpdateParam(PlayerEntity entity)
        {
            var speedOnGround = new Vector3(entity.Speed.x, 0f,entity.Speed.z);

            UpdateMap.FloatValues[AnimParams.SpeedValue] = speedOnGround.magnitude * 100f;
        }
    }
}