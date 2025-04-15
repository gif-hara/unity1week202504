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

        public void TryBeat(string name)
        {
            if (beating)
            {
                return;
            }
            SetSprite(name);
            PlayAnimationAsync(name).Forget();
        }

        public void ForceBeat(string name)
        {
            SetSprite(name);
            PlayAnimationAsync(name).Forget();
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
