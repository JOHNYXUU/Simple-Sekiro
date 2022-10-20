using Controllers;
using Entity;
using StateMachine.Anim.Data;
using StateMachine.Base;
using StateMachine.Enum.AnimEnum;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Anim.FreeMove
{
    public class FreeSprint : PlayerAnimState
    {
        public FreeSprint()
        {
            EnterMap = new AnimParamMap();
            

            EnterMap.FloatValues.Add(AnimParams.SpeedValue, 0);
            EnterMap.BoolValues.Add(AnimParams.IsMove, true);
            EnterMap.BoolValues.Add(AnimParams.IsSprint, true);
            
            UpdateMap = new AnimParamMap();
            UpdateMap.FloatValues.Add(AnimParams.SpeedValue, 0);

            LeaveMap = new AnimParamMap();
            LeaveMap.BoolValues.Add(AnimParams.IsSprint, false);
            // LeaveMap.BoolValues.Add(AnimParams.IsMove, false);
        }
        
        public override void Enter(PlayerEntity entity)
        {
            base.Enter(entity);
            
            var speedOnGround = new Vector3(entity.Speed.x, 0f,entity.Speed.z);

            AnimController.Anim.speed = speedOnGround.magnitude / Config.standSprintMaxSpeed;
            
            if(Config.showAnimState)
                Debug.LogError("FreeSprint");
        }

        public override int UpdateAnim(PlayerEntity entity)
        {
            base.UpdateAnim(entity);
            
            var speedOnGround = new Vector3(entity.Speed.x, 0f,entity.Speed.z);
            
            // var ism = EnterMap.BoolValues[AnimParams.IsMove];
            // var iss = EnterMap.BoolValues[AnimParams.IsSprint];
            //
            // if(!ism || !iss)
            //     Debug.LogError(ism + " " + iss);
            
            // Debug.LogError(1);

            if (entity.MoveState == (int)MoveState.Idle && speedOnGround.magnitude * 100f <= 95f)
                return (int)AnimState.EquipIdle;
            
            if (entity.MoveState == (int)MoveState.Idle && speedOnGround.magnitude * 100f > 95f)
                return (int)AnimState.FreeRunStop;

            if (entity.MoveState == (int)MoveState.Run && entity.CameraState == (int)CameraMoveType.Free)
                return (int)AnimState.FreeRun;
            
            if (entity.MoveState == (int)MoveState.Run && entity.CameraState == (int)CameraMoveType.Lock)
                return (int)AnimState.LockRun;
            
            if (entity.MoveState == (int)MoveState.Jump && entity.MoveJumpState == (int)MoveJumpState.JumpStart)
                return (int)AnimState.JumpStart;
            
            if (entity.MoveState == (int)MoveState.Fall)
                return (int)AnimState.JumpLoop;

            return -1;
        }

        public override void Leave(PlayerEntity entity)
        {
            base.Leave(entity);
            
            AnimController.Anim.speed = 1f;
        }

        public override void UpdateEnterParam(PlayerEntity entity)
        {
            var speedOnGround = new Vector2(entity.Speed.x, entity.Speed.z);

            EnterMap.FloatValues[AnimParams.SpeedValue] = speedOnGround.magnitude * 100f;
        }

        public override void UpdateUpdateParam(PlayerEntity entity)
        {
            var speedOnGround = new Vector2(entity.Speed.x, entity.Speed.z);

            UpdateMap.FloatValues[AnimParams.SpeedValue] = speedOnGround.magnitude * 100f;
        }
    }
}