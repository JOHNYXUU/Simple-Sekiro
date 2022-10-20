using System;
using Entity;
using Newtonsoft.Json.Bson;
using UnityEngine;

namespace StateMachine.Base
{
    public class PlayerState
    {
        protected GameConfig Config => GameObject.Find("Config").GetComponent<GameConfig>();
        
        protected GameLoop GameLoop => GameObject.Find("Loop").GetComponent<GameLoop>();
        
        public virtual void Enter(PlayerEntity entity)
        {
            
        }

        public virtual int Update(PlayerEntity entity, CharacterController cc)
        {
            return -1;
        }

        public virtual void Leave(PlayerEntity entity)
        {
            
        }
    }
}