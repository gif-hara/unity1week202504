using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using R3;
using UnityEngine.UI;

namespace unity1week202504
{
    public class UIViewTitle
    {
        private readonly HKUIDocument titleDocument;
        public UIViewTitle(HKUIDocument titleDocument)
        {
            this.titleDocument = titleDocument;
        }

        public UniTask OnClickPleaseAnyClickAsync(CancellationToken cancellationToken)
        {
            var areaDocument = titleDocument.Q<HKUIDocument>("Area.PleaseAnyClick");
            areaDocument.gameObject.SetActive(true);
            areaDocument.Q<SimpleAnimation>("SimpleAnimation").Play("Loop");
            return areaDocument.Q<Button>("Button").OnClickAsync(cancellationToken);
        }

        public void ClosePleaseAnyClick(CancellationToken cancellationToken)
        {
            titleDocument
                .Q<HKUIDocument>("Area.PleaseAnyClick")
                .gameObject
                .SetActive(false);
        }

        public Observable<float> OnValueChangedBgmVolumeAsObservable()
        {
            return titleDocument
                .Q<HKUIDocument>("Area.Settings")
                .Q<Slider>("Slider.BgmVolume")
                .OnValueChangedAsObservable();
        }

        public Observable<float> OnValueChangedSfxVolumeAsObservable()
        {
            return titleDocument
                .Q<HKUIDocument>("Area.Settings")
                .Q<Slider>("Slider.SfxVolume")
                .OnValueChangedAsObservable();
        }

        public void OpenConfirm()
        {
            var areaDocument = titleDocument.Q<HKUIDocument>("Area.Confirm");
            areaDocument.gameObject.SetActive(true);
            areaDocument.Q<SimpleAnimation>("SimpleAnimation").Play("In");
        }

        public void CloseConfirm()
        {
            var areaDocument = titleDocument.Q<HKUIDocument>("Area.Confirm");
            areaDocument.gameObject.SetActive(false);
        }

        public UniTask OnClickGameStartButtonAsync(CancellationToken cancellationToken)
        {
            return titleDocument
                .Q<HKUIDocument>("Area.Confirm")
                .Q<Button>("Button.GameStart")
                .OnClickAsync(cancellationToken);
        }

        public UniTask PlayFadeAnimationAsync(string name, CancellationToken cancellationToken)
        {
            var areaDocument = titleDocument.Q<SimpleAnimation>("Area.Fade");
            areaDocument.gameObject.SetActive(true);
            return areaDocument.PlayAsync(name, cancellationToken);
        }
    }
}
