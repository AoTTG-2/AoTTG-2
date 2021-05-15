using Assets.Scripts.Characters.Titan;
using FluentAssertions;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Assets.Tests.EditorMode.Titan
{
    public class ColossalTitanTests
    {
        private readonly GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ColossalTitan.prefab");

        // A Test behaves as an ordinary method
        [Test]
        public void Prefab_Dependencies_ShouldExist()
        {
            // Arrange
            var colossalTitan = prefab.GetComponent<ColossalTitan>();
            colossalTitan.Should().NotBeNull();
            colossalTitan.enabled.Should().BeTrue();

            (colossalTitan.healthLabel != null).Should().BeTrue();
            (colossalTitan.bottomObject != null).Should().BeTrue();

            prefab.GetComponent<PhotonView>().Should().NotBeNull();
        }
    }
}
