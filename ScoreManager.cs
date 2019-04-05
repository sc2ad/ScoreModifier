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

        private TextMeshPro text;

        private static List<GameObject> gos;
        
        void Awake()
        {
            Plugin.CustomScore = 0;
            Plugin.CurrentCombo = 0;
            Plugin.BestCombo = 0;
            Plugin.Misses = 0;
            if (gos == null)
            {
                gos = new List<GameObject>();
            }
            foreach (GameObject o in gos)
            {
                Destroy(o);
            }
            gos.Add(gameObject);
            StartCoroutine(GrabController());
        }
        IEnumerator GrabController()
        {
            yield return new WaitUntil(() => Resources.FindObjectsOfTypeAll<ScoreController>().Any());
            controller = Resources.FindObjectsOfTypeAll<ScoreController>().Last();
            Init();
            yield break;
        }
        void Init()
        {
            controller.noteWasCutEvent += Controller_noteWasCutEvent;
            controller.noteWasMissedEvent += Controller_noteWasMissedEvent;
            controller.comboDidChangeEvent += Controller_comboDidChangeEvent;
            Logger.log.Info("Setup Controller Hooks!");

            Vector3 scorePosition = Plugin.config.Value.scorePositionInGame;
            scorePosition = new Vector3(scorePosition.x, scorePosition.y, scorePosition.z);

            GameObject viewer = new GameObject("ScoreManager | CustomScore");
            viewer.SetActive(false);
            text = viewer.AddComponent<TextMeshPro>();
            if (text == null)
            {
                text = viewer.GetComponent<TextMeshPro>();
            }
            text.text = Plugin.CustomScore.ToString();
            text.fontSize = 3;
            text.alignment = TextAlignmentOptions.Center;
            text.rectTransform.position = scorePosition;
            viewer.SetActive(true);

            GameObject labelGO = new GameObject("ScoreManager | CustomScoreLabel");
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
            scorePosition.y += Plugin.config.Value.yScoreLabelOffsetInGame;
            labelTM.rectTransform.localPosition = scorePosition;
            labelGO.SetActive(true);

            gos.Add(viewer);
            gos.Add(labelGO);
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
                switch (Plugin.config.Value.scoreType)
                {
                    case ScoreType.Osuv1:
                        int s1 = score + (int)(score * Math.Max(Plugin.CurrentCombo - 1, 0) / Plugin.config.Value.customScoreComboScale);
                        Logger.log.Info("Using Osuv1 ScoreType: " + s1);
                        Plugin.CustomScore += s1;
                        break;
                    case ScoreType.Function:
                        int s2 = PluginConfig.customScoreFunc(controller, score, noteData, info); ;
                        Logger.log.Info("Using CustomFunction ScoreType: " + s2);
                        Plugin.CustomScore += s2;
                        break;
                    case ScoreType.Test1:
                        int s3 = PluginConfig.getDeltaScore(score);
                        Logger.log.Info("Using Test1 ScoreType: " + s3);
                        Plugin.CustomScore += s3;
                        break;
                    default:
                        Logger.log.Info("Using Default ScoreType!");
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
