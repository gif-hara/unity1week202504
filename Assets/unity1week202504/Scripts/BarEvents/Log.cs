using System;
using UnityEngine;

namespace unity1week202504.BarEvents
{
    [Serializable]
    public class Log : IBarEvent
    {
        [SerializeField]
        private string message;
        public string Message => message;
    }
}
