using UnityEngine;
using System;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    public static class MovementUtility
    {
        private static Dictionary<GameObject, Dictionary<Type, Component>> gameObjectComponentMap = new Dictionary<GameObject, Dictionary<Type, Component>>();
        private static Dictionary<GameObject, Dictionary<Type, Component>> gameObjectParentComponentMap = new Dictionary<GameObject, Dictionary<Type, Component>>();
        private static Dictionary<GameObject, Dictionary<Type, Component[]>> gameObjectComponentsMap = new Dictionary<GameObject, Dictionary<Type, Component[]>>();

        // Cast a sphere with the desired distance. Check each collider hit to see if it is within the field of view. Set objectFound
        // to the object that is most directly in front of the agent
        public static GameObject WithinSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, Collider[] overlapColliders, LayerMask objectLayerMask, Vector3 targetOffset, LayerMask ignoreLayerMask, bool useTargetBone, HumanBodyBones targetBone, bool drawDebugRay)
        {
            GameObject objectFound = null;
            var hitCount = Physics.OverlapSphereNonAlloc(transform.TransformPoint(positionOffset), viewDistance, overlapColliders, objectLayerMask, QueryTriggerInteraction.Ignore);
            if (hitCount > 0) {
#if UNITY_EDITOR
                if (hitCount == overlapColliders.Length) {
                    Debug.LogWarning("Warning: The hit count is equal to the max collider array size. This will cause objects to be missed. Consider increasing the max collision count size.");
                }
#endif
                float minAngle = Mathf.Infinity;
                for (int i = 0; i < hitCount; ++i) {
                    float angle;
                    GameObject obj;
                    // Call the WithinSight function to determine if this specific object is within sight
                    if ((obj = WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, overlapColliders[i].gameObject, targetOffset, false, 0, out angle, ignoreLayerMask, useTargetBone, targetBone, drawDebugRay)) != null) {
                        // This object is within sight. Set it to the objectFound GameObject if the angle is less than any of the other objects
                        if (angle < minAngle) {
                            minAngle = angle;
                            objectFound = obj;
                        }
                    }
                }
            }
            return objectFound;
        }

        // Cast a circle with the desired distance. Check each collider hit to see if it is within the field of view. Set objectFound
        // to the object that is most directly in front of the agent
        public static GameObject WithinSight2D(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, Collider2D[] overlapColliders, LayerMask objectLayerMask, Vector3 targetOffset, float angleOffset2D, LayerMask ignoreLayerMask, bool drawDebugRay)
        {
            GameObject objectFound = null;
            var hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, viewDistance, overlapColliders, objectLayerMask);
            if (hitCount > 0) {
#if UNITY_EDITOR
                if (hitCount == overlapColliders.Length) {
                    Debug.LogWarning("Warning: The hit count is equal to the max collider array size. This will cause objects to be missed. Consider increasing the max collision count size.");
                }
#endif
                float minAngle = Mathf.Infinity;
                for (int i = 0; i < hitCount; ++i) {
                    float angle;
                    GameObject obj;
                    // Call the 2D WithinSight function to determine if this specific object is within sight
                    if ((obj = WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, overlapColliders[i].gameObject, targetOffset, true, angleOffset2D, out angle, ignoreLayerMask, false, HumanBodyBones.Hips, drawDebugRay)) != null) {
                        // This object is within sight. Set it to the objectFound GameObject if the angle is less than any of the other objects
                        if (angle < minAngle) {
                            minAngle = angle;
                            objectFound = obj;
                        }
                    }
                }
            }
            return objectFound;
        }

        // Public helper function that will automatically create an angle variable that is not used. This function is useful if the calling object doesn't
        // care about the angle between transform and targetObject
        public static GameObject WithinSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, GameObject targetObject, Vector3 targetOffset, LayerMask ignoreLayerMask, bool useTargetBone, HumanBodyBones targetBone, bool drawDebugRay)
        {
            float angle;
            return WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, targetObject, targetOffset, false, 0, out angle, ignoreLayerMask, useTargetBone, targetBone, drawDebugRay);
    }

        // Public helper function that will automatically create an angle variable that is not used. This function is useful if the calling object doesn't
        // care about the angle between transform and targetObject
        public static GameObject WithinSight2D(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, GameObject targetObject, Vector3 targetOffset, float angleOffset2D, LayerMask ignoreLayerMask, bool useTargetBone, HumanBodyBones targetBone, bool drawDebugRay)
        {
            float angle;
            return WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, targetObject, targetOffset, true, angleOffset2D, out angle, ignoreLayerMask, useTargetBone, targetBone, drawDebugRay);
        }

        // Determines if the targetObject is within sight of the transform. It will set the angle regardless of whether or not the object is within sight
        public static GameObject WithinSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, GameObject targetObject, Vector3 targetOffset, bool usePhysics2D, float angleOffset2D, out float angle, int ignoreLayerMask, bool useTargetBone, HumanBodyBones targetBone, bool drawDebugRay)
        {
            if (targetObject == null) {
                angle = 0;
                return null;
            }
            if (useTargetBone) {
                Animator animator;
                if ((animator = GetParentComponentForType<Animator>(targetObject)) != null) {
                    var bone = animator.GetBoneTransform(targetBone);
                    if (bone != null) {
                        targetObject = bone.gameObject;
                    }
                }
            }
            // The target object needs to be within the field of view of the current object
            var direction = targetObject.transform.TransformPoint(targetOffset) - transform.TransformPoint(positionOffset);
            if (usePhysics2D) {
                var eulerAngles = transform.eulerAngles;
                eulerAngles.z -= angleOffset2D;
                angle = Vector3.Angle(direction, Quaternion.Euler(eulerAngles) * Vector3.up);
                direction.z = 0;
            } else {
                angle = Vector3.Angle(direction, transform.forward);
                direction.y = 0;
            }
            if (direction.magnitude < viewDistance && angle < fieldOfViewAngle * 0.5f) {
                // The hit agent needs to be within view of the current agent
                var hitTransform = LineOfSight(transform, positionOffset, targetObject, targetOffset, usePhysics2D, ignoreLayerMask, drawDebugRay);
                if (hitTransform != null) {
                    if (IsAncestor(targetObject.transform, hitTransform)) {
#if UNITY_EDITOR
                        if (drawDebugRay) {
                            Debug.DrawLine(transform.TransformPoint(positionOffset), targetObject.transform.TransformPoint(targetOffset), Color.green);
                        }
#endif
                        return targetObject; // return the target object meaning it is within sight
#if UNITY_EDITOR
                    } else {
                        if (drawDebugRay) {
                            Debug.DrawLine(transform.TransformPoint(positionOffset), targetObject.transform.TransformPoint(targetOffset), Color.yellow);
                        }
#endif
                    }
                } else if (GetComponentForType<Collider>(targetObject) == null && GetComponentForType<Collider2D>(targetObject) == null) {
                    // If the linecast doesn't hit anything then that the target object doesn't have a collider and there is nothing in the way
                    if (targetObject.gameObject.activeSelf) {
                        return targetObject;
                    }
                }
            } else {
#if UNITY_EDITOR
                if (drawDebugRay) {
                    Debug.DrawLine(transform.TransformPoint(positionOffset), targetObject.transform.TransformPoint(targetOffset), angle >= fieldOfViewAngle * 0.5f ? Color.red : Color.magenta);
                }
#endif
            }
            // return null if the target object is not within sight
            return null;
        }

        public static Transform LineOfSight(Transform transform, Vector3 positionOffset, GameObject targetObject, Vector3 targetOffset, bool usePhysics2D, int ignoreLayerMask, bool drawDebugRay)
        {
            Transform hitTransform = null;
            if (usePhysics2D) {
                RaycastHit2D hit;
                if ((hit = Physics2D.Linecast(transform.TransformPoint(positionOffset), targetObject.transform.TransformPoint(targetOffset), ~ignoreLayerMask))) {
                    hitTransform = hit.transform;
                }
            } else {
                RaycastHit hit;
                if (Physics.Linecast(transform.TransformPoint(positionOffset), targetObject.transform.TransformPoint(targetOffset), out hit, ~ignoreLayerMask, QueryTriggerInteraction.Ignore)) {
                    hitTransform = hit.transform;
                }
            }
            return hitTransform;
        }

        // Is the hitObject an ancestor of the target?
        public static bool IsAncestor(Transform target, Transform hitTransform)
        {
            return hitTransform.IsChildOf(target) || target.IsChildOf(hitTransform);
        }

        // Cast a sphere with the desired radius. Check each object's audio source to see if audio is playing. If audio is playing
        // and its audibility is greater than the audibility threshold then return the object heard
        public static GameObject WithinHearingRange(Transform transform, Vector3 positionOffset, float audibilityThreshold, float hearingRadius, Collider[] overlapColliders, LayerMask objectLayerMask)
        {
            GameObject objectHeard = null;
            var hitCount = Physics.OverlapSphereNonAlloc(transform.TransformPoint(positionOffset), hearingRadius, overlapColliders, objectLayerMask, QueryTriggerInteraction.Ignore);
            if (hitCount > 0) {
#if UNITY_EDITOR
                if (hitCount == overlapColliders.Length) {
                    Debug.LogWarning("Warning: The hit count is equal to the max collider array size. This will cause objects to be missed. Consider increasing the max collision count size.");
                }
#endif
                float maxAudibility = 0;
                for (int i = 0; i < hitCount; ++i) {
                    float audibility = 0;
                    GameObject obj;
                    // Call the WithinHearingRange function to determine if this specific object is within hearing range
                    if ((obj = WithinHearingRange(transform, positionOffset, audibilityThreshold, overlapColliders[i].gameObject, ref audibility)) != null) {
                        // This object is within hearing range. Set it to the objectHeard GameObject if the audibility is less than any of the other objects
                        if (audibility > maxAudibility) {
                            maxAudibility = audibility;
                            objectHeard = obj;
                        }
                    }
                }
            }
            return objectHeard;
        }

        // Cast a circle with the desired radius. Check each object's audio source to see if audio is playing. If audio is playing
        // and its audibility is greater than the audibility threshold then return the object heard
        public static GameObject WithinHearingRange2D(Transform transform, Vector3 positionOffset, float audibilityThreshold, float hearingRadius, Collider2D[] overlapColliders, LayerMask objectLayerMask)
        {
            GameObject objectHeard = null;
            var hitCount = Physics2D.OverlapCircleNonAlloc(transform.TransformPoint(positionOffset), hearingRadius, overlapColliders, objectLayerMask);
            if (hitCount > 0) {
#if UNITY_EDITOR
                if (hitCount == overlapColliders.Length) {
                    Debug.LogWarning("Warning: The hit count is equal to the max collider array size. This will cause objects to be missed. Consider increasing the max collision count size.");
                }
#endif
                float maxAudibility = 0;
                for (int i = 0; i < hitCount; ++i) {
                    float audibility = 0;
                    GameObject obj;
                    // Call the WithinHearingRange function to determine if this specific object is within hearing range
                    if ((obj = WithinHearingRange(transform, positionOffset, audibilityThreshold, overlapColliders[i].gameObject, ref audibility)) != null) {
                        // This object is within hearing range. Set it to the objectHeard GameObject if the audibility is less than any of the other objects
                        if (audibility > maxAudibility) {
                            maxAudibility = audibility;
                            objectHeard = obj;
                        }
                    }
                }
            }
            return objectHeard;
        }

        // Public helper function that will automatically create an audibility variable that is not used. This function is useful if the calling call doesn't
        // care about the audibility value
        public static GameObject WithinHearingRange(Transform transform, Vector3 positionOffset, float audibilityThreshold, GameObject targetObject)
        {
            float audibility = 0;
            return WithinHearingRange(transform, positionOffset, audibilityThreshold, targetObject, ref audibility);
        }

        public static GameObject WithinHearingRange(Transform transform, Vector3 positionOffset, float audibilityThreshold, GameObject targetObject, ref float audibility)
        {
            AudioSource[] colliderAudioSource;
            // Check to see if the hit agent has an audio source and that audio source is playing
            if ((colliderAudioSource = GetComponentsForType<AudioSource>(targetObject)) != null) {
                for (int i = 0; i < colliderAudioSource.Length; ++i) {
                    if (colliderAudioSource[i].isPlaying) {
                        var distance = Vector3.Distance(transform.position, targetObject.transform.position);
                        if (colliderAudioSource[i].rolloffMode == AudioRolloffMode.Logarithmic) {
                            audibility = 1 / (1 + colliderAudioSource[i].maxDistance * (distance - 1));
                        } else { // linear
                            audibility = colliderAudioSource[i].volume * Mathf.Clamp01((distance - colliderAudioSource[i].minDistance) / (colliderAudioSource[i].maxDistance - colliderAudioSource[i].minDistance)); 
                        }
                        if (audibility > audibilityThreshold) {
                            return targetObject;
                        }
                    }
                }
            }
            return null;
        }

        // Draws the line of sight representation
        public static void DrawLineOfSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float angleOffset, float viewDistance, bool usePhysics2D)
        {
#if UNITY_EDITOR
            var oldColor = UnityEditor.Handles.color;
            var color = Color.yellow;
            color.a = 0.1f;
            UnityEditor.Handles.color = color;

            var halfFOV = fieldOfViewAngle * 0.5f + angleOffset;
            var beginDirection = Quaternion.AngleAxis(-halfFOV, (usePhysics2D ? transform.forward : transform.up)) * (usePhysics2D ? transform.up : transform.forward);
            UnityEditor.Handles.DrawSolidArc(transform.TransformPoint(positionOffset), (usePhysics2D ? transform.forward : transform.up), beginDirection, fieldOfViewAngle, viewDistance);

            UnityEditor.Handles.color = oldColor;
#endif
        }

        public static T GetComponentForType<T>(GameObject target) where T : Component
        {
            Dictionary<Type, Component> typeComponentMap;
            Component targetComponent;
            // Return the cached component if it exists.
            if (gameObjectComponentMap.TryGetValue(target, out typeComponentMap)) {
                if (typeComponentMap.TryGetValue(typeof(T), out targetComponent)) {
                    return targetComponent as T;
                }
            } else {
                // The cached component doesn't exist for the specified type.
                typeComponentMap = new Dictionary<Type, Component>();
                gameObjectComponentMap.Add(target, typeComponentMap);
            }

            // Find the component reference and cache the results.
            targetComponent = target.GetComponent<T>();
            typeComponentMap.Add(typeof(T), targetComponent);
            return targetComponent as T;
        }

        public static T GetParentComponentForType<T>(GameObject target) where T : Component
        {
            Dictionary<Type, Component> typeComponentMap;
            Component targetComponent;
            // Return the cached component if it exists.
            if (gameObjectParentComponentMap.TryGetValue(target, out typeComponentMap)) {
                if (typeComponentMap.TryGetValue(typeof(T), out targetComponent)) {
                    return targetComponent as T;
                }
            } else {
                // The cached component doesn't exist for the specified type.
                typeComponentMap = new Dictionary<Type, Component>();
                gameObjectParentComponentMap.Add(target, typeComponentMap);
            }

            // Find the component reference and cache the results.
            targetComponent = target.GetComponentInParent<T>();
            typeComponentMap.Add(typeof(T), targetComponent);
            return targetComponent as T;
        }

        public static T[] GetComponentsForType<T>(GameObject target) where T : Component
        {
            Dictionary<Type, Component[]> typeComponentsMap;
            Component[] targetComponents;
            // Return the cached component if it exists.
            if (gameObjectComponentsMap.TryGetValue(target, out typeComponentsMap)) {
                if (typeComponentsMap.TryGetValue(typeof(T), out targetComponents)) {
                    return targetComponents as T[];
                }
            } else {
                // The cached components doesn't exist for the specified type.
                typeComponentsMap = new Dictionary<Type, Component[]>();
                gameObjectComponentsMap.Add(target, typeComponentsMap);
            }

            // Find the component reference and cache the results.
            targetComponents = target.GetComponents<T>();
            typeComponentsMap.Add(typeof(T), targetComponents);
            return targetComponents as T[];
        }

        // Clears the static references.
        public static void ClearCache()
        {
            gameObjectComponentMap.Clear();
            gameObjectComponentsMap.Clear();
        }
    }
}