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

        private AudioManager audioManager;

        private MusicalScore musicalScore;

        private float beatSeconds;

        private float barSeconds;

        private int currentBarCount;

        private int barId;

        private Define.DanceType requiredDanceType = Define.DanceType.Default;

        private float requiredTime;

        void Start()
        {
            audioManager = Instantiate(audioManagerPrefab);
            musicalScore = gameRules.MusicalScores[musicalScoreIndex];
            audioManager.PlayBgm(musicalScore.Bgm.name, bgmScheduleTime);
            beatSeconds = 60.0f / musicalScore.Bpm;
            barSeconds = beatSeconds / 4;
            currentBarCount = -1;
            barId = 0;
        }

        void Update()
        {
            if (barId >= musicalScore.Bars.Count)
            {
                Debug.Log("All bars completed.");
                return;
            }
            var time = audioManager.BgmSource.time;
            if (upAction.action.WasPerformedThisFrame())
            {
                player.ExecuteBeat(Define.DanceType.Up, time);
            }
            if (downAction.action.WasPerformedThisFrame())
            {
                player.ExecuteBeat(Define.DanceType.Down, time);
            }
            if (leftAction.action.WasPerformedThisFrame())
            {
                player.ExecuteBeat(Define.DanceType.Left, time);
            }
            if (rightAction.action.WasPerformedThisFrame())
            {
                player.ExecuteBeat(Define.DanceType.Right, time);
            }
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
                            enemy.ExecuteBeat(enemyDance.DanceType, time);
                        }
                        else if (barEvent.Value is RequiredDance requiredDance)
                        {
                            requiredDanceType = requiredDance.DanceType;
                            requiredTime = time;
                        }
                        else if (barEvent.Value is TryDefaultBeat)
                        {
                            player.TryDefaultBeat();
                            enemy.TryDefaultBeat();
                        }
                    }
                    barId++;
                }
            }

            if (requiredDanceType != Define.DanceType.Default)
            {
                var min = requiredTime - musicalScore.SuccessRange;
                var max = requiredTime + musicalScore.SuccessRange;
                if (player.BeatTime >= min && player.BeatTime <= max && player.CurrentDanceType == requiredDanceType)
                {
                    Debug.Log("Success!");
                    requiredDanceType = Define.DanceType.Default;
                }
                else if (time >= max)
                {
                    Debug.Log("Fail!");
                    requiredDanceType = Define.DanceType.Default;
                }
            }
        }
    }
}
