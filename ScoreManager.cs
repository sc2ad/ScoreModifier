using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ScoreModifier
{
    class ScoreManager : MonoBehaviour
    {
        private ScoreController controller;
        
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
        }
    }
}
