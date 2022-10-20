using System;
using Controllers;
using Entity;
using Manager;
using StateMachine.Base;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Logic.MoveStateMachine
{
    public class MoveRun : PlayerLogicState
    {
        public override void Enter(PlayerEntity entity)
        {
            if(Config.showState)
                Debug.LogError("Run");
        }

        public override int Update(PlayerEntity entity, CharacterController cc)
        {
            base.Update(entity, cc);

            var camera = GameObject.FindWithTag("MainCamera").transform;

            if(entity.CameraState == (int)CameraMoveType.Free)
                RotateFromCamera(camera, cc.transform, entity);
            else
                Rotate2LockPoint(cc.transform, entity);

            var maxSpeed = entity.ArmState == (int)ArmState.Defence
            ? Config.standWalkMaxSpeedF
            : Config.standRunMaxSpeedF;

            var accSpeedDir = (entity.CameraState == (int)CameraMoveType.Free)
                ? camera.rotation.eulerAngles.y + GetMoveAngle()
                : cc.transform.rotation.eulerAngles.y + GetMoveAngle();
            
            entity.Speed = CalcSpeedOnGround(entity.Speed, Config.standAccSpeed, maxSpeed * (1 - entity.SlowDownPer),
                Config.standBrakeAccSpeed,
                accSpeedDir,entity.CameraState == (int)CameraMoveType.Lock);

            var forward2SpeedAngle = Vector3.SignedAngle(cc.transform.forward,
                new Vector3(entity.Speed.x, 0f, entity.Speed.z), Vector3.up);

            entity.Forward2WantDirAngle = NormalizeYaw(forward2SpeedAngle);
            
            var start = cc.gameObject.transform.position;
            start.y += 1.0f;
            Debug.DrawRay(start , entity.Speed,Color.red);

            var dis = entity.Speed * Time.deltaTime;

            dis += GroundTest;

            var realDis =  Move(cc, entity, dis);
            
            entity.Speed = FixSpeed(realDis, Time.deltaTime, entity.Speed);

            // Debug.LogError(Vector3.SignedAngle(cc.gameObject.transform.forward, entity.Speed, Vector3.down));

            if (entity.MoveBlock > 0)
                return (int)MoveState.Idle;

            if (Jump() && entity.ArmState == (int)ArmState.Equip)
                return (int)MoveState.Jump;

            if (!cc.isGrounded)
                return (int)MoveState.Fall;
            
            if (!HasMove())
                return (int)MoveState.Idle;

            // if (!Input.GetKey(KeyCode.LeftShift) && MgrInput.LeftShiftHoldTime  < Config.pressClickBoundary && MgrInput.LeftShiftHoldTime > 0f) 
            //     return (int)MoveState.Dodge;
            
            if (Input.GetKey(KeyCode.LeftShift) &&  entity.ArmState != (int)ArmState.Defence)
                return (int)MoveState.Dodge;

            // if (HasMove() && Input.GetKey(KeyCode.LeftShift) &&
            //     MgrInput.LeftShiftHoldTime >= Config.pressClickBoundary)
            //     return (int)MoveState.Sprint;

            return -1;
        }
    }
}