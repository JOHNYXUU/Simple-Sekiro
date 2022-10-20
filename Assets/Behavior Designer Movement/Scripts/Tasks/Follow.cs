using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Follows the specified target using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}FollowIcon.png")]
    public class Follow : NavMeshMovement
    {
        [Tooltip("The GameObject that the agent is following")]
        public SharedGameObject target;
        [Tooltip("Start moving towards the target if the target is further than the specified distance")]
        public SharedFloat moveDistance = 2;

        private Vector3 lastTargetPosition;
        private bool hasMoved;

        public override void OnStart()
        {
            base.OnStart();

            if (target.Value == null) {
                return;
            }

            lastTargetPosition = target.Value.transform.position + Vector3.one * (moveDistance.Value + 1);
            hasMoved = false;
        }

        // Follow the target. The task will never return success as the agent should continue to follow the target even after arriving at the destination.
        public override TaskStatus OnUpdate()
        {
            if (target.Value == null) {
                return TaskStatus.Failure;
            }

            // Move if the target has moved more than the moveDistance since the last time the agent moved.
            var targetPosition = target.Value.transform.position;
            if ((targetPosition - lastTargetPosition).magnitude >= moveDistance.Value) {
                SetDestination(targetPosition);
                lastTargetPosition = targetPosition;
                hasMoved = true;
            } else {
                // Stop moving if the agent is within the moveDistance of the target.
                if (hasMoved && (targetPosition - transform.position).magnitude < moveDistance.Value) {
                    Stop();
                    hasMoved = false;
                    lastTargetPosition = targetPosition;
                }
            }

            return TaskStatus.Running;
        }

        public override void OnReset()
        {
            base.OnReset();
            target = null;
            moveDistance = 2;
        }
    }
}