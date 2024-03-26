using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script
{
    public struct BoxCheckParam
    {
        public Vector2 center;
        public Vector2 size;
    }

    public static class Utils
    {
        public static LayerMask LAYER_PLAYERS = (1 << LayerMask.NameToLayer("P1")
                                                 | 1 << LayerMask.NameToLayer("P2")
                                                 | 1 << LayerMask.NameToLayer("P3")
                                                 | 1 << LayerMask.NameToLayer("P4"));

        public static int GetLayerByIndex(byte index)
        {
            return LayerMask.NameToLayer("P"+(index+1));
        }

        public static int GetSortingLayer(byte index)
        {
            return SortingLayer.NameToID("P" + (index+1));
        }
        
        public static BoxCheckParam GetBoxCheckParam(this BoxCollider2D collider)
        {
            return new BoxCheckParam()
            {
                center = collider.offset + new Vector2(collider.transform.position.x, collider.transform.position.y),
                size = collider.size * new Vector2(collider.transform.localScale.x, collider.transform.localScale.y)
            };
        }

        public static string ToHumanReadableName(InputAction action,int index)
        { 
         return InputControlPath.ToHumanReadableString(action.bindings[index].effectivePath,InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
    }
}