using System;
using System.Collections.Generic;
using Script.Character;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Game.Room
{
    [Serializable]

    public partial class RoomManager
    {
        private void SelectFighter(ulong id,FighterType type)
        {
            if (!IsServer)
            {
                Debug.Log("Select fighter not on server");
            }

            var stateRef = GetPlayerStateById(id);
            var originState = stateRef.Value;
            var originFighter = GetFighterAsset(originState);
            originFighter.Selected = false;
            var fighterAsset = GetFirstAvailableFighter(type);
            fighterAsset.Selected = true;
            var selection = originState;
            selection.FighterClass = fighterAsset.Type;
            selection.FighterIndex = fighterAsset.Index;
            selection.Selected = true; 
            GetPlayerStateById(id).Value = selection;
        }
 

        public FighterAsset GetFighterAsset(RoomPlayerState state)
        {
            return GetFighters(state.FighterClass)[state.FighterIndex];
        }
        public FighterAsset GetFirstAvailableFighter(List<FighterAsset> fighters)
        {
            foreach (var fighterAsset in fighters)
            {
                if (!fighterAsset.Selected)
                    return fighterAsset;
            }

            return fighters[0];
        }
    }
}