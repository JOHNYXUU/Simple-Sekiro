using Entity;
using StateMachine.Anim.Data;
using StateMachine.Base;
using StateMachine.Enum.AnimEnum;
using StateMachine.Enum.LogicEnum;
using UnityEngine;

namespace StateMachine.Anim.Idle
{
    public class DefenceIdle : PlayerAnimState
    {
        public DefenceIdle()
        {
            EnterMap = new AnimParamMap();
            EnterMap.BoolValues.Add(AnimParams.IsDefence, true);
            EnterMap.BoolValues.Add(AnimParams.IsMove, false);
            EnterMap.FloatValues.Add(AnimParams.SpeedValue, 0);
            

            UpdateMap = new AnimParamMap();
            UpdateMap.FloatValues.Add(AnimParams.SpeedValue, 0);
        }
        
        public override void Enter(PlayerEntity entity)
        {
            base.Enter(entity);

            entity.AttackBlock++;
            
            if(Config.showAnimState)
                Debug.LogError("DefenceIdle");
        }

        public override int UpdateAnim(PlayerEntity entity)
        {
            base.UpdateAnim(entity);

            if (entity.ArmState == (int)ArmState.Equip)
                return (int)AnimState.EquipIdle;

            if (entity.MoveState == (int)MoveState.Run)
                return (int)AnimState.DefenceRun;

            return -1;
        }

        public override void Leave(PlayerEntity entity)
        {
            base.Leave(entity);
            
            entity.AttackBlock--;
        }

        public override void UpdateEnterParam(PlayerEntity entity)
        {
            var speedOnGround = new Vector2(entity.Speed.x, entity.Speed.z);

            EnterMap.FloatValues[AnimParams.SpeedValue] = speedOnGround.magnitude * 100f;
        }

        public override void UpdateUpdateParam(PlayerEntity entity)
        {
            var speedOnGround = new Vector2(entity.Speed.x, entity.Speed.z);

            UpdateMap.FloatValues[AnimParams.SpeedValue] = speedOnGround.magnitude * 100f;
        }
    }
}