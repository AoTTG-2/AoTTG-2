using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.UI
{
    public class Speedometer : MonoBehaviour
    {
        private TMP_Text text;
        private Rigidbody playerRigidbody;
        private float currentColor = 0f;
        private const float COLOR_SHIFT_SPEED = 1.5f;

        private void Awake() => text = GetComponent<TMP_Text>();
        private void OnEnable() => Hero.OnSpawn += OnPlayerSpawn;
        private void OnDisable() => Hero.OnSpawn -= OnPlayerSpawn;

        private void OnPlayerSpawn(Rigidbody rigidbody)
        {
            playerRigidbody = rigidbody;

            StartCoroutine(Rainbow());
        }

        private IEnumerator Rainbow()
        {
            while (playerRigidbody)
            {
                text.text = $"Speed:\n{playerRigidbody.velocity.magnitude:N2}";

                currentColor += COLOR_SHIFT_SPEED * Time.deltaTime;
                if (currentColor >= 1f)
                    currentColor -= 1f;

                text.color = Color.HSVToRGB(currentColor, 1f, 1f);

                yield return 0;
            }
        }
    }
}