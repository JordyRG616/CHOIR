using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public static class GlobalFunctions 
    {

        public static Vector2 CalculatePointerPosition()
        {
            var position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            position -= Vector3.one * 0.5f;

            position.x *= 640;
            position.y *= 360;

            return position;
        }

        public static bool IsStackable(this StatusType status)
        {
            if(status == StatusType.Burn || status == StatusType.Frailty)
            {
                return true;
            } else return false;
        }

        public static int MaxStack(this StatusType status)
        {
            if(status.IsStackable()) return 5;
            else return 1;
        }

        public static int StackCount(this List<StatusHolder> list, StatusType status)
        {
            int count = 0;

            foreach(var holder in list)
            {
                if(holder.status == status) count++;
            }

            return count;
        }
    }
}

