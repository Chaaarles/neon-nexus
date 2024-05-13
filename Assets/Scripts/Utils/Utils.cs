using UnityEngine;

namespace Utils
{
    public static class Utils
    {
        public static float TimeMultiplier = 1;
        
        public static float AngleBetween(Vector2 looker, Vector2 target)
        {
            return Vector2.SignedAngle(Vector2.up, target - looker);
        }
    }
}