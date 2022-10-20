using Algorithm;
using Entity;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIEnemyHp : UIBase
    {
        private static EnemyHpUIPool _enemyHpUIPool;
        
        public UIEnemyHp(GameObject ui = null, EntityBase resourceEntity = null) : base(ui, resourceEntity)
        {
            if (_enemyHpUIPool == null)
                _enemyHpUIPool = new EnemyHpUIPool(5);
            UI = _enemyHpUIPool.Get();
        }
        
        public override void Update()
        {
            if (ResourceEntity.GetType() == typeof(EnemyEntity))
            {
                var enemyEntity = (EnemyEntity)ResourceEntity;
                UI.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,  2 * enemyEntity.Hp);
                
                var enemyCenterPos = enemyEntity.Pos;
                enemyCenterPos.y += enemyEntity.Height + 0.2f;
                
                if (Camera.main != null)
                {
                    UI.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(enemyCenterPos);
                    var dir = (enemyCenterPos - Camera.main.transform.position).normalized;
                    var dot = Vector3.Dot(Camera.main.transform.forward, dir);
                    if (Vector3.Distance(Camera.main.transform.position, enemyEntity.Pos) > Config.enemyHpUIMaxViewDis || dot <= 0f)
                    {
                        UI.GetComponent<Image>().enabled = false;
                    }
                    else
                    {
                        UI.GetComponent<Image>().enabled = true;
                    }
                }
            }
        }
    }
}