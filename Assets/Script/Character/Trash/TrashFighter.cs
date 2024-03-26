namespace Script.Character.Trash
{
    public class TrashFighter: GlortonFighter
    {
        protected override void Awake()
        {
            base.Awake();
            DestroyImmediate(combat);
            combat = gameObject.AddComponent<TrashCombat>();
            combat.RegInit(this);
            combat.setting = combatSetting; 
            combat.kick = kickAttackArea;
            combat.punch = punckAttackArea;
            combat.upPunch = upPunchAttackArea;
            combat.throwArea = throwAttackArea;
        }
    }
}