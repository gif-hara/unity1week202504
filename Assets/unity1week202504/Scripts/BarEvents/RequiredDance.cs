using System;
using UnityEngine;

namespace unity1week202504.BarEvents
{
    [Serializable]
    public class RequiredDance : IBarEvent
    {
        [SerializeField]
        private Define.DanceType danceType;
        public Define.DanceType DanceType => danceType;
    }
}
