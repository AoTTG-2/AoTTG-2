using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class SpawnMenu : MonoBehaviour
    {
        public void Spawn()
        {
            AottgUi.TestSpawn();
            gameObject.SetActive(false);
        }

        public void Update()
        {
            Cursor.visible = true;
            Screen.lockCursor = false;
        }
    }
}
