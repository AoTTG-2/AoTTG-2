using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Body;
using FluentAssertions;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Assets.Tests.EditorMode.Titan
{
    public class ErenTitanTests
    {
        private readonly GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/ErenTitan.prefab");

        // A Test behaves as an ordinary method
        [Test]
        public void Prefab_Dependencies_ShouldExist()
        {
            // Arrange
            var erenTitan = prefab.GetComponent<ErenTitan>();
            erenTitan.Should().NotBeNull();
            erenTitan.enabled.Should().BeTrue();

            (erenTitan.HealthLabel != null).Should().BeTrue();
            (erenTitan.NavMeshAgent != null).Should().BeTrue();
            (erenTitan.AudioSourceFoot != null).Should().BeTrue();
            (erenTitan.bottomObject != null).Should().BeTrue();

            prefab.GetComponent<PhotonView>().Should().NotBeNull();
            prefab.GetComponent<ErenTitanBody>().Should().NotBeNull();

            prefab.transform.Find("MinimapIcon").Should().NotBeNull();
        }
    }
}
