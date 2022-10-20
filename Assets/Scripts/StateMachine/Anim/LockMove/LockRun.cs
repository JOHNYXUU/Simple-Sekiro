using Controllers;
using Entity;
using StateMachine.Anim.Data;
using StateMachine.Base;
using StateMachine.Enum.AnimEnum;
using StateMachine.Enum.LogicEnum;
using UnityEngine;
using UnityEngine.UIElements;

namespace StateMachine.Anim.LockMove
{
    public class LockRun : PlayerAnimState
    {
        public LockRun()
        {
            EnterMap = new AnimParamMap();
            EnterMap.FloatValues.Add(AnimParams.SpeedX, 0);
            EnterMap.FloatValues.Add(AnimParams.SpeedZ, 0);
            EnterMap.BoolValues.Add(AnimParams.IsMove, true);
            EnterMap.BoolValues.Add(AnimParams.IsDefence, false);
            EnterMap.BoolValues.Add(AnimParams.IsLock, true);
            EnterMap.FloatValues.Add(AnimParams.Forward2WantDirAngle, 0);
            
            UpdateMap = new AnimParamMap();
            UpdateMap.FloatValues.Add(AnimParams.SpeedX, 0);
            UpdateMap.FloatValues.Add(AnimParams.SpeedZ, 0);
            UpdateMap.FloatValues.Add(AnimParams.Forward2WantDirAngle, 0);
        }
        
        public override void Enter(PlayerEntity entity)
        {
            base.Enter(entity);
            
            if(Config.showAnimState)
                Debug.LogError("LockRun");
        }

        public override int UpdateAnim(PlayerEntity entity)
        {
            base.UpdateAnim(entity);
            
            var speedOnGround = new Vector3(entity.Speed.x, 0f,entity.Speed.z);

            if (entity.MoveState == (int)MoveState.Idle && entity.AttackState == (int)AttackState.NoAttack )
                return (int)AnimState.EquipIdle;
            
            if (entity.MoveState == (int)MoveState.Idle && entity.AttackState == (int)AttackState.LightAttack)
                return (int)AnimState.LightAttack01;
            
            if (entity.MoveState == (int)MoveState.Idle && entity.AttackState == (int)AttackState.HeavyAttack)
                return (int)AnimState.HeavyAttack;
            
            if (entity.MoveState == (int)MoveState.Jump && entity.MoveJumpState == (int)MoveJumpState.JumpStart)
                return (int)AnimState.JumpStart;

            if (entity.MoveState == (int)MoveState.Run && entity.CameraState == (int)CameraMoveType.Free)
                return (int)AnimState.FreeRun;
            
            if (entity.MoveState == (int)MoveState.Run && entity.ArmState == (int)ArmState.Defence)
                return (int)AnimState.DefenceRun;

            if (entity.MoveState == (int)MoveState.Fall)
                return (int)AnimState.JumpLoop;
            
            if (entity.MoveState == (int)MoveState.Idle && entity.AttackState == (int)AttackState.LightAttack)
                return (int)AnimState.LightAttack01;
            
            if (entity.MoveState == (int)MoveState.Dodge)
                return (int)AnimState.LockDodge;
            
            // if (entity.MoveState == (int)MoveState.Dodge)
            //     return (int)AnimState.FreeDodge;

            return -1;
        }

        public override void UpdateEnterParam(PlayerEntity entity)
        {
            var speedOnGround = new Vector3(entity.Speed.x, 0,entity.Speed.z);

            var forward = Quaternion.Euler(0, entity.ViewYaw, 0) * Vector3.forward;

            var localSpeed = MapWordSpeed2LockSpeed(speedOnGround, forward.normalized);
            

            EnterMap.FloatValues[AnimParams.SpeedX] = localSpeed.x;
            EnterMap.FloatValues[AnimParams.SpeedZ] = localSpeed.z;
            EnterMap.FloatValues[AnimParams.Forward2WantDirAngle] = entity.Forward2WantDirAngle;
        }

        public override void UpdateUpdateParam(PlayerEntity entity)
        {
            var speedOnGround = new Vector3(entity.Speed.x, 0,entity.Speed.z);

            var forward = Quaternion.Euler(0, entity.ViewYaw, 0) * Vector3.forward;

            var localSpeed = MapWordSpeed2LockSpeed(speedOnGround, forward.normalized);
            
            UpdateMap.FloatValues[AnimParams.SpeedX] = localSpeed.x;
            UpdateMap.FloatValues[AnimParams.SpeedZ] = localSpeed.z;
            UpdateMap.FloatValues[AnimParams.Forward2WantDirAngle] = entity.Forward2WantDirAngle;
        }
    }
}