using Entity;
using StateMachine.Base;
using StateMachine.Enum.LogicEnum;
using UnityEngine;
using UnityEngine.Rendering;

namespace StateMachine.Logic.MoveStateMachine.MoveJumpStateMachine
{
    public class MoveJumpLoop : PlayerLogicState
    {
        // private float updis = 0;
        //
        // private float downdis = 0;
        //
        // private float lastY = 0;
        
        public override void Enter(PlayerEntity entity)
        {
            if(Config.showState)
                Debug.LogError("Jump Loop");
            
            // updis = 0;
            //
            // downdis = 0;
        }

        public override int Update(PlayerEntity entity, CharacterController cc)
        {
            var deltaTime = Time.deltaTime;

            entity.Speed.y -= Config.gravity * deltaTime;

            var dis = entity.Speed * deltaTime;

            var realDis =  Move(cc, entity, dis);


            // if (dis.y > 0)
            // {
            //     updis += dis.y;
            // }
            // else
            // {
            //     downdis += dis.y;
            // }
            
            // Debug.LogError("speed " + entity.Speed.y);
            
            entity.Speed = FixSpeed(realDis, deltaTime, entity.Speed);
            
            // Debug.LogError(cc.isGrounded);

            if (cc.isGrounded)
                return (int)MoveJumpState.JumpEnd;
            
            return -1;
        }

        public override void Leave(PlayerEntity entity)
        {
            // Debug.LogError(updis +" "+downdis);
        }
    }
}