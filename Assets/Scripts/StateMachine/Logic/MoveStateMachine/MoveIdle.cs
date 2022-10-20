using System.Runtime.CompilerServices;
using Controllers;
using Entity;
using Manager;
using StateMachine.Base;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Logic.MoveStateMachine
{
    public class MoveIdle : PlayerLogicState
    {
        public override void Enter(PlayerEntity entity)
        {
            if(Config.showState)
                Debug.LogError("Idle");
        }

        public override int Update(PlayerEntity entity,CharacterController cc)
        {
            base.Update(entity, cc);

            entity.ViewYaw = cc.gameObject.transform.rotation.eulerAngles.y;

            entity.Speed = BrakeSpeedOnGround(entity.Speed, Config.standBrakeAccSpeed);

            // var dis = entity.Speed * Time.deltaTime;

            var dis = GroundTest;

            cc.Move(dis);
            
            // Debug.LogError(cc.velocity.y);

            if(entity.CameraState == (int)CameraMoveType.Lock)
                Rotate2LockPoint(cc.transform, entity);
            
            if (entity.MoveBlock > 0)
            {
                return -1;
            }
                


            if (Jump() && entity.ArmState == (int)ArmState.Equip)
                return (int)MoveState.Jump;

            if (!cc.isGrounded)
                return (int)MoveState.Fall;

            // if (HasMove() && Input.GetKey(KeyCode.LeftShift) && MgrInput.LeftShiftHoldTime >= Config.pressClickBoundary && IsMoveForward(cc, entity))
            //     return (int)MoveState.Sprint;
            
            // if (!Input.GetKey(KeyCode.LeftShift) && MgrInput.LeftShiftHoldTime < Config.pressClickBoundary && MgrInput.LeftShiftHoldTime > 0f)
            //     return (int)MoveState.Dodge;
            
            if(Input.GetKey(KeyCode.LeftShift) && (!MgrInput.LeftShiftContinue || HasMove()) &&  entity.ArmState != (int)ArmState.Defence)
                return (int)MoveState.Dodge;
            
            // if(Input.GetKey(KeyCode.LeftShift) && MgrInput.LeftShiftContinue)
            //     return (int)MoveState.Dodge;

            if (HasMove())
                return (int)MoveState.Run;

            return -1;
        }
    }
}