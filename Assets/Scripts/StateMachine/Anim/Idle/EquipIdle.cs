using Controllers;
using Entity;
using StateMachine.Anim.Data;
using StateMachine.Base;
using StateMachine.Enum.AnimEnum;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Anim.Idle
{
    public class EquipIdle : PlayerAnimState
    {
        public EquipIdle()
        {
            EnterMap = new AnimParamMap();
            EnterMap.BoolValues.Add(AnimParams.IsEquiped, true);
            EnterMap.BoolValues.Add(AnimParams.IsDefence, false);
            EnterMap.BoolValues.Add(AnimParams.IsMove, false);
            EnterMap.FloatValues.Add(AnimParams.SpeedValue, 0);
            EnterMap.BoolValues.Add(AnimParams.IsLock, false);
            

            UpdateMap = new AnimParamMap();
            UpdateMap.FloatValues.Add(AnimParams.SpeedValue, 0);
            UpdateMap.BoolValues.Add(AnimParams.IsLock, false);
        }
        
        public override void Enter(PlayerEntity entity)
        {
            base.Enter(entity);
            
            if(Config.showAnimState)
                Debug.LogError("EquipIdle");
        }

        public override int UpdateAnim(PlayerEntity entity)
        {
            base.UpdateAnim(entity);
            
            if (entity.ArmState == (int)ArmState.Unequip)
                return (int)AnimState.UnequipIdle;

            if (entity.ArmState == (int)ArmState.Defence)
                return (int)AnimState.DefenceIdle;

            if (entity.MoveState == (int)MoveState.Run && entity.CameraState == (int)CameraMoveType.Free)
                return (int)AnimState.FreeRun;

            if (entity.MoveState == (int)MoveState.Run && entity.CameraState == (int)CameraMoveType.Lock)
                return (int)AnimState.LockRun;

            if (entity.MoveState == (int)MoveState.Dodge)
                return (int)AnimState.FreeDodge;

            if (entity.MoveState == (int)MoveState.Jump && entity.MoveJumpState == (int)MoveJumpState.JumpStart)
                return (int)AnimState.JumpStart;

            if (entity.MoveState == (int)MoveState.Fall)
                return (int)AnimState.JumpLoop;

            if (entity.AttackState == (int)AttackState.LightAttack)
                return (int)AnimState.LightAttack01;

            if (entity.AttackState == (int)AttackState.HeavyAttack)
                return (int)AnimState.HeavyAttack;

            return -1;
        }

        public override void UpdateEnterParam(PlayerEntity entity)
        {
            var speedOnGround = new Vector2(entity.Speed.x, entity.Speed.z);

            EnterMap.FloatValues[AnimParams.SpeedValue] = speedOnGround.magnitude * 100f;
            
            EnterMap.BoolValues[AnimParams.IsLock] = entity.CameraState == (int)CameraMoveType.Lock;
        }

        public override void UpdateUpdateParam(PlayerEntity entity)
        {
            var speedOnGround = new Vector2(entity.Speed.x, entity.Speed.z);

            UpdateMap.FloatValues[AnimParams.SpeedValue] = speedOnGround.magnitude * 100f;

            UpdateMap.BoolValues[AnimParams.IsLock] = entity.CameraState == (int)CameraMoveType.Lock;
        }
    }
}