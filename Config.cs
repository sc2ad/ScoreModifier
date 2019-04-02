using BS_Utils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreModifier
{
    class Config
    {
        private static BS_Utils.Utilities.Config config = new BS_Utils.Utilities.Config(Constants.Name);
        public static bool enabled {
            get
            {
                return config.GetBool(Constants.Name, "Enabled", true);
            }
            set
            {
                config.SetBool(Constants.Name, "Enabled", value);
            }
        }
        public static ScoreType scoreType {
            get
            {
                ScoreType result = ScoreType.Osuv1;
                Enum.TryParse(config.GetString(Constants.Name, "ScoreType", ScoreType.Osuv1.ToString()), out result);
                return result;
            }
            set
            {
                config.SetString(Constants.Name, "ScoreType", value.ToString());
            }
        }
        public static float customScoreComboScale
        {
            get
            {
                return config.GetFloat(Constants.Name, "CustomScoreComboScale", 25);
            }
            set
            {
                config.SetFloat(Constants.Name, "CustomScoreComboScale", value);
            }
        }
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
            return (int)((BS_Utils.Plugin.dataManager.data.accuracy * swingScore) + (0.1f * swingScore));
        };
        public static Func<int> getTotalScore = () =>
        {
            // The overall score is your final score multiplied by your accuracy
            return (int)(BS_Utils.Plugin.dataManager.data.accuracy * BS_Utils.Plugin.dataManager.data.finalScore);
        };
    }
}
