using Assets.Scripts.Audio;
using Assets.Scripts.Characters.Titan;
using FluentAssertions;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Assets.Tests
{
    public class AudioControllerTests
    {
        private readonly GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/AudioController.prefab");

        [Test]
        public void Prefab_Dependencies_ShouldExist()
        {
            // Arrange
            var audioController = prefab.GetComponent<AudioController>();
            audioController.Should().NotBeNull();
            audioController.enabled.Should().BeTrue();

            //var thing = new PrivateObject(audioController);

            prefab.GetComponent<PhotonView>().Should().NotBeNull();
        }
    }
}
