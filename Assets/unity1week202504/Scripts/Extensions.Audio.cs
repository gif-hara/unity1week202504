using UnityEngine;

namespace HK
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class Extensions
    {
        public static float AsAudioVolume(this float value)
        {
            return Mathf.Log10(value) * 20;
        }
    }
}