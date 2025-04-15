using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using R3;
using ZeroMessenger;
using ZeroMessenger.R3;

namespace unity1week202504
{
    public static partial class Workarounds
    {
        public static Task<T> FirstAsync<T>(this IMessageSubscriber<T> self, CancellationToken cancellationToken)
        {
            return self.ToObservable()
                .FirstAsync(cancellationToken);
        }
    }
}
