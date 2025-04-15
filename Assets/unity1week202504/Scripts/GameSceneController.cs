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

        private MusicalScore musicalScore;

        private float bgmLength;

        private float beatSeconds;

        private float barSeconds;

        private int currentBarCount;

        private int barId;

        private float time;

        void Start()
        {
            var audioManager = Instantiate(audioManagerPrefab);
            musicalScore = gameRules.MusicalScores[musicalScoreIndex];
            audioManager.PlayBgm(musicalScore.Bgm.name);
            bgmLength = musicalScore.Bgm.length;
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
            time += Time.deltaTime;
            var barCount = (int)(time / barSeconds);
            if (currentBarCount != barCount)
            {
                currentBarCount = barCount;
                var bar = musicalScore.Bars[barId];
                if (bar.Timing == currentBarCount)
                {
                    foreach (var barEvent in bar.Events)
                    {
                        if (barEvent.Value is Beat)
                        {
                            player.Beat("Default", 5.0f);
                            enemy.Beat("Default", 5.0f);
                        }
                        Debug.Log($"BarEvent: {barEvent.Value.GetType().Name} Timing: {bar.Timing}");
                    }
                    barId++;
                }
            }
        }
    }
}
