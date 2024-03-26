using UnityEngine;

namespace Script.Character.Aubergine
{
    public class AubergineSprite : GlortonFighterSprite
    {
        protected override void SpecialAttack()
        {
            base.SpecialAttack();
            FistOn();
            fighter.r_footRenderer.sprite = fighter.skinSheet.defaultSheet.l_foot;
        }
    }
}