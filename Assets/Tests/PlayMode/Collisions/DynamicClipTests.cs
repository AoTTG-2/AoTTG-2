using System;
using System.Collections;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using static UnityEngine.CollisionDetectionMode;

namespace Tests.Collisions
{
    public sealed class DynamicClipTests
    {
        [NotNull] private const string SceneName = "PlayMode-DynamicClip";
        private const int WaitFrames = 1;
        private const int Speed = 1000000;

        [NotNull] private Rigidbody player;
        [NotNull] private Rigidbody moving;

        private Plane xyPlane = new Plane(Vector3.back, Vector3.zero);

        [UnitySetUp]
        [UsedImplicitly]
        public IEnumerator LoadScene()
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
            yield return null;
            player = GameObject.Find("Player").GetComponent<Rigidbody>();
            moving = GameObject.Find("Moving").GetComponent<Rigidbody>();
        }

        [UnityTearDown]
        [UsedImplicitly]
        public IEnumerator UnloadScene()
        {
            yield return SceneManager.UnloadSceneAsync(SceneName);
            player = null!;
            moving = null!;
        }

        [Test]
        public void Scene_Is_Loaded()
        {
            Assert.That(SceneManager.GetSceneByName(SceneName).IsValid());
        }

        [Test]
        public void Player_And_Wall_Exist()
        {
            Assert.That(player, "Player does not exist");
            Assert.That(moving, "Moving does not exist");
        }
        
        [UnityTest]
        [TestCase(Discrete, Discrete, Speed, WaitFrames, true, ExpectedResult = null)]
        [TestCase(Discrete, Continuous, Speed, WaitFrames, true, ExpectedResult = null)]
        [TestCase(Discrete, ContinuousDynamic, Speed, WaitFrames, true, ExpectedResult = null)]
        [TestCase(Discrete, ContinuousSpeculative, Speed, WaitFrames, false, ExpectedResult = null)]
        [TestCase(Continuous, Discrete, Speed, WaitFrames, true, ExpectedResult = null)]
        [TestCase(Continuous, Continuous, Speed, WaitFrames, true, ExpectedResult = null)]
        [TestCase(Continuous, ContinuousDynamic, Speed, WaitFrames, false, ExpectedResult = null)]
        [TestCase(Continuous, ContinuousSpeculative, Speed, WaitFrames, false, ExpectedResult = null)]
        [TestCase(ContinuousDynamic, Discrete, Speed, WaitFrames, true, ExpectedResult = null)]
        [TestCase(ContinuousDynamic, Continuous, Speed, WaitFrames, false, ExpectedResult = null)]
        [TestCase(ContinuousDynamic, ContinuousDynamic, Speed, WaitFrames, false, ExpectedResult = null)]
        [TestCase(ContinuousDynamic, ContinuousSpeculative, Speed, WaitFrames, false, ExpectedResult = null)]
        [TestCase(ContinuousSpeculative, Discrete, Speed, WaitFrames, false, ExpectedResult = null)]
        [TestCase(ContinuousSpeculative, Continuous, Speed, WaitFrames, false, ExpectedResult = null)]
        [TestCase(ContinuousSpeculative, ContinuousDynamic, Speed, WaitFrames, false, ExpectedResult = null)]
        [TestCase(ContinuousSpeculative, ContinuousSpeculative, Speed, WaitFrames, false, ExpectedResult = null)]
        public IEnumerator Player_Clips_Through_Moving(
            CollisionDetectionMode playerMode,
            CollisionDetectionMode movingMode,
            float speed,
            int frames,
            bool doesClip)
        {
            player.collisionDetectionMode = playerMode;
            moving.collisionDetectionMode = movingMode;
            player.velocity = Vector3.forward * speed;
            moving.velocity = Vector3.back * speed;
            Assume.That(HasClipped(player), Is.False);
            for (var i = 0; i < frames; i++) yield return new WaitForFixedUpdate();
            Assert.That(HasClipped(player), Is.EqualTo(doesClip));
        }

        private bool HasClipped([NotNull] Rigidbody rigidbody)
        {
            if (rigidbody == player)
                return !xyPlane.GetSide(rigidbody.position);
            if (rigidbody == moving)
                return xyPlane.GetSide(rigidbody.position);
            throw new ArgumentOutOfRangeException();
        }
    }
}
