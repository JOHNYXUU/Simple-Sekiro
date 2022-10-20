using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Check to see if the any object specified by the object list or tag is within the distance specified of the current agent.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}WithinDistanceIcon.png")]
    public class WithinDistance : Conditional
    {
        [Tooltip("Should the 2D version be used?")]
        public bool usePhysics2D;
        [Tooltip("The object that we are searching for")]
        public SharedGameObject targetObject;
        [Tooltip("The tag of the object that we are searching for")]
        public SharedString targetTag;
        [Tooltip("The LayerMask of the objects that we are searching for")]
        public LayerMask objectLayerMask;
        [Tooltip("If using the object layer mask, specifies the maximum number of colliders that the physics cast can collide with")]
        public int maxCollisionCount = 200;
        [Tooltip("The distance that the object needs to be within")]
        public SharedFloat magnitude = 5;
        [Tooltip("If true, the object must be within line of sight to be within distance. For example, if this option is enabled then an object behind a wall will not be within distance even though it may " +
                 "be physically close to the other object")]
        public SharedBool lineOfSight;
        [Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
        public LayerMask ignoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        [Tooltip("The raycast offset relative to the pivot position")]
        public SharedVector3 offset;
        [Tooltip("The target raycast offset relative to the pivot position")]
        public SharedVector3 targetOffset;
        [Tooltip("Should a debug look ray be drawn to the scene view?")]
        public SharedBool drawDebugRay;
        [Tooltip("The object variable that will be set when a object is found what the object is")]
        public SharedGameObject returnedObject;

        private List<GameObject> objects;
        // distance * distance, optimization so we don't have to take the square root
        private float sqrMagnitude;
        private bool overlapCast = false;
        private Collider[] overlapColliders;
        private Collider2D[] overlap2DColliders;

        public override void OnStart()
        {
            sqrMagnitude = magnitude.Value * magnitude.Value;

            if (objects != null) {
                objects.Clear();
            } else {
                objects = new List<GameObject>();
            }
            // if objects is null then find all of the objects using the layer mask or tag
            if (targetObject.Value == null) {
                if (!string.IsNullOrEmpty(targetTag.Value)) {
                    var gameObjects = GameObject.FindGameObjectsWithTag(targetTag.Value);
                    for (int i = 0; i < gameObjects.Length; ++i) {
                        objects.Add(gameObjects[i]);
                    }
                } else {
                    overlapCast = true;
                }
            } else {
                objects.Add(targetObject.Value);
            }
        }

        // returns success if any object is within distance of the current object. Otherwise it will return failure
        public override TaskStatus OnUpdate()
        {
            if (transform == null || objects == null)
                return TaskStatus.Failure;

            if (overlapCast) {
                objects.Clear();
                if (usePhysics2D) {
                    if (overlap2DColliders == null) {
                        overlap2DColliders = new Collider2D[maxCollisionCount];
                    }
                    var count = Physics2D.OverlapCircleNonAlloc(transform.position, magnitude.Value, overlap2DColliders, objectLayerMask.value);
                    for (int i = 0; i < count; ++i) {
                        objects.Add(overlap2DColliders[i].gameObject);
                    }
                } else {
                    if (overlapColliders == null) {
                        overlapColliders = new Collider[maxCollisionCount];
                    }
                    var count = Physics.OverlapSphereNonAlloc(transform.position, magnitude.Value, overlapColliders, objectLayerMask.value);
                    for (int i = 0; i < count; ++i) {
                        objects.Add(overlapColliders[i].gameObject);
                    }
                }
            }

            Vector3 direction;
            // check each object. All it takes is one object to be able to return success
            for (int i = 0; i < objects.Count; ++i) {
                if (objects[i] == null || objects[i] == gameObject) {
                    continue;
                }
                direction = objects[i].transform.position - (transform.position + offset.Value);
                // check to see if the square magnitude is less than what is specified
                if (Vector3.SqrMagnitude(direction) < sqrMagnitude) {
                    // the magnitude is less. If lineOfSight is true do one more check
                    if (lineOfSight.Value) {
                        var hitTransform = MovementUtility.LineOfSight(transform, offset.Value, objects[i], targetOffset.Value, usePhysics2D, ignoreLayerMask.value, drawDebugRay.Value);
                        if (hitTransform != null && MovementUtility.IsAncestor(hitTransform, objects[i].transform)) {
                            // the object has a magnitude less than the specified magnitude and is within sight. Set the object and return success
                            returnedObject.Value = objects[i];
                            return TaskStatus.Success;
                        }
                    } else {
                        // the object has a magnitude less than the specified magnitude. Set the object and return success
                        returnedObject.Value = objects[i];
                        return TaskStatus.Success;
                    }
                }
            }
            // no objects are within distance. Return failure
            return TaskStatus.Failure;
        }

        public override void OnReset()
        {
            usePhysics2D = false;
            targetObject = null;
            targetTag = string.Empty;
            objectLayerMask = 0;
            magnitude = 5;
            lineOfSight = true;
            ignoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
            offset = Vector3.zero;
            targetOffset = Vector3.zero;
        }

        // Draw the seeing radius
        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (Owner == null || magnitude == null) {
                return;
            }
            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(Owner.transform.position, usePhysics2D ? Owner.transform.forward : Owner.transform.up, magnitude.Value);
            UnityEditor.Handles.color = oldColor;
#endif
        }
    }
}