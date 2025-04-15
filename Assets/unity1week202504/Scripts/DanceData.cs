using System;
using System.Collections.Generic;
using UnityEngine;

namespace unity1week202504
{
    [CreateAssetMenu(fileName = "DanceData", menuName = "unity1week202504/DanceData")]
    public class DanceData : ScriptableObject
    {
        [SerializeField]
        private List<Element> elements = new();
        public IReadOnlyList<Element> Elements => elements;

        [Serializable]
        public class Element
        {
            [SerializeField]
            private int timing;
            public int Timing => timing;

            [SerializeField]
            private Define.DanceType danceType;
            public Define.DanceType DanceType => danceType;
        }
    }
}
