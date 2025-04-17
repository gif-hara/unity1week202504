using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace unity1week202504
{
    public class UIViewGame
    {
        private readonly HKUIDocument document;

        private readonly List<HKUIDocument> lifeElements = new();

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

        public void OpenConfirm()
        {
            var areaDocument = document.Q<SimpleAnimation>("Area.Confirm");
            areaDocument.gameObject.SetActive(true);
            areaDocument.Play("In");
        }

        public void CloseConfirm()
        {
            var areaDocument = document.Q("Area.Confirm");
            areaDocument.SetActive(false);
        }

        public UniTask OnClickRetryButtonAsync(CancellationToken cancellationToken)
        {
            var areaDocument = document.Q<HKUIDocument>("Area.Confirm");
            return areaDocument.Q<Button>("Button.Retry").OnClickAsync(cancellationToken);
        }

        public UniTask OnClickTitleButtonAsync(CancellationToken cancellationToken)
        {
            var areaDocument = document.Q<HKUIDocument>("Area.Confirm");
            return areaDocument.Q<Button>("Button.Title").OnClickAsync(cancellationToken);
        }

        public async UniTask InitializeAreaLifeAsync(int life, CancellationToken cancellationToken)
        {
            var areaDocument = document.Q<HKUIDocument>("Area.Life");
            var parent = areaDocument.Q<RectTransform>("Parent.Element");
            var elementPrefab = areaDocument.Q<HKUIDocument>("Prefab.Element");
            areaDocument.gameObject.SetActive(true);
            for (var i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child != null)
                {
                    UnityEngine.Object.Destroy(child.gameObject);
                }
            }
            for (var i = 0; i < life; i++)
            {
                var element = UnityEngine.Object.Instantiate(elementPrefab, parent);
                element.Q<CanvasGroup>("Root").alpha = 0.0f;
                element.gameObject.SetActive(true);
                lifeElements.Add(element);
            }
            foreach (var i in lifeElements)
            {
                i.Q<SimpleAnimation>("Root").Play("In");
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: cancellationToken);
            }
        }

        public void PlayLifeElementOutAnimation(int index)
        {
            lifeElements[index].Q<SimpleAnimation>("Root").Play("Out");
        }
    }
}
