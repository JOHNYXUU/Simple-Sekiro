using Entity;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace UI
{
    public class UIBase
    {
        public GameObject UI;

        public EntityBase ResourceEntity;
        
        protected GameConfig Config => GameObject.Find("Config").GetComponent<GameConfig>();

        public UIBase(GameObject ui, EntityBase resourceEntity = null)
        {
            UI = ui;

            ResourceEntity = resourceEntity;
        }

        public virtual void Update()
        {
            
        }
    }
}