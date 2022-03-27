using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using static UnityEngine.CollisionDetectionMode;

namespace Tests.Collisions
{
    public sealed class KinematicClipTests
    {
        [NotNull] private const string SceneName = "PlayMode-KinematicClip";

        [NotNull] private Rigidbody player;
        [NotNull] private Transform wall;

        private Plane wallPlane;

        [UnitySetUp]
        [UsedImplicitly]
        public IEnumerator LoadScene()
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
            yield return null;
            player = GameObject.Find("Player").GetComponent<Rigidbody>();
            wall = GameObject.Find("Wall").transform;
            wallPlane = new Plane(wall.up, wall.position);
        }

        [UnityTearDown]
        [UsedImplicitly]
        public IEnumerator UnloadScene()
        {
            yield return SceneManager.UnloadSceneAsync(SceneName);
            player = null!;
            wall = null!;
            wallPlane = default;
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
            Assert.That(wall, "Wall does not exist");
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<TestCaseData> Create_Player_Clips_Through_Wall_Shared_TestCases()
        {
            yield return Create(Discrete, true);
            yield return Create(Continuous, false);
            yield return Create(ContinuousDynamic, false);
            yield return Create(ContinuousSpeculative, false);

            static TestCaseData Create(CollisionDetectionMode mode, bool doesClip) =>
                new TestCaseData(mode, 1000f, 1, doesClip).Returns(null);
        }

        [UnityTest]
        [TestCaseSource(nameof(Create_Player_Clips_Through_Wall_Shared_TestCases))]
        public IEnumerator Player_Clips_Through_Wall(
            CollisionDetectionMode mode,
            float speed,
            int frames,
            bool doesClip)
        {
            player.collisionDetectionMode = mode;
            player.velocity = Vector3.forward * speed;
            Assume.That(HasPlayerClipped(), Is.False);
            for (var i = 0; i < frames; i++) yield return new WaitForFixedUpdate();
            Assert.That(HasPlayerClipped(), Is.EqualTo(doesClip));
        }

        private bool HasPlayerClipped() => !wallPlane.GetSide(player.position);
    }
}
