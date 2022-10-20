using Controllers;
using Entity;
using Manager;
using StateMachine.Base;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Logic.MoveStateMachine
{
    public class MoveSprint : PlayerLogicState
    {
        public override void Enter(PlayerEntity entity)
        {
            // MgrInput.LeftShiftHoldTime = 0f;
            
            if(Config.showState)
                Debug.LogError("Sprint");
        }

        public override int Update(PlayerEntity entity, CharacterController cc)
        {
            base.Update(entity, cc);
            
            var camera = GameObject.FindWithTag("MainCamera").transform;

            // if(entity.CameraState == (int)CameraMoveType.Free)
            RotateFromCamera(camera, cc.transform, entity);
            // else
            //     Rotate2LockPoint(cc.transform, entity);
            
            var maxSpeed = entity.ArmState == (int)ArmState.Defence
                ? Config.standWalkMaxSpeedF
                : Config.standSprintMaxSpeed;

            entity.Speed = CalcSpeedOnGround(entity.Speed, Config.standAccSpeed, maxSpeed * (1 - entity.SlowDownPer),
                Config.standBrakeAccSpeed,
                camera.rotation.eulerAngles.y + GetMoveAngle(), entity.CameraState == (int)CameraMoveType.Lock);

            var dis = entity.Speed * Time.deltaTime;

            dis += GroundTest;

            var realDis =  Move(cc, entity, dis);
            
            entity.Speed = FixSpeed(realDis, Time.deltaTime, entity.Speed);

            if (entity.MoveBlock > 0)
                return (int)MoveState.Idle;
            
            if (Jump())
                return (int)MoveState.Jump;

            if (!cc.isGrounded)
                return (int)MoveState.Fall;

            if (HasMove() && !Input.GetKey(KeyCode.LeftShift))
                return (int)MoveState.Run;

            if (!HasMove())
                return (int)MoveState.Idle;
            
            return -1;
        }
        
        public override void Leave(PlayerEntity entity)
        {
            // MgrInput.LeftShiftHoldTime = 0f;
        }
    }
}