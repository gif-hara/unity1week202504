using UnityEngine;
using UnityEngine.InputSystem;

namespace unity1week202504
{
    public class GameSceneController : MonoBehaviour
    {
        [SerializeField]
        private Actor player;

        [SerializeField]
        private InputActionReference upAction;

        [SerializeField]
        private InputActionReference downAction;

        [SerializeField]
        private InputActionReference leftAction;

        [SerializeField]
        private InputActionReference rightAction;

        void Start()
        {
            new Player(
                player,
                upAction,
                downAction,
                leftAction,
                rightAction
                ).Attach().Forget();
        }
    }
}
