using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZeroMessenger;

namespace unity1week202504.BarEvents
{
    [Serializable]
    public class EnemyDance : IBarEvent
    {
        [SerializeField]
        private Define.DanceType danceType;

        public UniTask InvokeAsync(int bpm, float beatSeconds, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
