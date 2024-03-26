using System.Collections.Generic;
using Cinemachine;
using Game;
using Script.Character;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Game
{
    public class CameraSystem : global::Game.System
    {

        public CinemachineTargetGroup TargetGroup; 
        private CinemachineBrain _brain;
        public override void OnGameInit()
        { 
            base.OnGameInit();  
            EventManager.Instance.Game.OnPlayerSpawnClient+=AddToTargetGroup;
        }

        private void AddToTargetGroup(GlortonFighter obj)
        { 
            List<CinemachineTargetGroup.Target> targets = new List<CinemachineTargetGroup.Target>(TargetGroup.m_Targets);
            targets.Add(new CinemachineTargetGroup.Target(){radius = 1,target = obj.transform,weight = 0.5f});
            TargetGroup.m_Targets = targets.ToArray();
        }


        public override void OnGamePrepared()
        {
            base.OnGamePrepared();
            
        }

    }
}