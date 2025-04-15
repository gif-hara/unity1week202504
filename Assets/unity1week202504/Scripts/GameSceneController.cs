using System;
using Cysharp.Threading.Tasks;
using HK;
using UnityEngine;
using UnityEngine.InputSystem;
using ZeroMessenger;

namespace unity1week202504
{
    public class GameSceneController : MonoBehaviour
    {
        [SerializeField]
        private int musicalScoreIndex = 0;

        [SerializeField]
        private GameRules gameRules;

        [SerializeField]
        private Actor player;

        [SerializeField]
        private Actor enemy;

        [SerializeField]
        private InputActionReference upAction;

        [SerializeField]
        private InputActionReference downAction;

        [SerializeField]
        private InputActionReference leftAction;

        [SerializeField]
        private InputActionReference rightAction;

        [SerializeField]
        private AudioManager audioManagerPrefab;

        async UniTaskVoid Start()
        {
            var audioManager = Instantiate(audioManagerPrefab);
            var beatMessageBroker = new MessageBroker<Messages.Beat>();
            new Player(
                player,
                upAction,
                downAction,
                leftAction,
                rightAction,
                beatMessageBroker
                ).Attach(destroyCancellationToken).Forget();
            new Enemy(
                enemy,
                MessageBroker<Messages.ListenDance>.Default,
                beatMessageBroker
                ).AttachAsync(destroyCancellationToken).Forget();

            var musicalScore = gameRules.MusicalScores[musicalScoreIndex];
            var beatSeconds = 60.0f / musicalScore.Bpm;
            Debug.Log($"BPM: {musicalScore.Bpm}");
            Debug.Log($"Beat Seconds: {beatSeconds}");
            audioManager.PlayBgm(musicalScore.Bgm.name);
            var currentBeatCount = 0;
            var barId = 0;

            while (!destroyCancellationToken.IsCancellationRequested)
            {
                if (barId < musicalScore.Bars.Count)
                {
                    var bar = musicalScore.Bars[barId];
                    if (currentBeatCount >= bar.Timing)
                    {
                        foreach (var barEvent in bar.Events)
                        {
                            barEvent.Value.InvokeAsync(musicalScore.Bpm, beatSeconds, destroyCancellationToken).Forget();
                        }
                        barId++;
                    }
                }
                Debug.Log($"Beat: {currentBeatCount}");
                beatMessageBroker.Publish(new Messages.Beat(), destroyCancellationToken);
                currentBeatCount++;
                await UniTask.Delay(TimeSpan.FromSeconds(beatSeconds), cancellationToken: destroyCancellationToken);
            }
        }
    }
}
