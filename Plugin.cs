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
        public string Name => "ScoreModifier";
        public string Version => "0.0.1";

        public static int CustomScore;
        public static int CurrentCombo;
        public static int BestCombo;

        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManagerOnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            if (!Config.enabled)
            {
                return;
            }
            if (newScene.name == "MenuCore")
            {
                //Code to execute when entering The Menu
                if (oldScene.name.Equals("GameCore"))
                {
                    // Went from GameCore to MenuCore
                    new GameObject("Viewer").AddComponent<ResultsViewer>();
                }
            }

            if (newScene.name == "GameCore")
            {
                //Code to execute when entering actual gameplay
                new GameObject("Score Listener").AddComponent<ScoreManager>();
            }
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            //Create GameplayOptions/SettingsUI if using either
            if (scene.name == "MenuCore")
                UI.BasicUI.CreateUI();

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
