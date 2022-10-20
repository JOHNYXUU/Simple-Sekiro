using System.Collections.Generic;

namespace StateMachine.Anim.Data
{
    public class AnimParamMap
    {
        public Dictionary<string, bool> BoolValues;

        public Dictionary<string, int> IntValues;

        public Dictionary<string, float> FloatValues;

        public AnimParamMap()
        {
            BoolValues = new Dictionary<string, bool>();

            IntValues = new Dictionary<string, int>();

            FloatValues = new Dictionary<string, float>();
        }
    }
}