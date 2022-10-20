using UnityEngine;

namespace Entity
{
    public class EnemyEntity : EntityBase
    {
        public int Id;
        
        public int Hp = 100;

        public Vector3 Pos = new Vector3();

        public float Height;
        public float Width;
    }
}