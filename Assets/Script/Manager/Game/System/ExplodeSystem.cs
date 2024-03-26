using Game;
using Script.Character;
using Script.Character.Peach;
using Script.Projectile.Peach;
using UnityEngine;

namespace Script.Game
{
    public class ExplodeSystem: global::Game.System
    {
        public override void OnGameInit()
        {
            base.OnGameInit();
        }

        public override void OnGamePrepared()
        {
            base.OnGamePrepared();
            _eventManager.Combat.Peach.OnPeachRocketExplode += PeachExplode;
        }

        private void PeachExplode(PeachRocket rocket)
        { 
            Debug.Log("Peach Explode");
            var param=rocket.range.GetBoxCheckParam();
            Collider2D[] hit=Physics2D.OverlapBoxAll(param.center,param.size,0,Utils.LAYER_PLAYERS);
            
            foreach (var targetCollider in hit)
            {
                if (targetCollider.TryGetComponent(out GlortonFighter target))
                { 
                    Debug.Log("Explode included"+target.gameObject.name);
                    EventManager.Instance.Combat.Peach.OnPeachRocketDamageSomeone?.Invoke(rocket,target); 
                }
            }
        }
    }
}