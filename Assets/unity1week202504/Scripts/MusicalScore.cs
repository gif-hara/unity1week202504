using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 小節
        /// </summary>
        [Serializable]
        public class Bar
        {
            [SerializeField]
            private int timing;

            [SerializeField]
            private string test;
        }
    }
}
