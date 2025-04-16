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
    }
}
