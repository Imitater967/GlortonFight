using UnityEngine;

namespace Script.Character.FB
{
    public class BallFighter: GlortonFighter
    {
        [Header("球特殊设置")] public BoxCollider2D specialAttackArea;
       
        protected override void Awake()
        {
            base.Awake();
            DestroyImmediate(sprite);
            sprite=components.AddComponent<BallSprite>();
            sprite.RegInit(this);            
            DestroyImmediate(combat);
            combat = gameObject.AddComponent<BallCombat>();
            combat.RegInit(this);
            combat.setting = combatSetting; 
            combat.kick = kickAttackArea;
            combat.punch = punckAttackArea;
            combat.upPunch = upPunchAttackArea;
            combat.throwArea = throwAttackArea;
        }
    }
}