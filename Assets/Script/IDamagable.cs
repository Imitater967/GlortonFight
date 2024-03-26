namespace Script
{
    //机关，或者人类
    public enum Damagable
    {
        Fighter,Trigger
    }
    public interface IDamagable
    {
        public void RevDamage(float damage);
        public Damagable GetType();
    }
}