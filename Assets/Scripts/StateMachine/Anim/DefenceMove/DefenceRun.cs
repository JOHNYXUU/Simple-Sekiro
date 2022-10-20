using Controllers;
using Entity;
using StateMachine.Anim.Data;
using StateMachine.Base;
using StateMachine.Enum.AnimEnum;
using StateMachine.Enum.LogicEnum;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace StateMachine.Anim.DefenceMove
{
    public class DefenceRun : PlayerAnimState
    {
        public DefenceRun()
        {
            EnterMap = new AnimParamMap();
            EnterMap.FloatValues.Add(AnimParams.SpeedValue, 0);
            EnterMap.FloatValues.Add(AnimParams.Forward2WantDirAngle, 0);
            EnterMap.BoolValues.Add(AnimParams.IsDefence, true);
            EnterMap.BoolValues.Add(AnimParams.IsMove, true);
            
            
            UpdateMap = new AnimParamMap();
            UpdateMap.FloatValues.Add(AnimParams.Forward2WantDirAngle, 0);
            UpdateMap.FloatValues.Add(AnimParams.SpeedValue, 0);
        }
        
        public override void Enter(PlayerEntity entity)
        {
            base.Enter(entity);

            entity.AttackBlock++;
            
            if(Config.showAnimState)
                Debug.LogError("DefenceRun");
        }

        public override int UpdateAnim(PlayerEntity entity)
        {
            base.UpdateAnim(entity);

            if (entity.ArmState == (int)ArmState.Defence && entity.MoveState == (int)MoveState.Idle)
                return (int)AnimState.DefenceIdle;
            
            if (entity.ArmState == (int)ArmState.Equip && entity.MoveState == (int)MoveState.Idle)
                return (int)AnimState.EquipIdle;

            if (entity.ArmState == (int)ArmState.Equip && entity.MoveState == (int)MoveState.Run && entity.CameraState == (int)CameraMoveType.Free)
                return (int)AnimState.FreeRun;
            
            if (entity.ArmState == (int)ArmState.Equip && entity.MoveState == (int)MoveState.Run && entity.CameraState == (int)CameraMoveType.Lock)
                return (int)AnimState.LockRun;
            
            return -1;
        }

        public override void Leave(PlayerEntity entity)
        {
            base.Leave(entity);
            
            entity.AttackBlock--;
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
            // Debug.LogError(entity.Forward2WantDirAngle);
        }
    }
}