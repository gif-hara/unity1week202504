using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using TMPro;

namespace unity1week202504
{
    public class UIViewGame
    {
        private readonly HKUIDocument document;

        public UIViewGame(HKUIDocument document)
        {
            this.document = document;
        }

        public void OpenLeftSpeechBalloon(string message)
        {
            var areaDocument = document.Q<HKUIDocument>("Area.LeftSpeechBalloon");
            var text = areaDocument.Q<TMP_Text>("Text");
            text.text = message;
            areaDocument.gameObject.SetActive(true);
        }

        public void CloseLeftSpeechBalloon()
        {
            var areaDocument = document.Q<HKUIDocument>("Area.LeftSpeechBalloon");
            areaDocument.gameObject.SetActive(false);
        }

        public void OpenRightSpeechBalloon(string message)
        {
            var areaDocument = document.Q<HKUIDocument>("Area.RightSpeechBalloon");
            var text = areaDocument.Q<TMP_Text>("Text");
            text.text = message;
            areaDocument.gameObject.SetActive(true);
        }

        public void CloseRightSpeechBalloon()
        {
            var areaDocument = document.Q<HKUIDocument>("Area.RightSpeechBalloon");
            areaDocument.gameObject.SetActive(false);
        }

        public void OpenInputGuide()
        {
            var areaDocument = document.Q<SimpleAnimation>("Area.InputGuide");
            areaDocument.gameObject.SetActive(true);
            areaDocument.Play("In");
        }

        public void CloseInputGuide()
        {
            var areaDocument = document.Q<HKUIDocument>("Area.InputGuide");
            areaDocument.gameObject.SetActive(false);
        }

        public void OpenLoseScreen()
        {
            var areaDocument = document.Q<SimpleAnimation>("Area.LoseScreen");
            areaDocument.gameObject.SetActive(true);
            areaDocument.Play("In");
        }

        public void CloseLoseScreen()
        {
            var areaDocument = document.Q("Area.LoseScreen");
            areaDocument.SetActive(false);
        }

        public void OpenWinScreen()
        {
            var areaDocument = document.Q<SimpleAnimation>("Area.WinScreen");
            areaDocument.gameObject.SetActive(true);
            areaDocument.Play("In");
        }

        public void CloseWinScreen()
        {
            var areaDocument = document.Q("Area.WinScreen");
            areaDocument.SetActive(false);
        }

        public UniTask PlayFadeAnimation(string name, CancellationToken cancellationToken)
        {
            var areaDocument = document.Q<SimpleAnimation>("Area.Fade");
            areaDocument.gameObject.SetActive(true);
            return areaDocument.PlayAsync(name, cancellationToken);
        }
    }
}
