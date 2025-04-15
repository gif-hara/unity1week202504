using System;
using System.Collections.Generic;
using TNRD;
using unity1week202504.BarEvents;
using UnityEngine;

namespace unity1week202504
{
    [CreateAssetMenu(fileName = "MusicalScore", menuName = "unity1week202504/MusicalScore")]
    public class MusicalScore : ScriptableObject
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

            public Bar(int timing, List<SerializableInterface<IBarEvent>> events)
            {
                this.timing = timing;
                this.events = events;
            }
        }

        [ContextMenu("Add Beat Event")]
        private void AddBeat()
        {
            var beatCount = bpm * 4; // 1小節4拍
            for (var i = 0; i < beatCount; i += 4)
            {
                var events = new List<SerializableInterface<IBarEvent>>
                {
                    new(new Beat())
                };
                var bar = new Bar(i, events);
                bars.Add(bar);
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
}
