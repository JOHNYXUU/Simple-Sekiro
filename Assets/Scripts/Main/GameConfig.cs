using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameConfig : MonoBehaviour
{
    public bool showState = false;

    public bool showAnimState = false;

    public float pressClickBoundary = 0.15f;
    
    [Space(10)] 
    public float gravity = 9.8f;

    public float jumpStartSpeed = 6.0f;
    
    [Space(10)]
    public float cameraForwardOffset = -3f;

    public float cameraUpwardOffset = 2.7f;

    public float mouseHorizonTalRatio = 2f;

    public float mouseVerticalRatio = 2f;

    [Space(10)]
    public float dodgeAnimTime = 0.7f;

    public float dodgeSpeed = 5f;

    [Space(10)] 
    public float standAccSpeed = 10f;

    public float standBrakeAccSpeed = 20f;
    
    public float standWalkMaxSpeedF = 2.5f;
    
    public float standRunMaxSpeedF = 4.0f;

    public float standSprintMaxSpeed = 6.0f;

    [Space(10)] 
    public float lightAttack01NextTime = 0.6f;
    
    public float lightAttack01EndTime = 1.2f;

    public float heavyAttackNextTime = 1.4f;
    
    public float heavyAttackEndTime = 1.45f;

    public int lightAttackNum = 6;

    [Space(10)] 
    public float maxLockDegree = 30f;

    public float maxLockDistance = 10f;

    [Space(10)] 
    public float enemyHpUIMaxViewDis = 5.0f;

    public int lightAttackDamage = 10;

    public int heavyAttackDamage = 15;

    public float lightAttack01AudioBeginTime = 0.17f;
    
    public float lightAttack02AudioBeginTime = 0.17f;
    
    public float lightAttack03AudioBeginTime = 0.17f;
    
    public float lightAttack04AudioBeginTime = 0.17f;
    
    public float lightAttack05AudioBeginTime = 0.17f;
    
    public float lightAttack06AudioBeginTime = 0.17f;
    
    public float heavyAttackAudioBeginTime = 0.17f;
    

    [Space(10)] 
    public string bladeHitFleshAudioPath = "Audio/Sword_Slash_Flesh_And_Bones_";

    public int bladeHitFleshAudioNum = 4;

    public string bladeWooshAudioPath = "Audio/Woosh_Sword_Normal_01_Metallic_Wav";
    
    public int bladeWooshAudioNum = 5;

    [Space(10)] 
    public float bloodParticleStayTime = 5.0f;
}
