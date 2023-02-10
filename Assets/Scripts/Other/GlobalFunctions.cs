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

            position.x *= 1280;
            position.y *= 720;

            return position;
        }
    }
}

