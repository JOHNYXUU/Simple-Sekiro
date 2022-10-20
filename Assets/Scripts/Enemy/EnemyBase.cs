using Entity;
using UnityEngine;

namespace Enemy
{
    public class EnemyBase
    {
        public EnemyEntity EnemyEntity;

        public GameObject EnemyGo;

        public EnemyBase(GameObject enemyGo)
        {
            EnemyEntity = new EnemyEntity();
            EnemyGo = enemyGo;

            EnemyEntity.Height = enemyGo.GetComponent<CapsuleCollider>().height;
            EnemyEntity.Width = enemyGo.GetComponent<CapsuleCollider>().radius;
        }

        public void Update()
        {
            EnemyEntity.Pos = EnemyGo.transform.position;
        }
    }
}