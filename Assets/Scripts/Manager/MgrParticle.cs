using System.Collections.Generic;
using Algorithm;
using DefaultNamespace;
using UnityEngine;

namespace Manager
{
    public class MgrParticle : ObjectPool<GameObject>
    {
        public GameConfig Config => GameObject.Find("Config").GetComponent<GameConfig>();
        public GameObject Particle => GameObject.Find("Particle");
        public List<ParticleSystem> ParticleSystems => Particle.GetComponent<ParticleResource>().ParticleSystems;

        public List<float> InUseBeginTime;

        public MgrParticle(int size) : base(size)
        {
            InUseBeginTime = new List<float>();

            foreach (var p in ParticleSystems)
            {
                var go = Object.Instantiate(p, Particle.transform, true);
                go.gameObject.SetActive(false);
                Pool.Enqueue(go.gameObject);
            }
        }

        public void Update()
        {
            var nowTime = Time.time;
            for (var i = 0; i < InUseBeginTime.Count; i++)
            {
                if (nowTime - InUseBeginTime[i] > Config.bloodParticleStayTime)
                {
                    Return(InUse[i]);
                    InUseBeginTime.RemoveAt(i);
                    i--;
                }
            }
        }

        public void RandomBlood(Vector3 pos)
        {
            var particle = Get();

            particle.transform.position = pos;
            
            particle.gameObject.SetActive(true);
            
            InUseBeginTime.Add(Time.time);
        }

        public override GameObject Get()
        {
            GameObject res;
            if (Pool.Count > 0)
            {
                res = Pool.Peek();
                
                InUse.Add(Pool.Dequeue());
            }
            else
            {
                var index = Random.Range(0, ParticleSystems.Count);

                res = Object.Instantiate(ParticleSystems[index]).gameObject;
                res.transform.SetParent(Particle.transform);
                res.gameObject.SetActive(false);
                
                MaxSize++;
                
                InUse.Add(res);
            }

            return res;
        }

        public override bool Return(GameObject ob)
        {
            ob.SetActive(false);
            return base.Return(ob);
        }
    }
}