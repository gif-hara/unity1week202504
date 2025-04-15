using System;
using Cysharp.Threading.Tasks;
using HK;
using unity1week202504.BarEvents;
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

        [SerializeField]
        private float bgmScheduleTime = 0.0f;

        private MusicalScore musicalScore;

        private double startTime;

        private float beatSeconds;

        private float barSeconds;

        private int currentBarCount;

        private int barId;

        void Start()
        {
            var audioManager = Instantiate(audioManagerPrefab);
            musicalScore = gameRules.MusicalScores[musicalScoreIndex];
            audioManager.PlayBgm(musicalScore.Bgm.name, bgmScheduleTime);
            beatSeconds = 60.0f / musicalScore.Bpm;
            barSeconds = beatSeconds / 4;
            currentBarCount = -1;
            barId = 0;
            startTime = AudioSettings.dspTime + bgmScheduleTime;
        }

        void Update()
        {
            if (barId >= musicalScore.Bars.Count)
            {
                Debug.Log("All bars completed.");
                return;
            }
            if (upAction.action.WasPerformedThisFrame())
            {
                player.ExecuteBeat(Define.DanceType.Up);
            }
            if (downAction.action.WasPerformedThisFrame())
            {
                player.ExecuteBeat(Define.DanceType.Down);
            }
            if (leftAction.action.WasPerformedThisFrame())
            {
                player.ExecuteBeat(Define.DanceType.Left);
            }
            if (rightAction.action.WasPerformedThisFrame())
            {
                player.ExecuteBeat(Define.DanceType.Right);
            }
            var time = AudioSettings.dspTime - startTime;
            if (time <= 0.0f)
            {
                return;
            }
            var barCount = (int)(time / barSeconds);
            if (currentBarCount != barCount)
            {
                currentBarCount = barCount;
                var bar = musicalScore.Bars[barId];
                if (bar.Timing == currentBarCount)
                {
                    foreach (var barEvent in bar.Events)
                    {
                        if (barEvent.Value is EnemyDance enemyDance)
                        {
                            enemy.ExecuteBeat(enemyDance.DanceType);
                        }
                        else if (barEvent.Value is TryDefaultBeat)
                        {
                            player.TryDefaultBeat();
                            enemy.TryDefaultBeat();
                        }
                        Debug.Log($"BarEvent: {barEvent.Value.GetType().Name} Timing: {bar.Timing}");
                    }
                    barId++;
                }
            }
        }
    }
}
