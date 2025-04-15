using System.Collections.Generic;
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

        public void SetSprite(string name)
        {
            foreach (var sprite in sprites)
            {
                sprite.SetActive(sprite.name == name);
            }
        }

        public void PlayAnimation(string name, float speed)
        {
            animationController.Play(name);
            animationController.Rewind(name);
            animationController.GetState(name).speed = speed;
        }
    }
}
