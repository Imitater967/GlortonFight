using Script.Character;
using Script.Character.Aubergine;
using Script.Projectile.Aubergine;
using UnityEngine; 

namespace Game
{
    public partial class CombatSystem
    {
        
        protected void OnAubergineRangedAttackSomeone(PencilProjectile pencilProjectile, GlortonFighter victim)
        {
            var attacker = pencilProjectile.launcher;
            var setting = attacker.combat.setting;
            DamagePlayer(victim, setting.rangedDamage);
            victim.UnShock();
            MakeTargetFly(attacker,victim);     
            var finalForce =GetFinalForce(pencilProjectile,victim,setting.rangedForce,setting.rangedForceOffset);
            victim.rb.AddForce(finalForce,ForceMode2D.Impulse);
            Debug.Log(finalForce);
        }

        protected void OnAubergineSpecialAttackSomeone(AubergineFighter attacker, GlortonFighter victim)
        {
            var setting = attacker.combat.setting;
            DamagePlayer(victim, setting.saDamage);
            victim.UnShock();
            MakeTargetFly(attacker,victim);     
            var finalForce =GetFinalForce(attacker,victim,setting.saForce,setting.saForceOffset);
            victim.rb.AddForce(finalForce,ForceMode2D.Impulse);
            Debug.Log(finalForce);   
        }
    }
}