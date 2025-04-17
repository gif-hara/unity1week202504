using Cysharp.Threading.Tasks;
using HK;
using UnityEngine;
using UnityEngine.Audio;

namespace unity1week202504
{
    public class TitleSceneController : MonoBehaviour
    {
        [SerializeField]
        private HKUIDocument titleDocument;

        [SerializeField]
        private AudioMixer audioMixer;

        async UniTaskVoid Start()
        {
        }
    }
}
