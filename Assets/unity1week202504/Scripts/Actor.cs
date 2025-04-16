using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace unity1week202504
{
    public class Actor : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> sprites = new();

        [SerializeField]
        private GameObject defaultSprite;

        [SerializeField]
        private SimpleAnimation animationController;

        [SerializeField]
        private Animator animator;

        private bool beating = false;

        public Define.DanceType CurrentDanceType { get; private set; } = Define.DanceType.Default;

        public float BeatTime { get; private set; } = 0.0f;

        private bool isMiss = false;

        void Start()
        {
            foreach (var sprite in sprites)
            {
                sprite.SetActive(false);
            }
            if (defaultSprite != null)
            {
                defaultSprite.SetActive(true);
            }
        }

        public void TryDefaultBeat(float beatTime)
        {
            if (beating)
            {
                return;
            }
            var name = Define.DanceType.Default.ToString();
            SetSprite(name);
            PlayAnimationAsync(name).Forget();
            CurrentDanceType = Define.DanceType.Default;
            BeatTime = beatTime;
        }

        public void ExecuteBeat(Define.DanceType danceType, float beatTime)
        {
            if (isMiss)
            {
                return;
            }
            var name = danceType.ToString();
            SetSprite(name);
            PlayAnimationAsync(name).Forget();
            CurrentDanceType = danceType;
            BeatTime = beatTime;
        }

        public void ResetMiss()
        {
            isMiss = false;
        }

        public void Miss()
        {
            SetSprite(Define.DanceType.Fail.ToString());
            isMiss = true;
        }

        public void SetSprite(string name)
        {
            foreach (var sprite in sprites)
            {
                sprite.SetActive(sprite.name == name);
            }
        }

        public async UniTask PlayAnimationAsync(string name)
        {
            beating = true;
            animationController.Rewind(name);
            await animationController.PlayAsync(name, destroyCancellationToken);
            beating = false;
        }
    }
}
