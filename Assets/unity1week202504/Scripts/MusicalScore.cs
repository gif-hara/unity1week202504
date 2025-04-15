using System;
using System.Collections.Generic;
using TNRD;
using unity1week202504.BarEvents;
using UnityEngine;

namespace unity1week202504
{
    [Serializable]
    public class MusicalScore
    {
        [SerializeField]
        private AudioClip bgm;
        public AudioClip Bgm => bgm;

        [SerializeField]
        private int bpm;
        public int Bpm => bpm;

        [SerializeField]
        private List<Bar> bars = new();
        public IReadOnlyList<Bar> Bars => bars;

        /// <summary>
        /// 小節
        /// </summary>
        [Serializable]
        public class Bar
        {
            [SerializeField]
            private int timing;
            public int Timing => timing;

            [SerializeField]
            private List<SerializableInterface<IBarEvent>> events = new();
            public IReadOnlyList<SerializableInterface<IBarEvent>> Events => events;
        }
    }
}
