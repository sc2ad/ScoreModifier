using IPA;
using IPA.Config;
using IPA.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;


namespace ScoreModifier
{
    internal static class Logger
    {
        public static IPALogger log { get; set; }
    }
    public class Plugin : IBeatSaberPlugin
    {
        public string Name => Constants.Name;
        public string Version => Constants.Version;

        public static int CustomScore;
        public static int CurrentCombo;
        public static int BestCombo;
        public static int Misses;

        internal static Ref<PluginConfig> config;
        internal static IConfigProvider configProvider;

        public void Init(IPALogger logger, [Config.Prefer("json")] IConfigProvider cfgProvider)
        {
            Logger.log = logger;
            configProvider = cfgProvider;

            config = cfgProvider.MakeLink<PluginConfig>((p, v) =>
            {
                if (v.Value == null || v.Value.RegenerateConfig)
                    p.Store(v.Value = new PluginConfig() { RegenerateConfig = false });
                config = v;
            });
        }

        public void OnApplicationStart()
        {
            Logger.log.Debug("OnApplicationStart");
        }

        public void OnApplicationQuit()
        {
            Logger.log.Debug("OnApplicationQuit");
        }

        public void OnFixedUpdate()
        {

        }

        public void OnUpdate()
        {

        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (!config.Value.Enabled)
            {
                return;
            }
            if (nextScene.name.Equals("MenuCore"))
            {
                //Code to execute when entering The Menu
                if (prevScene.name.Equals("GameCore"))
                {
                    // Went from GameCore to MenuCore
                    if (config.Value.scoreType == ScoreType.TotalFunction)
                    {
                        CustomScore = PluginConfig.getTotalScore();
                    }
                    new GameObject("Viewer").AddComponent<ResultsViewer>();
                }
            }

            if (nextScene.name.Equals("GameCore"))
            {
                //Code to execute when entering actual gameplay
                new GameObject("Score Listener").AddComponent<ScoreManager>();
            }
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {

        }

        public void OnSceneUnloaded(Scene scene)
        {

        }
    }
}
