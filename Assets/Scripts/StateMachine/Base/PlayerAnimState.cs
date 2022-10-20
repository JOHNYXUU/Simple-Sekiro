using Entity;
using StateMachine.Anim;
using StateMachine.Anim.Data;
using UnityEngine;
using AnimatorController = Controllers.AnimatorController;

namespace StateMachine.Base
{
    public class PlayerAnimState : PlayerState
    {
        public AnimParamMap EnterMap;

        public AnimParamMap UpdateMap;

        public AnimParamMap LeaveMap;

        public static AnimatorController AnimController;

        public void Init(AnimatorController ac)
        {
            AnimController = ac;
        }

        public override void Enter(PlayerEntity entity)
        {
            UpdateEnterParam(entity);
            
            AnimController.UpdateParam(EnterMap);
        }

        public virtual int UpdateAnim(PlayerEntity entity)
        {
            UpdateUpdateParam(entity);
            
            AnimController.UpdateParam(UpdateMap);

            return -1;
        }

        public override void Leave(PlayerEntity entity)
        {
            UpdateLeaveParam(entity);
            
            AnimController.UpdateParam(LeaveMap);
        }

        public virtual void UpdateEnterParam(PlayerEntity entity)
        {
            
        }
        
        public virtual void UpdateUpdateParam(PlayerEntity entity)
        {
            
        }
        
        public virtual void UpdateLeaveParam(PlayerEntity entity)
        {
            
        }

        protected Vector3 MapWordSpeed2LockSpeed(Vector3 worldSpeed, Vector3 forward)
        {
            var right = Quaternion.AngleAxis(90, Vector3.up) * forward;

            right = right.normalized;
            forward = forward.normalized;

            var speedZ = Vector3.Dot(worldSpeed, forward);
            var speedX = Vector3.Dot(worldSpeed, right);

            return new Vector3(speedX, 0, speedZ);
        }
    }
}