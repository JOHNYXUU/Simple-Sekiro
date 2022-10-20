using System.Collections.Generic;
using Entity;
using StateMachine.Base;
using UnityEditor;
using UnityEngine;

namespace StateMachine.Logic
{
    public class PlayerLogicParallelState: PlayerLogicState
    {
        public Dictionary<int, PlayerLogicState> SubStates = new ();

        public void Regist(int t, PlayerLogicState state)
        {
            if(SubStates.ContainsKey(t))
                SubStates.Remove(t);
            
            SubStates.Add(t, state);
        }

        public override void Enter(PlayerEntity entity)
        {
            foreach (var subState in SubStates.Values)
            {
                subState.Enter(entity);
            }
        }

        public override int Update(PlayerEntity entity,CharacterController cc)
        {
            foreach (var subState in SubStates.Values)
            {
                subState.Update(entity, cc);
            }

            return -1;
        }
    }
}