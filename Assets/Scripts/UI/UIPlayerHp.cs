using Entity;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace UI
{
    public class UIPlayerHp : UIBase
    {
        public UIPlayerHp(GameObject ui, EntityBase resourceEntity = null) : base(ui, resourceEntity)
        {
            
        }

        public override void Update()
        {
            if (ResourceEntity.GetType() == typeof(PlayerEntity))
            {
                var playerEntity = (PlayerEntity)ResourceEntity;
                UI.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,  playerEntity.Hp);
            }
        }
    }
}