using System;
using Controllers;
using Entity;
using UnityEngine;

namespace StateMachine.Base
{
    public class PlayerLogicState : PlayerState
    {
        protected static Vector3 GroundTest = new Vector3(0f, -0.2f, 0f);

        protected bool HasMove()
        {
            bool res = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) ||
                        Input.GetKey(KeyCode.D)) &&
                       !(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) &&
                         Input.GetKey(KeyCode.D)) &&
                       !(Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) &&
                         !Input.GetKey(KeyCode.D)) &&
                       !(!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) &&
                         Input.GetKey(KeyCode.D));
            return res;
        }

        protected bool Jump()
        {
            return Input.GetKey(KeyCode.Space);
        }

        protected void RotateFromCamera(Transform camera, Transform player, PlayerEntity entity)
        {
            var angle = GetMoveAngle();
            
            if(Math.Abs(angle - -1f) < 0.1f)
                return;

            var cameraYaw = camera.rotation.eulerAngles.y;

            var playerYaw = cameraYaw + angle;

            var rot = Quaternion.Euler(0f, playerYaw, 0f);

            player.rotation = Quaternion.Slerp(player.rotation, rot, 0.3f);

            entity.ViewYaw = playerYaw;
        }
        
        protected void Rotate2LockPoint(Transform player, PlayerEntity entity)
        {
            var lockedEnemy = CameraController.LockedEnemy;
            
            var lockPoint = lockedEnemy.transform.position;

            lockPoint.y += lockedEnemy.gameObject.GetComponent<CapsuleCollider>().height * 0.75f;

            var forwardVec = lockPoint - player.position;
            forwardVec.y = 0;

            var nowRotation = player.rotation;

            player.forward = forwardVec;

            var finalRotation = player.rotation;

            player.rotation = Quaternion.Slerp(nowRotation, finalRotation, 0.2f);

            // Debug.LogError(Vector3.Angle(forward, player.forward));

            entity.ViewYaw = player.rotation.eulerAngles.y;
        }

        protected float GetMoveAngle()
        {
            if (!HasMove())
                return -1f;

            var vec = new Vector2();
            
            if (Input.GetKey(KeyCode.W))
                vec.y++;

            if (Input.GetKey(KeyCode.S))
                vec.y--;
            
            if (Input.GetKey(KeyCode.A))
                vec.x--;

            if (Input.GetKey(KeyCode.D))
                vec.x++;

            return Vector2.SignedAngle(vec, new Vector2(0, 1));
        }

        protected Vector3 CalcSpeedOnGround(Vector3 speed, float accSpeed, float maxSpeed, float brakeAcc, float viewYaw, bool isLock)
        {
            var speedOnGround = new Vector3(speed.x, 0f, speed.z);

            var dir = Quaternion.Euler(0f, viewYaw, 0f) * Vector3.forward;
            
            
            if(Vector3.Angle(dir,speedOnGround) < 120f)
                speedOnGround += accSpeed * dir.normalized * Time.deltaTime;
            else
                speedOnGround += 2 * accSpeed * dir.normalized * Time.deltaTime;


            if(!isLock)
                speedOnGround = dir.normalized * speedOnGround.magnitude;

            if (speedOnGround.magnitude > maxSpeed)
            {
                speedOnGround = BrakeSpeedOnGround(speedOnGround, brakeAcc);

                if (speedOnGround.magnitude < maxSpeed)
                    speedOnGround = speedOnGround.normalized * maxSpeed;
            }

            speed.x = speedOnGround.x;
            speed.z = speedOnGround.z;

            return speed;
        }

        protected Vector3 ShrinkSpeed(Vector3 speed, float maxSpeed)
        {
            if (speed.magnitude > maxSpeed)
                speed = speed.normalized * maxSpeed;

            return speed;
        }

        protected Vector3 BrakeSpeedOnGround(Vector3 speed, float brakeAccSpeed)
        {
            var speedOnGround = new Vector3(speed.x, 0f, speed.z);

            var speedValue = speedOnGround.magnitude;

            speedValue -= Time.deltaTime * brakeAccSpeed;
            speedValue = Mathf.Max(0f, speedValue);

            speedOnGround = speedOnGround.normalized * speedValue;

            speed.x = speedOnGround.x;
            speed.z = speedOnGround.z;

            return speed;
        }

        protected bool IsMoveForward(CharacterController cc,PlayerEntity entity)
        {
            var speedOnGround = new Vector3(entity.Speed.x, 0f, entity.Speed.z);
            return Vector3.Angle(cc.gameObject.transform.forward, speedOnGround) <= 20f;
        }

        protected Vector3 FixSpeed(Vector3 dis, float deltaTime, Vector3 speed)
        {
            if (Math.Abs(dis.x / deltaTime - speed.x) > 0.1f)
            {
                speed.x = dis.x / deltaTime;
            }
            
            if (Math.Abs(dis.z / deltaTime - speed.z) > 0.1f)
            {
                speed.z = dis.z / deltaTime;
            }

            return speed;
        }

        protected Vector3 Move(CharacterController cc, PlayerEntity entity,Vector3 dis)
        {
            var lastPos = cc.transform.position;
            
            entity.CollisionFlag = (int)cc.Move(dis);

            var nowPos = cc.transform.position;

            var realDis = nowPos - lastPos;

            return realDis;
        }

        protected float NormalizeYaw(float yaw)
        {
            while (yaw > 180f)
                yaw -= 360f;

            while (yaw < -180f)
                yaw += 360f;

            return yaw;
        }
    }
}