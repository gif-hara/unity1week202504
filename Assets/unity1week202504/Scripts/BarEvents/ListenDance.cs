using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace unity1week202504.BarEvents
{
    [Serializable]
    public class ListenDance : IBarEvent
    {
        [SerializeField]
        private DanceData danceData;


        public async UniTask InvokeAsync(int bpm, float beatSeconds, CancellationToken cancellationToken = default)
        {
            var currentDanceIndex = 0;
            var currentBeatTiming = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                var danceElement = danceData.Elements[currentDanceIndex];
                if (currentBeatTiming >= danceElement.Timing)
                {
                    Debug.Log($"Dance: {danceElement.DanceType}");
                    currentDanceIndex++;
                    if (currentDanceIndex >= danceData.Elements.Count)
                    {
                        break;
                    }
                }
                currentBeatTiming++;
                await UniTask.Delay(TimeSpan.FromSeconds(beatSeconds / 4), cancellationToken: cancellationToken);
            }
        }
    }
}
