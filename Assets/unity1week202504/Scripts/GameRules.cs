using System.Collections.Generic;
using UnityEngine;

namespace unity1week202504
{
    [CreateAssetMenu(fileName = "GameRules", menuName = "unity1week202504/GameRules")]
    public class GameRules : ScriptableObject
    {
        [SerializeField]
        private List<MusicalScore> musicalScores = new();
        public List<MusicalScore> MusicalScores => musicalScores;
    }
}
