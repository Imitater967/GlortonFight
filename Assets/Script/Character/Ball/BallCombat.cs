using Game;
using Script.Projectile;
using UnityEngine;

namespace Script.Character.FB
{
    public class BallCombat: GlortonFighterCombat
    {
        public override void RangedAttack()
        {
            base.RangedAttack();
            var ballFighter = (BallFighter)fighter;
            EventManager.Instance.Combat.Ball.OnBallRangedAttack?.Invoke(ballFighter);
            if (ballFighter.rangedProjectilePrefab != null)
            {
               var projectile= Instantiate(ballFighter.rangedProjectilePrefab, ballFighter.rangedMuzzleTransform.position,ballFighter.rangedMuzzleTransform.rotation);
               projectile.GetComponent<BallProjectile>().Init(fighter, fighter.rangedMuzzleTransform);
            }
            
        }

        public override void SpecialAttack()
        { 
            base.SpecialAttack();
            var ballFighter = fighter as BallFighter;
            var param=ballFighter.specialAttackArea.GetBoxCheckParam();
            var saTransform = ballFighter.transform; 
            //拳击是单体攻击
            Collider2D[] hit=Physics2D.OverlapBoxAll(param.center,param.size,saTransform.rotation.z,checkLayer);
            
            EventManager.Instance.Combat.Ball.OnBallSpecialAttack?.Invoke((BallFighter)fighter); 
            foreach (var targetCollider in hit)
            { 
                if (targetCollider.TryGetComponent(out GlortonFighter target))
                {  
                    EventManager.Instance.Combat.Ball.OnBallSpecialAttackSomeone?.Invoke((BallFighter)fighter,target); 
                }
            }
        }  
    }
}