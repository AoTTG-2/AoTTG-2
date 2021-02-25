using Assets.Scripts.Characters.Titan.Body;
using FluentAssertions;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Assets.Tests.EditorMode.Titan
{
    public class FemaleTitanTests
    {
        private readonly GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/FemaleTitan.prefab");

        // A Test behaves as an ordinary method
        [Test]
        public void Prefab_Dependencies_ShouldExist()
        {
            // Arrange
            var femaleTitan = prefab.GetComponent<FemaleTitan>();
            femaleTitan.Should().NotBeNull();
            femaleTitan.enabled.Should().BeTrue();

            (femaleTitan.healthLabel != null).Should().BeTrue();
            (femaleTitan.bottomObject != null).Should().BeTrue();

            prefab.GetComponent<PhotonView>().Should().NotBeNull();
            prefab.GetComponent<FemaleTitanBody>().Should().NotBeNull();

            prefab.transform.Find("MinimapIcon").Should().NotBeNull();
        }
    }
}
