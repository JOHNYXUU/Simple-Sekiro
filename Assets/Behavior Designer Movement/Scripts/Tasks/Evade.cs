using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Evade the target specified using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}EvadeIcon.png")]
    public class Evade : NavMeshMovement
    {
        [Tooltip("The agent has evaded when the magnitude is greater than this value")]
        public SharedFloat evadeDistance = 10;
        [Tooltip("The distance to look ahead when evading")]
        public SharedFloat lookAheadDistance = 5;
        [Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
        public SharedFloat targetDistPrediction = 20;
        [Tooltip("Multiplier for predicting the look ahead distance")]
        public SharedFloat targetDistPredictionMult = 20;
        [Tooltip("The GameObject that the agent is evading")]
        public SharedGameObject target;

        // The position of the target at the last frame
        private Vector3 targetPosition;

        public override void OnStart()
        {
            base.OnStart();

            targetPosition = target.Value.transform.position;
            SetDestination(Target());
        }

        // Evade from the target. Return success once the agent has fleed the target by moving far enough away from it
        // Return running if the agent is still fleeing
        public override TaskStatus OnUpdate()
        {
            if (Vector3.Magnitude(transform.position - target.Value.transform.position) > evadeDistance.Value) {
                return TaskStatus.Success;
            }

            SetDestination(Target());

            return TaskStatus.Running;
        }

        // Evade in the opposite direction
        private Vector3 Target()
        {
            // Calculate the current distance to the target and the current speed
            var distance = (target.Value.transform.position - transform.position).magnitude;
            var speed = Velocity().magnitude;

            float futurePrediction = 0;
            // Set the future prediction to max prediction if the speed is too small to give an accurate prediction
            if (speed <= distance / targetDistPrediction.Value) {
                futurePrediction = targetDistPrediction.Value;
            } else {
                futurePrediction = (distance / speed) * targetDistPredictionMult.Value; // the prediction should be accurate enough
            }

            // Predict the future by taking the velocity of the target and multiply it by the future prediction
            var prevTargetPosition = targetPosition;
            targetPosition = target.Value.transform.position;
            var position = targetPosition + (targetPosition - prevTargetPosition) * futurePrediction;

            return transform.position + (transform.position - position).normalized * lookAheadDistance.Value;
        }

        // Reset the public variables
        public override void OnReset()
        {
            base.OnReset();

            evadeDistance = 10;
            lookAheadDistance = 5;
            targetDistPrediction = 20;
            targetDistPredictionMult = 20;
            target = null;
        }
    }
}