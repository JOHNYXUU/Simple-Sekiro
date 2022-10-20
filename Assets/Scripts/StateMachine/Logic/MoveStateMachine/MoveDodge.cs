using System;
using System.Data.Common;
using Controllers;
using Entity;
using Manager;
using StateMachine.Base;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Logic.MoveStateMachine
{
    public class MoveDodge : PlayerLogicState
    {
        public static Vector3 DodgeDir;

        private float _enterDodgeTime;

        public MoveDodge()
        {
            DodgeDir = new Vector3();
        }
        
        public override void Enter(PlayerEntity entity)
        {
            
            if(Config.showState)
                Debug.LogError("Dodge");

            _enterDodgeTime = Time.time;

            var camera = GameObject.FindWithTag("MainCamera").transform;
            
            var angle = GetMoveAngle();

            if (Math.Abs(angle - -1f) < 0.1f)
                DodgeDir = Quaternion.Euler(0f, entity.ViewYaw, 0f) * Vector3.forward;
            else
            {
                var cameraYaw = camera.rotation.eulerAngles.y;

                var viewYaw = cameraYaw + angle;

                viewYaw = NormalizeYaw(viewYaw);

                if(entity.CameraState == (int)CameraMoveType.Lock)
                    DodgeDir = new Vector3(entity.Speed.x, 0f, entity.Speed.z);
                else
                    DodgeDir = Quaternion.Euler(0f, viewYaw, 0f) * Vector3.forward;
            }

            DodgeDir = DodgeDir.normalized;

            // MgrInput.LeftShiftHoldTime = 0f;
        }

        public override int Update(PlayerEntity entity, CharacterController cc)
        {
            base.Update(entity, cc);

            var nowTime = Time.time;

            var dis = Config.dodgeSpeed * DodgeDir *Time.deltaTime;

            dis += GroundTest;

            cc.Move(dis);
            
            if(entity.CameraState == (int)CameraMoveType.Lock)
                Rotate2LockPoint(cc.transform, entity);

            if (entity.MoveBlock > 0)
                return (int)MoveState.Idle;
            
            if (Jump())
                return (int)MoveState.Jump;
            
            if (nowTime - _enterDodgeTime < Config.dodgeAnimTime)
                return -1;
            
            if (!cc.isGrounded)
                return (int)MoveState.Fall;

            // if (HasMove() && Input.GetKey(KeyCode.LeftShift) &&
            //     MgrInput.LeftShiftHoldTime >= Config.pressClickBoundary && IsMoveForward(cc, entity))
            //     return (int)MoveState.Sprint;

            if (HasMove() && Input.GetKey(KeyCode.LeftShift) &&  entity.ArmState != (int)ArmState.Defence)
                return (int)MoveState.Sprint;
            
            if (HasMove() && Input.GetKey(KeyCode.LeftShift) &&  entity.ArmState == (int)ArmState.Defence)
                return (int)MoveState.Run;

            if (HasMove())
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