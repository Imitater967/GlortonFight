using System;
using Game;
using Script.Projectile.Peach;

namespace Script.Character.Peach
{
    public class PeachCombat: GlortonFighterCombat
    {
        public override void RangedAttack()
        {
            base.RangedAttack();
            var ballFighter = (PeachFighter)fighter;
            EventManager.Instance.Combat.Peach.OnPeachFireBullet?.Invoke(ballFighter);
            if (ballFighter.rangedProjectilePrefab != null)
            {
                var projectile= Instantiate(ballFighter.rangedProjectilePrefab, ballFighter.rangedMuzzleTransform.position,ballFighter.rangedMuzzleTransform.rotation);
               projectile.GetComponent<PeachBullet>().Init(fighter, fighter.rangedMuzzleTransform);
            }

        }

        public override void SpecialAttack()
        {
            base.SpecialAttack();
            var ballFighter = (PeachFighter)fighter;
            EventManager.Instance.Combat.Peach.OnPeachFireRocket?.Invoke(ballFighter);
            if (ballFighter.specialAttackProjectilePrefab != null)
            {
                var projectile= Instantiate(ballFighter.specialAttackProjectilePrefab, ballFighter.specialAttackMuzzle.position,ballFighter.specialAttackMuzzle.rotation);
                projectile.GetComponent<PeachRocket>().Init(fighter, fighter.specialAttackMuzzle);
            }
        }
    }
}