using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System.Threading;
using HK;
using ZeroMessenger.Internal;
using ZeroMessenger;

namespace unity1week202504
{
    public class Player
    {
        private readonly Actor actor;

        private readonly InputActionReference upAction;

        private readonly InputActionReference downAction;

        private readonly InputActionReference leftAction;

        private readonly InputActionReference rightAction;

        private readonly IMessageSubscriber<Messages.Beat> beatSubscriber;

        public Player(
            Actor actor,
            InputActionReference upAction,
            InputActionReference downAction,
            InputActionReference leftAction,
            InputActionReference rightAction,
            IMessageSubscriber<Messages.Beat> beatSubscriber
            )
        {
            this.actor = actor;
            this.upAction = upAction;
            this.downAction = downAction;
            this.leftAction = leftAction;
            this.rightAction = rightAction;
            this.beatSubscriber = beatSubscriber;
        }

        public async UniTaskVoid Attach(CancellationToken cancellationToken)
        {
            BeatAsync(cancellationToken).Forget();
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

        private async UniTaskVoid BeatAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await beatSubscriber.FirstAsync(cancellationToken);
                var stateName = "Default";
                actor.SetSprite(stateName);
                actor.PlayAnimation(stateName, 5.0f);
            }
        }
    }
}
