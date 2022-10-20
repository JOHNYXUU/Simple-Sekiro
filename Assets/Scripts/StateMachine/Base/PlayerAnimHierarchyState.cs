using System.Collections.Generic;
using Entity;
using UnityEngine;

namespace StateMachine.Base
{
    public class PlayerAnimHierarchyState : PlayerAnimState
    {
        private int nowState = -1;
        
        public Dictionary<int, PlayerAnimState> SubStates = new ();

        public void Regist(int t, PlayerAnimState state)
        {
            if(SubStates.ContainsKey(t))
                SubStates.Remove(t);
            
            SubStates.Add(t, state);
        }
        
        public override int UpdateAnim(PlayerEntity entity)
        {
            var nextState = SubStates[nowState].UpdateAnim(entity);
            
            ChangeState(nextState,entity);
            
            return -1;
        }

        public virtual void ChangeState(int nextState,PlayerEntity entity)
        {
            if(nextState == -1 || !SubStates.ContainsKey(nextState))
                return;
            
            if(SubStates.ContainsKey(nowState))
                SubStates[nowState]?.Leave(entity);
            
            nowState = nextState;
            
            SubStates[nowState].Enter(entity);
        }
    }
}