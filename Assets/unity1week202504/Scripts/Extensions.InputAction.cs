using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;

namespace HK
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class Extensions
    {
        public static UniTask OnPerformedAsync(this InputAction action, CancellationToken cancellationToken = default)
        {
            var tcs = new UniTaskCompletionSource();

            void OnHandler(InputAction.CallbackContext ctx)
            {
                action.performed -= OnHandler;
                tcs.TrySetResult();
            }
            action.performed += OnHandler;
            cancellationToken.RegisterWithoutCaptureExecutionContext(() =>
            {
                action.performed -= OnHandler;
                tcs.TrySetCanceled(cancellationToken);
            });
            return tcs.Task;
        }
    }
}