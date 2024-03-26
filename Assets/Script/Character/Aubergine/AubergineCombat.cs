using System.Collections.Generic;
using Game;
using Script.Projectile;
using Script.Projectile.Aubergine;
using UnityEngine;

namespace Script.Character.Aubergine
{
    public class AubergineCombat: GlortonFighterCombat
    {
        public override void SpecialAttack()
        {
            base.SpecialAttack();
            var fighter = this.fighter as AubergineFighter;
            EventManager.Instance.Combat.Aubergine.OnAubergineSpecialAttack?.Invoke(fighter);
            var param1=fighter.specialAttackArea1.GetBoxCheckParam();
            Collider2D[] hit1=Physics2D.OverlapBoxAll(param1.center,param1.size,0,checkLayer);
            var param2=fighter.specialAttackArea2.GetBoxCheckParam();
            Collider2D[] hit2=Physics2D.OverlapBoxAll(param2.center,param2.size,0,checkLayer);
            HashSet<GlortonFighter> fighters = new HashSet<GlortonFighter>();
            foreach (var collider2D1 in hit1)
            {
                if (collider2D1.TryGetComponent(out GlortonFighter victim))
                {
                    fighters.Add(victim);
                }
            }
            foreach (var collider2D1 in hit2)
            {
                if (collider2D1.TryGetComponent(out GlortonFighter victim))
                {
                    fighters.Add(victim);
                }
            }
            foreach (var glortonFighter in fighters)
            {
                EventManager.Instance.Combat.Aubergine.OnAubergineSpecialAttackSomeone?.Invoke(fighter,glortonFighter);
            }
        }

        public override void RangedAttack()
        {
            base.RangedAttack();
            var ballFighter = (AubergineFighter)fighter;
            EventManager.Instance.Combat.Aubergine.OnAubergineRangedAttack?.Invoke(ballFighter);
            if (ballFighter.rangedProjectilePrefab != null)
            {
                var projectile= Instantiate(ballFighter.rangedProjectilePrefab, ballFighter.rangedMuzzleTransform.position,ballFighter.rangedMuzzleTransform.rotation);
                projectile.GetComponent<PencilProjectile>().Init(fighter, fighter.rangedMuzzleTransform);
            }
            
        }
    }
}