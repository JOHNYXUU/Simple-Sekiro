using System;
using StateMachine.Enum.AnimEnum;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace Trigger
{
    public class BladeTrigger : MonoBehaviour
    {
        public GameLoop GameLoop => GameObject.Find("Loop").GetComponent<GameLoop>();
        
        public GameConfig Config => GameObject.Find("Config").GetComponent<GameConfig>();

        private float _lastTriggerTime;

        private const float TriggerDeltaTime = 0.2f;

        private void OnTriggerEnter(Collider other)
        {
            var enemy = other.gameObject;

            if (GameLoop.MgrEnemy.AllEnemy.ContainsKey(enemy.GetInstanceID()))
            {
                var enemyCenterPos = enemy.transform.position;
                enemyCenterPos.y += enemy.GetComponent<CapsuleCollider>().height / 2.0f;
                
                var nowTime = Time.time;
                
                if(nowTime - _lastTriggerTime <= TriggerDeltaTime)
                    return;

                GameLoop.MgrAudio.PlayOneShot(
                    GameLoop.MgrAudio.GetRandomAudioClip(Config.bladeHitFleshAudioPath, Config.bladeHitFleshAudioNum),
                    0.5f);
                
                if (GameLoop.MgrPlayer.MyPlayerEntity.AttackState == (int)AttackState.LightAttack)
                {
                    GameLoop.MgrEnemy.AllEnemy[enemy.GetInstanceID()].EnemyEntity.Hp -= Config.lightAttackDamage;
                    // Debug.LogError(enemy.name + "hp - 10");
                }
                else if (GameLoop.MgrPlayer.MyPlayerEntity.AttackState == (int)AttackState.HeavyAttack)
                {
                    GameLoop.MgrEnemy.AllEnemy[enemy.GetInstanceID()].EnemyEntity.Hp -= Config.heavyAttackDamage;
                    // Debug.LogError(enemy.name + "hp - 15");
                }

                _lastTriggerTime = Time.time;
                
                GameLoop.MgrParticle.RandomBlood(enemyCenterPos);
            }
        }
    }
}