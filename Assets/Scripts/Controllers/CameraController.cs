using System.Collections.Generic;
using Entity;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public enum CameraMoveType
    {
        Free,
        Lock,
    }
    public class CameraController
    {
        public GameObject MyCamera => GameObject.FindWithTag("MainCamera");

        private CameraMoveType _moveType;

        private CharacterController _cc;
        
        private float _disCamera2Player;

        private Vector3 _playerLastPos;

        private Vector3 _cameraPos;
        
        public GameConfig Config => GameObject.Find("Config").GetComponent<GameConfig>();

        public GameObject LockPoint => GameObject.Find("LockPoint");

        public static GameObject LockedEnemy;

        public CameraController(GameObject myPlayer,PlayerEntity entity)
        {
            _moveType = CameraMoveType.Free;
            entity.CameraState = (int)CameraMoveType.Free;

            var playerTrans = myPlayer.transform;
            _cc = myPlayer.GetComponent<CharacterController>();
            
            _cameraPos = new Vector3(playerTrans.position.x, playerTrans.position.y, playerTrans.position.z);
            _cameraPos += Config.cameraUpwardOffset * playerTrans.up;
            _cameraPos += Config.cameraForwardOffset * playerTrans.forward;
            
            MyCamera.transform.position = _cameraPos;

            var center = playerTrans.position + _cc.height * 0.75f * playerTrans.up;
            MyCamera.transform.LookAt(center);

            _disCamera2Player = Vector3.Distance(MyCamera.transform.position, center);
            _playerLastPos = new Vector3(playerTrans.position.x, playerTrans.position.y, playerTrans.position.z);

            LockPoint.GetComponent<Image>().enabled = false;
        }

        public void Update(GameObject myPlayer, PlayerEntity entity)
        {
            if (Input.GetMouseButton(2) && !MgrInput.MidMouseButtonContinue)
            {
                switch (_moveType)
                {
                    case CameraMoveType.Free:
                        if (TryLockEnemy(entity, myPlayer.transform.position))
                        {
                            ChangeMoveType(CameraMoveType.Lock,entity, myPlayer);
                        }
                        break;
                    case CameraMoveType.Lock:
                        ChangeMoveType(CameraMoveType.Free,entity, myPlayer);
                        break;
                }
            }

            if (LockedEnemy)
            {
                var enemyCenterPos = LockedEnemy.gameObject.transform.position;
                enemyCenterPos.y += LockedEnemy.gameObject.GetComponent<CapsuleCollider>().height * 0.75f;
                LockPointUpdateDirectly(enemyCenterPos);
            }

            switch (_moveType)
            {
                case CameraMoveType.Free:
                    CameraUpdateFree(myPlayer);
                    break;
                case CameraMoveType.Lock:
                    if (!CameraUpdateLock(myPlayer))
                    {
                        ChangeMoveType(CameraMoveType.Free,entity, myPlayer);
                    }
                    break;
            }
        }

        public void ChangeMoveType(CameraMoveType type, PlayerEntity entity, GameObject player)
        {
            _moveType = type;
            entity.CameraState = (int)type;
            if (type == CameraMoveType.Free)
            {
                LockedEnemy = null;
                _playerLastPos = player.transform.position;
                LockPoint.GetComponent<Image>().enabled = false;
            }
            else
            {
                LockPoint.GetComponent<Image>().enabled = true;
            }
        }

        private void CameraUpdateFree(GameObject myPlayer)
        {
            var playerTrans = myPlayer.transform;
            var playerUp = playerTrans.up;
            var center = playerTrans.position + _cc.height * 0.75f * playerUp;
            
            var playerMoveVec = playerTrans.position - _playerLastPos;
            _playerLastPos = playerTrans.position;

            _cameraPos += playerMoveVec;

            // if (!_cc.isGrounded)
            // {
                MyCamera.transform.position = _cameraPos;
            // }
            // else
            // {
            //     MyCamera.transform.position = Vector3.Lerp(MyCamera.transform.position, _cameraPos, 0.1f);
            // }
            var player2CameraVec = MyCamera.transform.position - center;
            var player2FinalCameraVec = _cameraPos - center;
            
            var mouseMoveX = Input.GetAxis("Mouse X");
            var cameraRotationDegreeHorizon = mouseMoveX * Config.mouseHorizonTalRatio;
            if (cameraRotationDegreeHorizon != 0f)
            {
                var rotationHorizon = Quaternion.AngleAxis(cameraRotationDegreeHorizon, playerUp);
                player2CameraVec = rotationHorizon * player2CameraVec;
                player2FinalCameraVec = rotationHorizon * player2FinalCameraVec;
            }
        
            var mouseMoveY = Input.GetAxis("Mouse Y");
            var cameraRotationDegreeVertical = mouseMoveY * Config.mouseVerticalRatio;
            if (cameraRotationDegreeVertical != 0f)
            {
                var rotationVertical =
                    Quaternion.AngleAxis(cameraRotationDegreeVertical, Vector3.Cross(Vector3.up, player2CameraVec));
                var temp = rotationVertical * player2CameraVec;
                var finalTemp = rotationVertical * player2FinalCameraVec;
                if (Vector3.Angle(Vector3.up, temp) is <= 120f and >= 30f)
                {
                    player2CameraVec = temp;
                    player2FinalCameraVec = finalTemp;
                }
            }
            
            _cameraPos = player2FinalCameraVec + center;
            MyCamera.transform.position =  player2CameraVec + center;

            // var nowRotation = MyCamera.transform.rotation;
            
            MyCamera.transform.LookAt(center);

            // var finalRotation = MyCamera.transform.rotation;
            //
            // MyCamera.transform.rotation = Quaternion.Slerp(nowRotation, finalRotation, 0.1f);
        }
        
        private bool CameraUpdateLock(GameObject myPlayer)
        {
            var playerTrans = myPlayer.transform;

            _cameraPos = new Vector3(playerTrans.position.x, playerTrans.position.y, playerTrans.position.z);

            var forward = LockedEnemy.transform.position - myPlayer.transform.position;
            
            _cameraPos += Config.cameraUpwardOffset * playerTrans.up;
            _cameraPos += Config.cameraForwardOffset * forward.normalized;

            MyCamera.transform.position = _cameraPos;
            
            var center = LockedEnemy.gameObject.transform.position;
            center.y += LockedEnemy.gameObject.GetComponent<CapsuleCollider>().height * 0.75f;
            
            var nowRotation = MyCamera.transform.rotation;
            
            MyCamera.transform.LookAt(center);

            var finalRotation = MyCamera.transform.rotation;

            // if(_cc.isGrounded)
            // MyCamera.transform.rotation = Quaternion.Slerp(nowRotation, finalRotation, 0.7f);
            // else
            // {
            MyCamera.transform.rotation = finalRotation;
            // }

            if (Vector3.Distance(center, myPlayer.transform.position) > Config.maxLockDistance)
                return false;

            return true;
        }

        private void LockPointUpdateWithLerp(Vector3 worldPos, float lerpRate = 0.1f)
        {
            if (Camera.main != null)
            {
                var nowPos = LockPoint.GetComponent<RectTransform>().position;
                LockPoint.GetComponent<RectTransform>().position = Vector3.Lerp(nowPos, Camera.main.WorldToScreenPoint(worldPos), lerpRate);
            }
        }
        
        private void LockPointUpdateDirectly(Vector3 worldPos)
        {
            if (Camera.main != null)
            {
                // Debug.LogError(1);
                LockPoint.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(worldPos);
            }
        }

        private bool TryLockEnemy(PlayerEntity entity, Vector3 playerPos)
        {
            var cameraForward = MyCamera.gameObject.transform.forward;

            var cameraPos = MyCamera.gameObject.transform.position;
            
            var cols = Physics.OverlapSphere(playerPos, Config.maxLockDistance);

            var minDegree = 200f;

            GameObject lockEnemy = null;
            
            foreach (var col in cols)
            {
                var camera2EnemyVec = col.gameObject.transform.position - cameraPos;
                var degree = Vector3.Angle(cameraForward, camera2EnemyVec);
                if (col.gameObject.CompareTag("Enemy") && degree < Config.maxLockDegree)
                {
                    if (degree < minDegree)
                    {
                        minDegree = degree;
                        lockEnemy = col.gameObject;
                    }
                }
            }

            LockedEnemy = lockEnemy;

            // if (LockedEnemy is not null)
            // {
            //     var forward = LockedEnemy.transform.position - playerPos;
            //     forward.y = 0f;
            //     var playerForward = Quaternion.Euler(0, entity.ViewYaw, 0) * Vector3.forward;
            //
            //     entity.Forward2WantDirAngle = Vector3.SignedAngle(playerForward, forward, Vector3.up);
            // }
            
            return lockEnemy is not null;
        }
    }
}