using System;
using Script.Character;
using Script.Game;
using Script.Game.Room;
using Script.Projectile.Peach;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    
    public class FXManager: MonoBehaviour
    {
        [NonSerialized]
        public FXSetting setting;
        private EventManager _eventManager=>ApplicationManager.Instance.EventManager;
        public bool SendEvent;

        public void RegCombatEvent()
        { 
            _eventManager.Combat.OnPlayerKick += OnAttackFx;
            _eventManager.Combat.OnPlayerPunch += OnAttackFx;
            _eventManager.Combat.OnPlayerUpPunch += OnAttackFx; 
            _eventManager.Combat.Peach.OnPeachRocketExplode += ExplodeEffect;
            _eventManager.Mechanism.OnPlayerTriggerExplosiveZone += (a, b) =>
            {
                ExplodeEffect(b.transform.position);
            };
            _eventManager.Game.OnPlayerDeathEffectClient += SpawnDeathEffect;
            // _eventManager.Game.OnPlayerDeathServer += (fighter) =>
            // {
            //     ApplicationManager.Instance.RoomManager.GetPlayerStateById(fighter.OwnerClientId).Value
            //         .Player.TryGet(out RoomPlayer roomPlayer);
            //     Vector3 pos = fighter.transform.position;
            //     PolygonCollider2D mapBound=ApplicationManager.Instance.GameManager.MapBounds;
            //     Vector2 normal=mapBound.bounds.ClosestPoint(pos) - pos; 
            //     Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
            //     // SpawnDeathEffect(pos, rotation);
            //     roomPlayer.SendDeathEffectClientRpc(pos,rotation);
            // };
        }

        private void Update()
        { 
 
        }

        public void ExplodeEffect(Vector3 pos)
        {
            Instantiate(setting.explodeEffect, pos, Quaternion.identity);
        }
        public void ExplodeEffect(Transform pos)
        {
                ExplodeEffect(pos.position);
        }
        public void ExplodeEffect(PeachRocket rocket)
        {
            Instantiate(rocket.transform);
        }

        public void OnAttackFx(GlortonFighter a,GlortonFighter v)
        { 
            //0.5概率出特效
            if(Random.Range(0, 1.0f)<0.5||setting.punchEffect==null)
                return;
            //后面根据kick和punch有不同的位置
            Vector3 pos=(a.transform.position + v.transform.position)/2;
            Instantiate(setting.punchEffect, pos, quaternion.identity);
        } 
        private void SpawnDeathEffect(Vector3 pos,Quaternion rotation)
        {
            Debug.Log("Spawn Death Effect");
            Instantiate(setting.deathEffect,pos, rotation);
        }
        // //根据玩家死亡的位置，播放不同方向的闪电
        // public void OnPlayerDeathFX(GlortonFighter glortonFighter,Vector3 pos)
        // { 
        //     // // 获取离开的边的法线向量
        //     // Vector2 normal = other.transform.position - transform.position;
        //     // normal = Quaternion.Euler(0f, 0f, -90f) * normal.normalized;
        //     // // 获取四边形内部的法线向量
        //     // Vector2 inwardNormal = transform.up;
        //     // // 计算死亡特效的旋转角度
        //     // float angle = Vector2.SignedAngle(normal, inwardNormal);
        //     // // 生成死亡特效
        //     // Instantiate(deathEffectPrefab, other.transform.position, Quaternion.Euler(0f, 0f, angle)));
        //
        //     // Vector2 direction = (transform.position - lastPosition).normalized;
        //     // Vector2 normal = boxCollider.bounds.ClosestPoint(transform.position) - transform.position;
        //     // Quaternion rotation = Quaternion.FromToRotation(Vector2.up, normal);
        //     // Instantiate(spawnObject, lastPosition, rotation);
        //     // lastPosition = transform.position;
        //     PolygonCollider2D mapBound=ApplicationManager.Instance.GameManager.MapBounds;
        //     Vector2 normal=mapBound.bounds.ClosestPoint(pos) - pos; 
        //     Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        //     Instantiate(setting.deathEffect,pos, rotation);
        //     // Instantiate(setting.deathEffect, glortonFighter.transform.position, Quaternion.identity);
        // }

        public void Init(FXSetting fxSetting)
        {
            this.setting = fxSetting;
        }
    }
}