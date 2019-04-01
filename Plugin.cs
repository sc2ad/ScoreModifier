using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using IllusionPlugin;


namespace ScoreModifier
{
    public class Plugin : IPlugin
    {
        public string Name => Constants.Name;
        public string Version => Constants.Version;

        public static int CustomScore;
        public static int CurrentCombo;
        public static int BestCombo;
        public static int Misses;

        public void OnApplicationStart()
        {
            if (IllusionInjector.PluginManager.Plugins.Any(x => x.Name.Equals("Data Wrappers")))
            {
                SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
                SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            } else
            {
                BS_Utils.Utilities.Logger.Log(Constants.Name, "Could not find Data Wrappers Plugin! Make sure this Plugin is installed correctly!");
            }
        }

        private void SceneManagerOnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            if (!Config.enabled)
            {
                return;
            }
            if (newScene.name.Equals("MenuCore"))
            {
                //Code to execute when entering The Menu
                if (oldScene.name.Equals("GameCore"))
                {
                    // Went from GameCore to MenuCore
                    if (Config.scoreType == ScoreType.TotalFunction) {
                        CustomScore = Config.getTotalScore();
                    }
                    new GameObject("Viewer").AddComponent<ResultsViewer>();
                }
            }

            if (newScene.name.Equals("GameCore"))
            {
                //Code to execute when entering actual gameplay
                new GameObject("Score Listener").AddComponent<ScoreManager>();
            }
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            //Create GameplayOptions/SettingsUI if using either
            if (scene.name.Equals("MenuCore"))
            {
                UI.BasicUI.CreateUI();
            }

        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        public void OnLevelWasLoaded(int level)
        {

        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnUpdate()
        {


        }

        public void OnFixedUpdate()
        {
        }
    }
}
