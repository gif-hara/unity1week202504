using System;
using Cysharp.Threading.Tasks;
using HK;
using unity1week202504.BarEvents;
using UnityEngine;
using UnityEngine.InputSystem;

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

        [SerializeField]
        private ParticleSystem successParticle;

        [SerializeField]
        private HKUIDocument gameDocument;

        private bool isGamePlay = false;

        private AudioManager audioManager;

        private MusicalScore musicalScore;

        private float beatSeconds;

        private float barSeconds;

        private int currentBarCount;

        private int barId;

        private Define.DanceType requiredDanceType = Define.DanceType.Default;

        private float requiredTime;

        async UniTaskVoid Start()
        {
            audioManager = Instantiate(audioManagerPrefab);
            musicalScore = gameRules.MusicalScores[musicalScoreIndex];
            beatSeconds = 60.0f / musicalScore.Bpm;
            barSeconds = beatSeconds / 4;
            currentBarCount = -1;
            barId = 0;
            var uiViewGame = new UIViewGame(gameDocument);
            uiViewGame.CloseLeftSpeechBalloon();
            uiViewGame.CloseRightSpeechBalloon();
            uiViewGame.CloseInputGuide();
            await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
            uiViewGame.OpenLeftSpeechBalloon("今日こそ ハト子ちゃん と なかよく なるぞ!");
            player.SetSprite("GameStart");
            player.PlayAnimation("Default");
            await UniTask.Delay(TimeSpan.FromSeconds(3.0f));
            player.SetSprite("Default");
            uiViewGame.CloseLeftSpeechBalloon();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            uiViewGame.OpenRightSpeechBalloon("あたしの ダンスを マネ してね!");
            enemy.PlayAnimation("Up");
            enemy.SetSprite("Up");
            await UniTask.Delay(TimeSpan.FromSeconds(3.0f));
            uiViewGame.CloseRightSpeechBalloon();
            enemy.SetSprite("Default");
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            uiViewGame.OpenInputGuide();
            await UniTask.Delay(TimeSpan.FromSeconds(3.0f));
            isGamePlay = true;
            audioManager.PlayBgm(musicalScore.Bgm.name, bgmScheduleTime);
        }

        void Update()
        {
            if (!isGamePlay)
            {
                return;
            }
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
                            player.ResetMiss();
                            player.TryDefaultBeat(time);
                            enemy.TryDefaultBeat(time);
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
                    enemy.SetSprite("Success");
                    audioManager.PlaySfx("Sfx.Success");
                    successParticle.Emit(1);
                    requiredDanceType = Define.DanceType.Default;
                }
                else if (time >= max)
                {
                    player.Miss();
                    enemy.SetSprite("Fail");
                    audioManager.PlaySfx("Sfx.Fail");
                    requiredDanceType = Define.DanceType.Default;
                }
            }
        }
    }
}
