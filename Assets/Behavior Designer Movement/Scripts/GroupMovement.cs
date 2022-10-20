using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    public abstract class GroupMovement : Action
    {
        protected abstract bool SetDestination(int index, Vector3 target);

        protected abstract Vector3 Velocity(int index);
    }
}