using System.Collections.Generic;
using Enemy;
using Entity;
using UnityEngine;

namespace Manager
{
    public class MgrEnemy
    {
        public Dictionary<int, EnemyBase> AllEnemy;

        public MgrEnemy()
        {
            AllEnemy = new Dictionary<int, EnemyBase>();
        }

        public void AddEnemy(EnemyBase enemy)
        {
            AllEnemy.Add(enemy.EnemyGo.GetInstanceID(),enemy);
        }
        
        public void RemoveEnemy(EnemyBase enemy)
        {
            AllEnemy.Remove(enemy.EnemyGo.GetInstanceID());
        }

        public void Update()
        {
            foreach (var enemy in AllEnemy)
            {
                enemy.Value.Update();
            }
        }
    }
}