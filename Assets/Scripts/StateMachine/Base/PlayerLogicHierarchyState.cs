using System.Collections.Generic;
using Entity;
using StateMachine.Base;
using UnityEngine;

namespace StateMachine.Logic
{
    public class PlayerLogicHierarchyState : PlayerLogicState
    {
        private int nowState = -1;
        
        public Dictionary<int, PlayerLogicState> SubStates = new ();
        
        public void Regist(int t, PlayerLogicState state)
        {
            if(SubStates.ContainsKey(t))
                SubStates.Remove(t);
            
            SubStates.Add(t, state);
        }

        public override int Update(PlayerEntity entity,CharacterController cc)
        {
            var nextState = SubStates[nowState].Update(entity, cc);
            
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