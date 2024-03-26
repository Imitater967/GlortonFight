using System.Collections;
using Script.Character;
using Script.Character.FB;
using Script.Projectile;
using UnityEngine;

namespace Game
{
    public partial class CombatSystem{

        private void BallSpecialAttackSomeone(BallFighter attacker, GlortonFighter victim)
        {
            if(attacker==null)
                return;
            var setting = attacker.combat.setting; 
            victim.Shock(1f);
            StartCoroutine(SpecialAttackDelayedFly(attacker, victim));
        }

        private IEnumerator SpecialAttackDelayedFly(GlortonFighter attacker,GlortonFighter victim)
        {
            yield return new WaitForSeconds(0.25f);
            var setting = attacker.combat.setting;
            DamagePlayer(victim, setting.saDamage);
            victim.UnShock();
            MakeTargetFly(attacker,victim);     
            var finalForce =GetFinalForce(attacker,victim,setting.saForce,setting.saForceOffset);
            victim.rb.AddForce(finalForce,ForceMode2D.Impulse);
            Debug.Log(finalForce);
        }
        
        private IEnumerator RangedAttackDelayedFly(Projectile projectile,GlortonFighter victim)
        {
            yield return new WaitForSeconds(0.15f);
            var attacker = projectile.launcher;
            var setting = attacker.combat.setting;
            DamagePlayer(victim, setting.rangedDamage);
            victim.UnShock();
            MakeTargetFly(attacker,victim);     
            var finalForce =GetFinalForce(projectile,victim,setting.rangedForce,setting.rangedForceOffset);
            victim.rb.AddForce(finalForce,ForceMode2D.Impulse); 
        }
        //这里可能有bug，比如说在电到的时候，同时被另一个东西攻击不知道会怎么样
        private void BallRangedDamage(Projectile attacker,GlortonFighter victim)
        {
            if(attacker==null)
                return; 
            
            victim.Shock(1f);
            StartCoroutine(RangedAttackDelayedFly(attacker, victim));
            // DamagePlayer(victim, setting.rangedDamage);
            // ShockPlayer(attacker,victim);
            // Debug.Log(attacker.name +" "+(attacker.combat.damage*100).ToString("f0")+"%"+" shocked "+
            //           target.name+" "+(target.combat.damage*100).ToString("f0")+"%"); 
        }
        
    }
}