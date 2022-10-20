using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Check to see if the any objects are within sight of the agent.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
    public class CanSeeObject : Conditional
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
        [Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
        public LayerMask ignoreLayerMask;
        [Tooltip("The field of view angle of the agent (in degrees)")]
        public SharedFloat fieldOfViewAngle = 90;
        [Tooltip("The distance that the agent can see")]
        public SharedFloat viewDistance = 1000;
        [Tooltip("The raycast offset relative to the pivot position")]
        public SharedVector3 offset;
        [Tooltip("The target raycast offset relative to the pivot position")]
        public SharedVector3 targetOffset;
        [Tooltip("The offset to apply to 2D angles")]
        public SharedFloat angleOffset2D;
        [Tooltip("Should the target bone be used?")]
        public SharedBool useTargetBone;
        [Tooltip("The target's bone if the target is a humanoid")]
        public HumanBodyBones targetBone;
        [Tooltip("Should a debug look ray be drawn to the scene view?")]
        public SharedBool drawDebugRay;
        [Tooltip("Should the agent's layer be disabled before the Can See Object check is executed?")]
        public SharedBool disableAgentColliderLayer;
        [Tooltip("The object that is within sight")]
        public SharedGameObject returnedObject;

        private GameObject[] agentColliderGameObjects;
        private int[] originalColliderLayer;
        private Collider[] overlapColliders;
        private Collider2D[] overlap2DColliders;

        private int ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");

        // Returns success if an object was found otherwise failure
        public override TaskStatus OnUpdate()
        {
            // The collider layers on the agent can be set to ignore raycast to prevent them from interferring with the raycast checks.
            if (disableAgentColliderLayer.Value) {
                if (agentColliderGameObjects == null) {
                    if (usePhysics2D) {
                        var colliders = gameObject.GetComponentsInChildren<Collider2D>();
                        agentColliderGameObjects = new GameObject[colliders.Length];
                        for (int i = 0; i < agentColliderGameObjects.Length; ++i) {
                            agentColliderGameObjects[i] = colliders[i].gameObject;
                        }
                    } else {
                        var colliders = gameObject.GetComponentsInChildren<Collider>();
                        agentColliderGameObjects = new GameObject[colliders.Length];
                        for (int i = 0; i < agentColliderGameObjects.Length; ++i) {
                            agentColliderGameObjects[i] = colliders[i].gameObject;
                        }
                    }
                    originalColliderLayer = new int[agentColliderGameObjects.Length];
                }

                // Change the layer. Remember the previous layer so it can be reset after the check has completed.
                for (int i = 0; i < agentColliderGameObjects.Length; ++i) {
                    originalColliderLayer[i] = agentColliderGameObjects[i].layer;
                    agentColliderGameObjects[i].layer = ignoreRaycastLayer;
                }
            }

            if (usePhysics2D) {
                if (targetObjects.Value != null && targetObjects.Value.Count > 0) { // If there are objects in the group list then search for the object within that list
                    GameObject objectFound = null;
                    float minAngle = Mathf.Infinity;
                    for (int i = 0; i < targetObjects.Value.Count; ++i) {
                        float angle;
                        GameObject obj;
                        if ((obj = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObjects.Value[i], targetOffset.Value, true, angleOffset2D.Value, out angle, ignoreLayerMask, useTargetBone.Value, targetBone, drawDebugRay.Value)) != null) {
                            // This object is within sight. Set it to the objectFound GameObject if the angle is less than any of the other objects
                            if (angle < minAngle) {
                                minAngle = angle;
                                objectFound = obj;
                            }
                        }
                    }
                    returnedObject.Value = objectFound;
                } else if (targetObject.Value != null) { // If the target is not null then determine if that object is within sight
                    returnedObject.Value = MovementUtility.WithinSight2D(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObject.Value, targetOffset.Value, angleOffset2D.Value, ignoreLayerMask, useTargetBone.Value, targetBone, drawDebugRay.Value);
                } else if (!string.IsNullOrEmpty(targetTag.Value)) { // If the target tag is not null then determine if there are any objects within sight based on the tag
                    GameObject objectFound = null;
                    float minAngle = Mathf.Infinity;
                    var targets = GameObject.FindGameObjectsWithTag(targetTag.Value);
                    for (int i = 0; i < targets.Length; ++i) {
                        float angle;
                        GameObject obj;
                        if ((obj = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targets[i], targetOffset.Value, true, angleOffset2D.Value, out angle, ignoreLayerMask, useTargetBone.Value, targetBone, drawDebugRay.Value)) != null) {
                            // This object is within sight. Set it to the objectFound GameObject if the angle is less than any of the other objects
                            if (angle < minAngle) {
                                minAngle = angle;
                                objectFound = obj;
                            }
                        }
                    }
                    returnedObject.Value = objectFound;
                } else { // If the target object is null and there is no tag then determine if there are any objects within sight based on the layer mask
                    if (overlap2DColliders == null) {
                        overlap2DColliders = new Collider2D[maxCollisionCount];
                    }
                    returnedObject.Value = MovementUtility.WithinSight2D(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, overlap2DColliders, objectLayerMask, targetOffset.Value, angleOffset2D.Value, ignoreLayerMask, drawDebugRay.Value);
                }
            } else {
                if (targetObjects.Value != null && targetObjects.Value.Count > 0) { // If there are objects in the group list then search for the object within that list
                    GameObject objectFound = null;
                    float minAngle = Mathf.Infinity;
                    for (int i = 0; i < targetObjects.Value.Count; ++i) {
                        float angle;
                        GameObject obj;
                        if ((obj = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObjects.Value[i], targetOffset.Value, false, angleOffset2D.Value, out angle, ignoreLayerMask, useTargetBone.Value, targetBone, drawDebugRay.Value)) != null) {
                            // This object is within sight. Set it to the objectFound GameObject if the angle is less than any of the other objects
                            if (angle < minAngle) {
                                minAngle = angle;
                                objectFound = obj;
                            }
                        }
                    }
                    returnedObject.Value = objectFound;
                } else if (targetObject.Value != null) { // If the target is not null then determine if that object is within sight
                    returnedObject.Value = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObject.Value, targetOffset.Value, ignoreLayerMask, useTargetBone.Value, targetBone, drawDebugRay.Value);
                } else if (!string.IsNullOrEmpty(targetTag.Value)) { // If the target tag is not null then determine if there are any objects within sight based on the tag
                    GameObject objectFound = null;
                    float minAngle = Mathf.Infinity;
                    var targets = GameObject.FindGameObjectsWithTag(targetTag.Value);
                    for (int i = 0; i < targets.Length; ++i) {
                        float angle;
                        GameObject obj;
                        if ((obj = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targets[i], targetOffset.Value, false, angleOffset2D.Value, out angle, ignoreLayerMask, useTargetBone.Value, targetBone, drawDebugRay.Value)) != null) {
                            // This object is within sight. Set it to the objectFound GameObject if the angle is less than any of the other objects
                            if (angle < minAngle) {
                                minAngle = angle;
                                objectFound = obj;
                            }
                        }
                    }
                    returnedObject.Value = objectFound;
                } else { // If the target object is null and there is no tag then determine if there are any objects within sight based on the layer mask
                    if (overlapColliders == null) {
                        overlapColliders = new Collider[maxCollisionCount];
                    }
                    returnedObject.Value = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, overlapColliders, objectLayerMask, targetOffset.Value, ignoreLayerMask, useTargetBone.Value, targetBone, drawDebugRay.Value);
                }
            }

            if (disableAgentColliderLayer.Value) {
                for (int i = 0; i < agentColliderGameObjects.Length; ++i) {
                    agentColliderGameObjects[i].layer = originalColliderLayer[i];
                }
            }

            if (returnedObject.Value != null) {
                // Return success if an object was found
                return TaskStatus.Success;
            }
            // An object is not within sight so return failure
            return TaskStatus.Failure;
        }

        // Reset the public variables
        public override void OnReset()
        {
            fieldOfViewAngle = 90;
            viewDistance = 1000;
            offset = Vector3.zero;
            targetOffset = Vector3.zero;
            angleOffset2D = 0;
            targetTag = "";
            drawDebugRay = false;
            ignoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        }

        // Draw the line of sight representation within the scene window
        public override void OnDrawGizmos()
        {
            MovementUtility.DrawLineOfSight(Owner.transform, offset.Value, fieldOfViewAngle.Value, angleOffset2D.Value, viewDistance.Value, usePhysics2D);
        }

        public override void OnBehaviorComplete()
        {
            MovementUtility.ClearCache();
        }
    }
}