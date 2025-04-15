using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace unity1week202504.BarEvents
{
    [Serializable]
    public class Log : IBarEvent
    {
        [SerializeField]
        private string message;

        public UniTask InvokeAsync(int bpm, float beatSeconds, CancellationToken cancellationToken = default)
        {
            Debug.Log(message);
            return UniTask.CompletedTask;
        }
    }
}
