using System;
using UnityEngine;

namespace Script.Character
{
    public class GlortonFighterComponent: MonoBehaviour,IGlortonComponent
    {
        public new string name=>fighter.name;
        protected GlortonFighter fighter;
        // public GlortonFighter fighter
        // {
        //     get
        //     {
        //         if (_fighter == null)
        //         {
        //             _fighter = GetComponent<GlortonFighter>();
        //         } 
        //         return _fighter;
        //     }
        // }

        // private GlortonFighter _fighter;

        public virtual void RegInit(GlortonFighter fighter1)
        {
            fighter=fighter1;
            fighter.OnInit += OnInit;
            
            // Debug.Log(name+": 已经为"+this.GetType().Name+"注册Init");
        }

        private void OnDestroy()
        {
            fighter.OnInit -= OnInit;
            // Debug.Log(name+": 已经为"+this.GetType().Name+"注销Init");
        }

        protected virtual void OnInit()
        {
            
        }
    }
}