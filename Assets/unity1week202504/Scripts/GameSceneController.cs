using HK;
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

        void Start()
        {
            var audioManager = Instantiate(audioManagerPrefab);
            new Player(
                player,
                upAction,
                downAction,
                leftAction,
                rightAction
                ).Attach().Forget();
            new Player(
                enemy,
                upAction,
                downAction,
                leftAction,
                rightAction
                ).Attach().Forget();

            var musicalScore = gameRules.MusicalScores[musicalScoreIndex];
            var beatSeconds = 60.0f / musicalScore.Bpm;
            Debug.Log($"BPM: {musicalScore.Bpm}");
            Debug.Log($"Beat Seconds: {beatSeconds}");
            audioManager.PlayBgm(musicalScore.Bgm.name);
        }
    }
}
