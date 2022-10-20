using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    public abstract class Movement : Action
    {
        /// <summary>
        /// Set a new pathfinding destination.
        /// </summary>
        /// <param name="destination">The destination to set.</param>
        /// <returns>True if the destination is valid.</returns>
        protected abstract bool SetDestination(Vector3 destination);

        /// <summary>
        /// Specifies if the rotation should be updated.
        /// </summary>
        /// <param name="update">Should the rotation be updated?</param>
        protected abstract void UpdateRotation(bool update);

        /// <summary>
        /// Does the agent have a pathfinding path?
        /// </summary>
        /// <returns>True if the agent has a pathfinding path.</returns>
        protected abstract bool HasPath();

        /// <summary>
        /// Returns the velocity of the agent.
        /// </summary>
        /// <returns>The velocity of the agent.</returns>
        protected abstract Vector3 Velocity();

        /// <summary>
        /// Has the agent arrived at the destination?
        /// </summary>
        /// <returns>True if the agent has arrived at the destination.</returns>
        protected abstract bool HasArrived();

        /// <summary>
        /// Stop pathfinding.
        /// </summary>
        protected abstract void Stop();
    }
}