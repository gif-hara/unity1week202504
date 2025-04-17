using System;
using Cysharp.Threading.Tasks;
using HK;
using unity1week202504.BarEvents;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

        [SerializeField]
        private bool isSkipPrologue = false;

        private Define.GameState gameState = Define.GameState.Initialize;

        private int lifeCount = 3;

        private AudioManager audioManager;

        private MusicalScore musicalScore;

        private float beatSeconds;

        private float barSeconds;

        private int currentBarCount;

        private int barId;

        private Define.DanceType requiredDanceType = Define.DanceType.Default;

        private float requiredTime;

        private UIViewGame uiViewGame;

        async UniTaskVoid Start()
        {
            audioManager = Instantiate(audioManagerPrefab);
            musicalScore = gameRules.MusicalScores[musicalScoreIndex];
            beatSeconds = 60.0f / musicalScore.Bpm;
            barSeconds = beatSeconds / 4;
            currentBarCount = -1;
            barId = 0;
            uiViewGame = new UIViewGame(gameDocument);
            gameState = Define.GameState.Initialize;
            uiViewGame.CloseLeftSpeechBalloon();
            uiViewGame.CloseRightSpeechBalloon();
            uiViewGame.CloseInputGuide();
            uiViewGame.CloseLoseScreen();
            uiViewGame.CloseWinScreen();
            uiViewGame.CloseConfirm();

            await uiViewGame.PlayFadeAnimation("In.1", destroyCancellationToken);

            // プロローグ
#if DEBUG && UNITY_EDITOR
            if (!isSkipPrologue)
#endif
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
                uiViewGame.OpenLeftSpeechBalloon("今日こそ ハト子ちゃん と なかよく なるぞ!");
                player.SetSprite("Up");
                player.PlayAnimation("Up");
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
            }
            // ゲーム開始
            {
                await uiViewGame.InitializeAreaLifeAsync(lifeCount, destroyCancellationToken);
                audioManager.PlayBgm(musicalScore.Bgm.name, bgmScheduleTime);
                gameState = Define.GameState.InGame;
            }
            // ゲーム待機
            {
                await UniTask.WaitWhile(this, @this => @this.gameState == Define.GameState.InGame);
            }
            // ゲーム終了
            {
                audioManager.StopBgm();
                if (gameState == Define.GameState.Win)
                {
                    audioManager.PlaySfx("Sfx.Win");
                    uiViewGame.OpenWinScreen();
                }
                else if (gameState == Define.GameState.Lose)
                {
                    audioManager.PlaySfx("Sfx.Lose");
                    uiViewGame.OpenLoseScreen();
                }
                await UniTask.Delay(TimeSpan.FromSeconds(2.0f));
                uiViewGame.OpenConfirm();
                var confirmButtonIndex = await UniTask.WhenAny(
                    uiViewGame.OnClickRetryButtonAsync(destroyCancellationToken),
                    uiViewGame.OnClickTitleButtonAsync(destroyCancellationToken)
                );
                if (confirmButtonIndex == 0)
                {
                    await uiViewGame.PlayFadeAnimation("Out.1", destroyCancellationToken);
                    SceneManager.LoadScene("Game");
                }
                else if (confirmButtonIndex == 1)
                {
                    await uiViewGame.PlayFadeAnimation("Out.1", destroyCancellationToken);
                    SceneManager.LoadScene("Title");
                }
            }
        }

        void Update()
        {
            if (gameState != Define.GameState.InGame)
            {
                return;
            }
            if (barId >= musicalScore.Bars.Count)
            {
                gameState = Define.GameState.Win;
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
                            audioManager.PlaySfx("Sfx.EnemyDance", enemyDance.SfxPitch);
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
                    uiViewGame.PlayLifeElementOutAnimation(lifeCount - 1);
                    player.Miss();
                    enemy.SetSprite("Fail");
                    audioManager.PlaySfx("Sfx.Fail");
                    lifeCount--;
                    if (lifeCount <= 0)
                    {
                        gameState = Define.GameState.Lose;
                    }
                    requiredDanceType = Define.DanceType.Default;
                }
            }
        }
    }
}
