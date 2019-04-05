using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using System.Collections;

namespace ScoreModifier
{
    class ResultsViewer : MonoBehaviour
    {
        private List<GameObject> objects = new List<GameObject>();
        private ResultsViewController controller;
        private const int tries = 200;
        private const float delayTime = 0.1f;

        void Awake()
        {
            StartCoroutine(GrabResults());
        }

        IEnumerator GrabResults()
        {
            for (int i = 0; i < tries; i++)
            {
                controller = Resources.FindObjectsOfTypeAll<ResultsViewController>().FirstOrDefault();
                if (controller != null && controller.isActivated)
                {
                    break;
                }
                if ((i + 1) % 50 == 0)
                {
                    Logger.log.Info("Could not find ResultsViewController in: " + i + " tries!");
                }
                yield return new WaitForSeconds(delayTime);
            }
            Init();
            yield break;
        }

        void Init()
        {
            if (controller.isActivated)
            {
                CreateViewer(Plugin.config.Value.scorePositionAfterGame, "Custom Score");
                controller.continueButtonPressedEvent += Continue;
                controller.restartButtonPressedEvent += Continue;
            }
        }

        void Continue(ResultsViewController _)
        {
            foreach (GameObject o in objects)
            {
                Destroy(o);
            }
        }
        void CreateViewer(Vector3 position, string label)
        {
            position = new Vector3(position.x, position.y, position.z);
            GameObject viewer = new GameObject("ResultsViewer | " + label);
            viewer.SetActive(false);
            TextMeshPro text = viewer.AddComponent<TextMeshPro>();
            if (text == null)
            {
                text = viewer.GetComponent<TextMeshPro>();
            }
            text.text = Plugin.CustomScore.ToString();
            text.fontSize = 3;
            text.alignment = TextAlignmentOptions.Center;
            text.rectTransform.position = position;
            viewer.SetActive(true);

            GameObject labelGO = new GameObject("LabelViewer | " + label);
            labelGO.SetActive(false);
            TextMeshPro labelTM = viewer.AddComponent<TextMeshPro>();
            if (labelTM == null)
            {
                labelTM = viewer.GetComponent<TextMeshPro>();
            }
            labelTM.text = "Custom Score";
            labelTM.fontSize = 3;
            labelTM.alignment = TextAlignmentOptions.Center;
            labelTM.rectTransform.parent = text.rectTransform;
            position.y += Plugin.config.Value.yScoreLabelOffsetAfterGame;
            labelTM.rectTransform.localPosition = position;
            labelGO.SetActive(true);

            objects.Add(viewer);
            objects.Add(labelGO);
        }
    }
}
