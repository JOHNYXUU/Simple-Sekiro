using Controllers;
using Entity;
using StateMachine.Anim.Data;
using StateMachine.Base;
using StateMachine.Enum.AnimEnum;
using StateMachine.Enum.LogicEnum;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace StateMachine.Anim.FreeMove
{
    public class FreeRun : PlayerAnimState
    {
        public FreeRun()
        {
            EnterMap = new AnimParamMap();
            EnterMap.FloatValues.Add(AnimParams.SpeedValue, 0);
            EnterMap.FloatValues.Add(AnimParams.Forward2WantDirAngle, 0);
            EnterMap.BoolValues.Add(AnimParams.IsMove, true);
            EnterMap.BoolValues.Add(AnimParams.IsDefence, false);
            EnterMap.BoolValues.Add(AnimParams.IsLock, false);
            
            UpdateMap = new AnimParamMap();
            UpdateMap.FloatValues.Add(AnimParams.SpeedValue, 0);
            UpdateMap.FloatValues.Add(AnimParams.Forward2WantDirAngle, 0);
        }
        
        public override void Enter(PlayerEntity entity)
        {
            base.Enter(entity);
            
            if(Config.showAnimState)
                Debug.LogError("FreeRun");
        }

        public override int UpdateAnim(PlayerEntity entity)
        {
            base.UpdateAnim(entity);
            
            var speedOnGround = new Vector3(entity.Speed.x, 0f,entity.Speed.z);

            if (entity.ArmState == (int)ArmState.Defence && entity.MoveState == (int)MoveState.Run)
                return (int)AnimState.DefenceRun;

            if (entity.MoveState == (int)MoveState.Idle && speedOnGround.magnitude * 100f > 95f &&
                entity.AttackState == (int)AttackState.NoAttack)
                return (int)AnimState.FreeRunStop;

            if (entity.MoveState == (int)MoveState.Idle && entity.AttackState == (int)AttackState.LightAttack)
                return (int)AnimState.LightAttack01;
            
            if (entity.MoveState == (int)MoveState.Idle && speedOnGround.magnitude * 100f < 95f &&
                entity.AttackState == (int)AttackState.NoAttack)
                return (int)AnimState.EquipIdle;
            
            if (entity.MoveState == (int)MoveState.Idle && entity.AttackState == (int)AttackState.HeavyAttack)
                return (int)AnimState.HeavyAttack;

            if (entity.MoveState == (int)MoveState.Dodge)
                return (int)AnimState.FreeDodge;
            
            if (entity.MoveState == (int)MoveState.Jump && entity.MoveJumpState == (int)MoveJumpState.JumpStart)
                return (int)AnimState.JumpStart;

            if (entity.MoveState == (int)MoveState.Fall)
                return (int)AnimState.JumpLoop;
            
            if (entity.MoveState == (int)MoveState.Run && entity.CameraState == (int)CameraMoveType.Lock)
                return (int)AnimState.LockRun;
            
            return -1;
        }

        public override void UpdateEnterParam(PlayerEntity entity)
        {
            var speedOnGround = new Vector2(entity.Speed.x, entity.Speed.z);

            EnterMap.FloatValues[AnimParams.SpeedValue] = speedOnGround.magnitude * 100f;
            EnterMap.FloatValues[AnimParams.Forward2WantDirAngle] = entity.Forward2WantDirAngle;
        }

        public override void UpdateUpdateParam(PlayerEntity entity)
        {
            var speedOnGround = new Vector3(entity.Speed.x, 0f,entity.Speed.z);

            UpdateMap.FloatValues[AnimParams.SpeedValue] = speedOnGround.magnitude * 100f;
            UpdateMap.FloatValues[AnimParams.Forward2WantDirAngle] = entity.Forward2WantDirAngle;
        }
    }
}