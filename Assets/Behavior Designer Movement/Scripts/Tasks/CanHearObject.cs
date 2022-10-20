using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Check to see if the any objects are within hearing range of the current agent.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanHearObjectIcon.png")]
    public class CanHearObject : Conditional
    {
        [Tooltip("Should the 2D version be used?")]
        public bool usePhysics2D;
        [Tooltip("The object that we are searching for")]
        public SharedGameObject targetObject;
        [Tooltip("The objects that we are searching for")]
        public SharedGameObjectList targetObjects;
        [Tooltip("The tag of the object that we are searching for")]
        public SharedString targetTag;
        [Tooltip("The LayerMask of the objects that we are searching for")]
        public LayerMask objectLayerMask;
        [Tooltip("If using the object layer mask, specifies the maximum number of colliders that the physics cast can collide with")]
        public int maxCollisionCount = 200;
        [Tooltip("How far away the unit can hear")]
        public SharedFloat hearingRadius = 50;
        [Tooltip("The further away a sound source is the less likely the agent will be able to hear it. " +
                 "Set a threshold for the the minimum audibility level that the agent can hear")]
        public SharedFloat audibilityThreshold = 0.05f;
        [Tooltip("The hearing offset relative to the pivot position")]
        public SharedVector3 offset;
        [Tooltip("The returned object that is heard")]
        public SharedGameObject returnedObject;

        private Collider[] overlapColliders;
        private Collider2D[] overlap2DColliders;

        // Returns success if an object was found otherwise failure
        public override TaskStatus OnUpdate()
        {
            if (targetObjects.Value != null && targetObjects.Value.Count > 0) { // If there are objects in the group list then search for the object within that list
                GameObject objectFound = null;
                for (int i = 0; i < targetObjects.Value.Count; ++i) {
                    float audibility = 0;
                    GameObject obj;
                    if (Vector3.Distance(targetObjects.Value[i].transform.position, transform.position) < hearingRadius.Value) {
                        if ((obj = MovementUtility.WithinHearingRange(transform, offset.Value, audibilityThreshold.Value, targetObjects.Value[i], ref audibility)) != null) {
                            objectFound = obj;
                        }
                    }
                }
                returnedObject.Value = objectFound;
            } else if (targetObject.Value == null) { // If the target object is null then determine if there are any objects within hearing range based on the layer mask
                if (usePhysics2D) {
                    if (overlap2DColliders == null) {
                        overlap2DColliders = new Collider2D[maxCollisionCount];
                    }
                    returnedObject.Value = MovementUtility.WithinHearingRange2D(transform, offset.Value, audibilityThreshold.Value, hearingRadius.Value, overlap2DColliders, objectLayerMask);
                } else {
                    if (overlapColliders == null) {
                        overlapColliders = new Collider[maxCollisionCount];
                    }
                    returnedObject.Value = MovementUtility.WithinHearingRange(transform, offset.Value, audibilityThreshold.Value, hearingRadius.Value, overlapColliders, objectLayerMask);
                }
            } else if (!string.IsNullOrEmpty(targetTag.Value)) {
                GameObject objectFound = null;
                var targets = GameObject.FindGameObjectsWithTag(targetTag.Value);
                for (int i = 0; i < targets.Length; ++i) {
                    float audibility = 0;
                    GameObject obj;
                    if (Vector3.Distance(targetObjects.Value[i].transform.position, transform.position) < hearingRadius.Value) {
                        if ((obj = MovementUtility.WithinHearingRange(transform, offset.Value, audibilityThreshold.Value, targetObjects.Value[i], ref audibility)) != null) {
                            objectFound = obj;
                        }
                    }
                }
                returnedObject.Value = objectFound;
            } else {
                var target = targetObject.Value;
                if (Vector3.Distance(target.transform.position, transform.position) < hearingRadius.Value) {
                    returnedObject.Value = MovementUtility.WithinHearingRange(transform, offset.Value, audibilityThreshold.Value, targetObject.Value);
                }
            }

            if (returnedObject.Value != null) {
                // returnedObject success if an object was heard
                return TaskStatus.Success;
            }
            // An object is not within heard so return failure
            return TaskStatus.Failure;
        }

        // Reset the public variables
        public override void OnReset()
        {
            hearingRadius = 50;
            audibilityThreshold = 0.05f;
        }

        // Draw the hearing radius
        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (Owner == null || hearingRadius == null) {
                return;
            }
            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(Owner.transform.position, Owner.transform.up, hearingRadius.Value);
            UnityEditor.Handles.color = oldColor;
#endif
        }

        public override void OnBehaviorComplete()
        {
            MovementUtility.ClearCache();
        }
    }
}