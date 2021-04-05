using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Body;
using FluentAssertions;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Assets.Tests.EditorMode.Titan
{
    public class PlayerTitanTests
    {
        private readonly GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/PlayerTitan.prefab");

        // A Test behaves as an ordinary method
        [Test]
        public void Prefab_Dependencies_ShouldExist()
        {
            // Arrange
            var playerTitan = prefab.GetComponent<PlayerTitan>();
            playerTitan.Should().NotBeNull();
            playerTitan.enabled.Should().BeTrue();

            (playerTitan.HealthLabel != null).Should().BeTrue();
            (playerTitan.AudioSourceFoot != null).Should().BeTrue();
            (playerTitan.Body != null).Should().BeTrue();
            (playerTitan.SetupScript != null).Should().BeTrue();
            (playerTitan.minimapIcon != null).Should().BeTrue();

            prefab.GetComponent<PhotonView>().Should().NotBeNull();
            prefab.GetComponent<MindlessTitanBody>().Should().NotBeNull();

            prefab.transform.Find("MinimapIcon").Should().NotBeNull();
        }
    }
}
