using System.Collections.Generic;
using StateMachine.Anim;
using StateMachine.Anim.Data;
using UnityEngine;

namespace Controllers
{
    public class AnimatorController
    {
        public Animator Anim;

        public AnimatorController(Animator anim)
        {
            Anim = anim;
        }

        public void UpdateParam(AnimParamMap map)
        {
            if(map == null)
                return;
            
            foreach (var kvp in map.BoolValues)
            {
                UpdateBoolParam(kvp.Key, kvp.Value);
            }
            
            foreach (var kvp in map.FloatValues)
            {
                UpdateFloatParam(kvp.Key, kvp.Value);
            }
            
            foreach (var kvp in map.IntValues)
            {
                UpdateIntParam(kvp.Key, kvp.Value);
            }
        }

        public void UpdateFloatParam(string name, float v)
        {
            Anim.SetFloat(name, v);
        }
        
        public void UpdateBoolParam(string name, bool v)
        {
            Anim.SetBool(name, v);
        }
        
        public void UpdateIntParam(string name, int v)
        {
            Anim.SetInteger(name, v);
        }
        
        
    }
}