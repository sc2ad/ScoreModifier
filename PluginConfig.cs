using BeatSaberDataWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreModifier
{
    internal class PluginConfig
    {
        public bool RegenerateConfig = true;
        public bool Enabled = true;
        public ScoreType scoreType = ScoreType.Osuv1;
        public float customScoreComboScale = 25;

        public static Func<ScoreController, int, NoteData, NoteCutInfo, int> customScoreFunc = (ScoreController sc, int score, NoteData data, NoteCutInfo info) =>
        {
            // Example for accessing ScoreController, although eventually will convert to using HTTPStatus
            int maxCombo = (int)typeof(ScoreController).GetField("_maxCombo").GetValue(sc);
            // Uses Plugin.Misses count for now
            int misses = Plugin.Misses;
            // If you miss even a single note, you no longer get any points for the remainder of the song. Otherwise, you get 1 point per successful note.
            return misses == 0 ? 1 : 0;
        };
        public static Func<int, int> getDeltaScore = (int swingScore) =>
        {
            // The score for a given swing is your overall accuracy * score + 0.1 * score
            return (int)((Data.accuracy * swingScore) + (0.1f * swingScore));
        };
        public static Func<int> getTotalScore = () =>
        {
            // The overall score is your final score multiplied by your accuracy
            return (int)(Data.accuracy * Data.finalScore);
        };
    }
}
