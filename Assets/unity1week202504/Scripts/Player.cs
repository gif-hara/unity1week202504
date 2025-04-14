using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System.Threading;
using HK;

namespace unity1week202504
{
    public class Player
    {
        private readonly Actor actor;

        private readonly InputActionReference upAction;

        private readonly InputActionReference downAction;

        private readonly InputActionReference leftAction;

        private readonly InputActionReference rightAction;

        public Player(
            Actor actor,
            InputActionReference upAction,
            InputActionReference downAction,
            InputActionReference leftAction,
            InputActionReference rightAction
            )
        {
            this.actor = actor;
            this.upAction = upAction;
            this.downAction = downAction;
            this.leftAction = leftAction;
            this.rightAction = rightAction;
        }

        public async UniTaskVoid Attach()
        {
            while (true)
            {
                var result = await UniTask.WhenAny(
                    upAction.action.OnPerformedAsync(),
                    downAction.action.OnPerformedAsync(),
                    leftAction.action.OnPerformedAsync(),
                    rightAction.action.OnPerformedAsync()
                );

                if (result == 0)
                {
                    actor.SetSprite("Up");
                    actor.PlayAnimation("Up", 5.0f);
                }
                else if (result == 1)
                {
                    actor.SetSprite("Down");
                    actor.PlayAnimation("Down", 5.0f);
                }
                else if (result == 2)
                {
                    actor.SetSprite("Left");
                    actor.PlayAnimation("Left", 5.0f);
                }
                else if (result == 3)
                {
                    actor.SetSprite("Right");
                    actor.PlayAnimation("Right", 5.0f);
                }
            }
        }
    }
}
