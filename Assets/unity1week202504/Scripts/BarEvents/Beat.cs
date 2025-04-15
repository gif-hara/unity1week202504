using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace unity1week202504.BarEvents
{
    public class Beat : IBarEvent
    {
        public UniTask InvokeAsync(int bpm, float beatSeconds, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
