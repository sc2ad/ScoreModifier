using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ScoreModifier
{
    class ScoreManager : MonoBehaviour
    {
        private ScoreController controller;

        private Vector3 scorePosition = new Vector3(-2f, 2.75f, 2.5f);
        private TextMeshPro text;
        
        void Awake()
        {
            Plugin.CustomScore = 0;
            Plugin.CurrentCombo = 0;
            Plugin.BestCombo = 0;
            Plugin.Misses = 0;
            StartCoroutine(GrabController());
        }
        IEnumerator GrabController()
        {
            yield return new WaitUntil(() => Resources.FindObjectsOfTypeAll<ScoreController>().Any());
            controller = Resources.FindObjectsOfTypeAll<ScoreController>().FirstOrDefault();
            Init();
        }
        void Init()
        {
            controller.noteWasCutEvent += Controller_noteWasCutEvent;
            controller.noteWasMissedEvent += Controller_noteWasMissedEvent;
            controller.comboDidChangeEvent += Controller_comboDidChangeEvent;

            GameObject viewer = new GameObject("ScoreManager | CustomScore");
            viewer.SetActive(false);
            text = viewer.AddComponent<TextMeshPro>();
            text.text = Plugin.CustomScore.ToString();
            text.fontSize = 3;
            text.alignment = TextAlignmentOptions.Center;
            text.rectTransform.position = scorePosition;
            viewer.SetActive(true);

            GameObject labelGO = new GameObject("ScoreManager | CustomScoreLabel");
            labelGO.SetActive(false);
            TextMeshPro labelTM = viewer.AddComponent<TextMeshPro>();
            labelTM.text = "Custom Score";
            labelTM.fontSize = 3;
            labelTM.alignment = TextAlignmentOptions.Center;
            labelTM.rectTransform.parent = text.rectTransform;
            labelTM.rectTransform.localPosition = scorePosition;
            labelGO.SetActive(true);
        }

        private void Controller_noteWasMissedEvent(NoteData arg1, int arg2)
        {
            Plugin.Misses++;
        }

        private void Controller_comboDidChangeEvent(int combo)
        {
            Plugin.CurrentCombo = combo;
            if (combo > Plugin.BestCombo)
            {
                Plugin.BestCombo = combo;
            }
        }

        private void Controller_noteWasCutEvent(NoteData noteData, NoteCutInfo info, int multiplier)
        {
            if (noteData.noteType == NoteType.Bomb)
            {
                Plugin.Misses++;
            }
            if (info.allIsOK)
            {
                int scoreBeforeCut, scoreAfterCut, scoreDistance;
                ScoreController.ScoreWithoutMultiplier(info, null, out scoreBeforeCut, out scoreAfterCut, out scoreDistance);
                int score = scoreAfterCut - scoreBeforeCut;
                switch (Config.scoreType)
                {
                    case ScoreType.Osuv1:
                        Plugin.CustomScore += score + score * Math.Max(Plugin.CurrentCombo - 1, 0);
                        break;
                    case ScoreType.Function:
                        Plugin.CustomScore += Config.customScoreFunc(controller, score, noteData, info);
                        break;
                    default:
                        Plugin.CustomScore += score * multiplier;
                        break;
                }
            } else
            {
                Plugin.Misses++;
            }
            text.text = Plugin.CustomScore.ToString();
        }
    }
}
