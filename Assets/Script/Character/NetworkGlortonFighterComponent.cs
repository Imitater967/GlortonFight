using Unity.Netcode;
using UnityEngine;

namespace Script.Character
{
    public class NetworkGlortonFighterComponent: NetworkBehaviour,IGlortonComponent
    {
        public new string name=>fighter.name;
        protected GlortonFighter fighter; 

        public virtual void RegInit(GlortonFighter fighter1)
        {
            fighter=fighter1;
            fighter.OnInit += OnInit;
            
            // Debug.Log(name+": 已经为"+this.GetType().Name+"注册Init");
        }

        protected virtual void OnInit(){}

        private void OnDestroy()
        {
            fighter.OnInit -= OnInit;
            // Debug.Log(name+": 已经为"+this.GetType().Name+"注销Init");
        }
        
    }
}