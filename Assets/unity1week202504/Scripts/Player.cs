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

                var stateName = result switch
                {
                    0 => "Up",
                    1 => "Down",
                    2 => "Left",
                    3 => "Right",
                    _ => throw new System.NotImplementedException(),
                };

                actor.SetSprite(stateName);
                actor.PlayAnimation(stateName, 5.0f);
            }
        }
    }
}
