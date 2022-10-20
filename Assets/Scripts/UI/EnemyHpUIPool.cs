using Algorithm;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace UI
{
    public class EnemyHpUIPool : ObjectPool<GameObject>
    {
        public EnemyHpUIPool(int size) : base(size)
        {
            for (var i = 0; i < size; i++)
            {
                var prefab = Resources.Load("UI/EnemyHp");
                var go = PrefabUtility.InstantiatePrefab(prefab, GameObject.Find("Canvas").transform) as GameObject;
                PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                if (go != null)
                {
                    go.name = "EnemyHp";
                    go.SetActive(false);
                    Pool.Enqueue(go);
                }
            }
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
                var prefab = Resources.Load("UI/EnemyHp");
                res = PrefabUtility.InstantiatePrefab(prefab, GameObject.Find("Canvas").transform) as GameObject;
                PrefabUtility.UnpackPrefabInstance(res, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                MaxSize++;
                
                InUse.Add(res);
            }
            if(res)
                res.SetActive(true);
            
            return res;
        }

        public override bool Return(GameObject ob)
        {
            bool res;
            if (InUse.Contains(ob))
            {
                res = true;
                Pool.Enqueue(ob);
                InUse.Remove(ob);
                ob.SetActive(false);
            }
            else
            {
                res = false;
            }

            return res;
        }
    }
}