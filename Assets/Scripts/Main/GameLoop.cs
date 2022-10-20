using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using DefaultNamespace;
using Enemy;
using Entity;
using Manager;
using StateMachine.Enum.LogicEnum;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameLoop : MonoBehaviour
{
    public MgrPlayer MgrPlayer;

    private MgrUI _mgrUI;

    public MgrEnemy MgrEnemy;

    public MgrAudio MgrAudio;

    public MgrParticle MgrParticle;
    
    public GameConfig Config => GameObject.Find("Config").GetComponent<GameConfig>();
    
    public GameObject Particle => GameObject.Find("Particle");

    public GameLoop()
    {
        
    }

    private void Awake()
    {
        MgrPlayer = new MgrPlayer();
        _mgrUI = new MgrUI();
        MgrEnemy = new MgrEnemy();
        MgrAudio = new MgrAudio();

        MgrParticle = new MgrParticle(Particle.GetComponent<ParticleResource>().ParticleSystems.Count);
    }

    private void Start()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            MgrEnemy.AddEnemy(new EnemyBase(enemy));
        }
        
        _mgrUI.AddUI(new UIPlayerHp(GameObject.Find("PlayerHp"),MgrPlayer.MyPlayerEntity));
        foreach (var enemy in MgrEnemy.AllEnemy.Values)
        {
            _mgrUI.AddUI(new UIEnemyHp(null, enemy.EnemyEntity));
        }

        Screen.SetResolution(2560, 1440, true);
    }

    private void Update()
    {
        // Debug.LogError(Time.deltaTime);

        MgrPlayer.Update();
        
        _mgrUI.Update(); 
        
        MgrEnemy.Update();
        
        // BehaviorManager.instance.Update();

        MgrInput.Update(MgrPlayer.MyPlayerEntity.ArmState != (int)ArmState.Unequip);
        
        MgrParticle.Update();

        if (Input.GetKey(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }

    public GameObject GetDeepChild(GameObject root, string name)
    {
        foreach (var child in root.transform.GetComponentsInChildren<Transform>())
        {
            if (child.name == name)
            {
                return child.gameObject;
            }
        }

        return null;
    }
}
