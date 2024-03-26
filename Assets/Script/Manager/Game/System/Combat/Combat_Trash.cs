using Script.Character;
using Script.Projectile.Trash;
using UnityEngine;

namespace Game
{
    public partial class CombatSystem
    {

        public void OnTrashHit(TrashProjectile projectile, GlortonFighter victim)
        {
            var attacker = projectile.launcher;
            var setting = attacker.combat.setting;
            DamagePlayer(victim, setting.rangedDamage);
            victim.UnShock();
            MakeTargetFly(attacker,victim);     
            var finalForce =GetFinalForce(projectile,victim,setting.rangedForce,setting.rangedForceOffset);
            victim.rb.AddForce(finalForce,ForceMode2D.Impulse);
        }
    }
}