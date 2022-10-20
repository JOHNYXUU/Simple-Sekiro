using System.Runtime.CompilerServices;
using UnityEngine;

namespace Manager
{
    public class MgrInput
    {
        public static bool Alpha1IsContinue;

        public static bool QContinue;

        public static bool LeftMouseButtonContinue;

        public static bool MidMouseButtonContinue;

        public static bool LeftShiftContinue;
        
        // public static float LeftShiftHoldTime;

        public static float LeftMouseButtonHoldTime;
        
        public static void Update(bool isCounting)
        {
            CalcContinue();

            CalcHoldTime(isCounting);
        }

        private static void CalcContinue()
        {
            Alpha1IsContinue = Input.GetKey(KeyCode.Alpha1);
            
            QContinue = Input.GetKey(KeyCode.Q);

            LeftMouseButtonContinue = Input.GetMouseButton(0);

            LeftShiftContinue = Input.GetKey(KeyCode.LeftShift);

            MidMouseButtonContinue = Input.GetMouseButton(2);
        }

        private static void CalcHoldTime(bool isCounting)
        {
            if(!isCounting)
                return;
            
            if (Input.GetMouseButton(0))
            {
                LeftMouseButtonHoldTime += Time.deltaTime;
            }
            // else
            // {
            //     LeftMouseButtonHoldTime = 0f;
            // }
        }
    }
}