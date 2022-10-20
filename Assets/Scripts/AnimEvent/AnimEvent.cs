using System;
using UnityEngine;

namespace AnimEvent
{
    public class AnimEvent : MonoBehaviour
    {
        public GameObject weapon;

        private BoxCollider _weaponCollider;

        public void Awake()
        {
            weapon = GetDeepChild(gameObject, "Weapon_r");

            _weaponCollider = weapon.GetComponent<BoxCollider>();
        }

        public void BladeTriggerOn()
        {
            _weaponCollider.isTrigger = true;
        }
        
        public void BladeTriggerOff()
        {
            _weaponCollider.isTrigger = false;
        }
        
        public GameObject GetDeepChild(GameObject root, string name)
        {
            foreach (var child in root.transform.GetComponentsInChildren<Transform>())
            {
                if (child.name == name)
                {
                    return child.gameObject;
                }
            }

            return null;
        }
    }
}