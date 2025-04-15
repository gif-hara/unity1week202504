using System.Threading;
using Cysharp.Threading.Tasks;

namespace unity1week202504.BarEvents
{
    /// <summary>
    /// 小節イベントのインターフェース
    /// </summary>
    public interface IBarEvent
    {
        UniTask InvokeAsync(int bpm, float beatSeconds, CancellationToken cancellationToken = default);
    }
}
