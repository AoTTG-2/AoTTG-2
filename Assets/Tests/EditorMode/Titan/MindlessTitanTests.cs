using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Body;
using FluentAssertions;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Assets.Tests.EditorMode.Titan
{
    public class MindlessTitanTests
    {
        private readonly GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/MindlessTitan.prefab");

        // A Test behaves as an ordinary method
        [Test]
        public void Prefab_Dependencies_ShouldExist()
        {
            // Arrange
            var mindlessTitan = prefab.GetComponent<MindlessTitan>();
            mindlessTitan.Should().NotBeNull();
            mindlessTitan.enabled.Should().BeTrue();

            (mindlessTitan.HealthLabel != null).Should().BeTrue();
            (mindlessTitan.AudioSourceFoot != null).Should().BeTrue();
            (mindlessTitan.Body != null).Should().BeTrue();
            (mindlessTitan.SetupScript != null).Should().BeTrue();
            (mindlessTitan.minimapIcon != null).Should().BeTrue();

            prefab.GetComponent<PhotonView>().Should().NotBeNull();
            prefab.GetComponent<MindlessTitanBody>().Should().NotBeNull();

            prefab.transform.Find("MinimapIcon").Should().NotBeNull();
        }
    }
}
