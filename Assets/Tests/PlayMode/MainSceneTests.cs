using Assets.Scripts;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Assets.Tests.PlayMode
{
    public class MainSceneTests
    {
        private bool sceneLoaded;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("AoTTG 2", LoadSceneMode.Single);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            sceneLoaded = true;
        }

        [UnityTest]
        public IEnumerator TestReferencesNotNullAfterLoad()
        {
            yield return new WaitWhile(() => !sceneLoaded);

            var startup = Resources.FindObjectsOfTypeAll<Startup>().SingleOrDefault();

            Assert.IsNotNull(startup);
            //Add all other references as well for quick nullref testing
            yield return null;
        }
    }
}
