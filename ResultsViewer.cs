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
        private const int tries = 20;
        private const float delayTime = 0.1f;

        private const float scoreXOffset = -0.2f;
        private const float scoreYOffset = 2.75f;
        private const float scoreZOffset = 2.5f;

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
                yield return new WaitForSeconds(delayTime);
            }
            Init();
        }

        void Init()
        {
            if (controller.isActivated)
            {
                CreateViewer(new Vector3(scoreXOffset, scoreYOffset, scoreZOffset), "Custom Score");
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
            GameObject viewer = new GameObject("ResultsViewer | " + label);
            viewer.SetActive(false);
            TextMeshPro text = viewer.AddComponent<TextMeshPro>();
            text.text = Plugin.CustomScore.ToString();
            text.fontSize = 3;
            text.alignment = TextAlignmentOptions.Center;
            text.rectTransform.position = position;
            viewer.SetActive(true);

            GameObject labelGO = new GameObject("LabelViewer | " + label);
            labelGO.SetActive(false);
            TextMeshPro labelTM = viewer.AddComponent<TextMeshPro>();
            labelTM.text = Plugin.CustomScore.ToString();
            labelTM.fontSize = 3;
            labelTM.alignment = TextAlignmentOptions.Center;
            labelTM.rectTransform.parent = text.rectTransform;
            labelTM.rectTransform.localPosition = position;
            labelGO.SetActive(true);

            objects.Add(viewer);
            objects.Add(labelGO);
        }
    }
}
