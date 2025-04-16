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

        private Define.DanceType currentDanceType = Define.DanceType.Default;

        private float beatTime;

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

        public void TryDefaultBeat()
        {
            if (beating)
            {
                return;
            }
            var name = Define.DanceType.Default.ToString();
            SetSprite(name);
            PlayAnimationAsync(name).Forget();
        }

        public void ExecuteBeat(Define.DanceType danceType, float beatTime)
        {
            var name = danceType.ToString();
            SetSprite(name);
            PlayAnimationAsync(name).Forget();
            currentDanceType = danceType;
            this.beatTime = beatTime;
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
