using UnityEngine;

namespace unity1week202504.BarEvents
{
    public class OpenRightSpeechBalloon : IBarEvent
    {
        [SerializeField]
        private string message = string.Empty;
        public string Message => message;
    }
}
