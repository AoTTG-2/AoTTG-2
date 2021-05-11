﻿using Assets.Scripts.UI.InGame;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UiHandler : MonoBehaviour
    {
        [SerializeField]
        private VersionManager versionManager;

        [HideInInspector]
        public UiNavigationElement[] Elements;

        public MenuUi MenuUi;
        public InGameUi InGameUi;
        public Text Version;

        public void Start()
        {
            ShowMenu();
            DontDestroyOnLoad(gameObject);
            Elements = gameObject.GetComponentsInChildren<UiNavigationElement>(true);
            foreach (var element in Elements)
            {
                element.Canvas = this;
                element.gameObject.SetActive(false);
            }
            Elements[0].gameObject.SetActive(true);
            Version.text = versionManager.Version;
        }

        public void ShowMenu()
        {
            MenuUi.gameObject.SetActive(true);
            MenuUi.ShowMainMenu();
            InGameUi.gameObject.SetActive(false);
        }

        public void ShowInGameUi()
        {
            InGameUi.gameObject.SetActive(true);
            MenuUi.gameObject.SetActive(false);
        }

        public UiNavigationElement Find(Type t)
        {
            return Elements.SingleOrDefault(x => x.GetType() == t);
        }

    }
}
