using Game;
using Script.Projectile.Peach;
using Script.Projectile.Trash;
using UnityEngine;

namespace Script.Character.Trash
{
    public class TrashCombat: GlortonFighterCombat
    {
        public override void RangedAttack()
        {
            base.RangedAttack();
            var ballFighter = (TrashFighter)fighter;
            EventManager.Instance.Combat.Trash.OnTrashRangedAttack?.Invoke(ballFighter);
            if (ballFighter.rangedProjectilePrefab != null)
            {
                var projectile= Instantiate(ballFighter.rangedProjectilePrefab, ballFighter.rangedMuzzleTransform.position,ballFighter.rangedMuzzleTransform.rotation);
 
                projectile.GetComponent<TrashProjectile>().Init(fighter, fighter.rangedMuzzleTransform);
            }
        }

        public override void SpecialAttack()
        {
            base.SpecialAttack();
            var ballFighter = (TrashFighter)fighter;
            EventManager.Instance.Combat.Trash.OnTrashSpecialAttack?.Invoke(ballFighter);
            float marign = 15;
            Vector3 arm = new Vector3(0, 0.1f, 0);
            Vector3 rotEuler=Vector3.zero;
            while (rotEuler.z < 360)
            {
                var rotation = Quaternion.Euler(rotEuler);
                Vector3 spawnPos = rotation * arm;
                rotEuler.z += marign;
                var projectile=Instantiate(ballFighter.rangedProjectilePrefab, transform.position+(spawnPos), rotation);
                projectile.GetComponent<TrashProjectile>().Init(fighter,projectile.transform);
            }
        }
    }
}