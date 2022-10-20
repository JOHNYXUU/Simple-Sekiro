using UnityEngine;

namespace Entity
{
    public class PlayerEntity : EntityBase
    {
        public long ID;

        public int Hp = 1000;

        public Vector3 Speed;

        public float ViewYaw;

        public int CameraState;

        public int AnimState;

        public int CollisionFlag;

        public int MoveState;

        public int MoveJumpState;

        public int ArmState;
        
        public int AttackState;

        public int PoseState;

        public int MoveBlock;

        public int ArmBlock;

        public int AttackBlock;

        public int PoseBlock;

        public int LightAttackIndex = 1;

        public float SlowDownPer;

        public float Forward2WantDirAngle;
    }
}
