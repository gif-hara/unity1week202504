using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System.Threading;
using HK;
using ZeroMessenger;
using UnityEngine;
using R3;

namespace unity1week202504
{
    public class Enemy
    {
        private readonly Actor actor;

        private readonly IMessageSubscriber<Messages.ListenDance> listenDanceSubscriber;

        private readonly IMessageSubscriber<Messages.Beat> beatSubscriber;

        public Enemy(
            Actor actor,
            IMessageSubscriber<Messages.ListenDance> listenDanceSubscriber,
            IMessageSubscriber<Messages.Beat> beatSubscriber
            )
        {
            this.actor = actor;
            this.listenDanceSubscriber = listenDanceSubscriber;
            this.beatSubscriber = beatSubscriber;
        }

        public UniTask AttachAsync(CancellationToken cancellationToken)
        {
            beatSubscriber
                .Subscribe(actor, static (_, actor) =>
                {
                    var stateName = "Default";
                    actor.SetSprite(stateName);
                    actor.PlayAnimation(stateName, 5.0f);
                })
                .RegisterTo(cancellationToken);
            listenDanceSubscriber
                .Subscribe(actor, static (x, actor) =>
                {
                    var danceType = x.DanceType;
                    var stateName = danceType.ToString();
                    actor.SetSprite(stateName);
                    actor.PlayAnimation(stateName, 5.0f);
                })
                .RegisterTo(cancellationToken);
            return UniTask.CompletedTask;
        }
    }
}
