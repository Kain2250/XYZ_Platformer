using System;
using UnityEngine;

namespace OneEyedJoe.Components
{
    public class ParabolaMovement : MonoBehaviour
    {
        private void StartMovement()
        {
            
        }

        public static Vector2 ParabolaMove(Vector2 start, Vector2 end, float height, float t)
        {
            Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

            var mid = Vector2.Lerp(start, end, t);

            return new Vector2(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t));
        }
    }
}
