using Cysharp.Threading.Tasks;
using HK;
using R3;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace unity1week202504
{
    public class TitleSceneController : MonoBehaviour
    {
        [SerializeField]
        private HKUIDocument titleDocument;

        [SerializeField]
        private AudioMixer audioMixer;

        [SerializeField]
        private AudioManager audioManagerPrefab;

        async UniTaskVoid Start()
        {
            var uiViewTitle = new UIViewTitle(titleDocument);
            var audioManager = Instantiate(audioManagerPrefab);
            var bgmVolume = PlayerPrefs.GetFloat("BgmVolume", 0.5f);
            var sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.5f);
            audioMixer.SetFloat("BgmVolume", bgmVolume.AsAudioVolume());
            audioMixer.SetFloat("SfxVolume", sfxVolume.AsAudioVolume());
            uiViewTitle.SetBgmVolumeSlider(bgmVolume);
            uiViewTitle.SetSfxVolumeSlider(sfxVolume);
            uiViewTitle.OnValueChangedBgmVolumeAsObservable()
                .Subscribe(audioMixer, static (x, audioMixer) =>
                {
                    audioMixer.SetFloat("BgmVolume", x.AsAudioVolume());
                    PlayerPrefs.SetFloat("BgmVolume", x);
                })
                .RegisterTo(destroyCancellationToken);
            uiViewTitle.OnValueChangedSfxVolumeAsObservable()
                .Subscribe(audioMixer, static (x, audioMixer) =>
                {
                    audioMixer.SetFloat("SfxVolume", x.AsAudioVolume());
                    PlayerPrefs.SetFloat("SfxVolume", x);
                })
                .RegisterTo(destroyCancellationToken);

            audioManager.PlayBgm("Bgm.Title", 0.0f);
            uiViewTitle.CloseConfirm();
            await uiViewTitle.PlayFadeAnimationAsync("In.1", destroyCancellationToken);
            await uiViewTitle.OnClickPleaseAnyClickAsync(destroyCancellationToken);
            audioManager.PlaySfx("Sfx.Decide");
            uiViewTitle.ClosePleaseAnyClick(destroyCancellationToken);
            uiViewTitle.OpenConfirm();
            await uiViewTitle.OnClickGameStartButtonAsync(destroyCancellationToken);
            audioManager.PlaySfx("Sfx.Decide");
            await uiViewTitle.PlayFadeAnimationAsync("Out.1", destroyCancellationToken);
            SceneManager.LoadScene("Game");
        }
    }
}
