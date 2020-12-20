using Assets.Scripts;
using Assets.Scripts.UI.Input;
using UnityEngine;

public class SpectatorMovement : MonoBehaviour
{
    public bool disable;
    private float speed = 100f;
    private bool isRestarting = true;
    private float startingTime;

    private void Awake()
    {
        startingTime = Time.time;
    }

    private void Update()
    {
        if (!this.disable)
        {
            float num2;
            float num3;
            float speed = this.speed;

            if (InputManager.Key(InputHuman.Jump))
            {
                speed *= 3f;
            }
            if (InputManager.Key(InputHuman.Forward))
            {
                num2 = 1f;
            }
            else if (InputManager.Key(InputHuman.Backward))
            {
                num2 = -1f;
            }

            else
            {
                num2 = 0f;
            }
    
            if (InputManager.Key(InputHuman.Left))
            {
                num3 = -1f;
            }      
            else if (InputManager.Key(InputHuman.Right))
            {
                num3 = 1f;
            }
        
            else
            {
                num3 = 0f;
            }
            Transform transform = base.transform;
            if (num2 > 0f)
            {
                transform.position += (Vector3) ((base.transform.forward * speed) * Time.deltaTime);
            }
            else if (num2 < 0f)
            {
                transform.position -= (Vector3) ((base.transform.forward * speed) * Time.deltaTime);
            }
            if (isRestarting && Time.time - startingTime >= 0.5f && !InputManager.Key(InputUi.Restart))
                isRestarting = false;

            if (InputManager.KeyDown(InputUi.Restart) && PhotonNetwork.offlineMode && !isRestarting)
            {
                FengGameManagerMKII.instance.restartRC();
            }
            if (num3 > 0f)
            {
                transform.position += (Vector3) ((base.transform.right * speed) * Time.deltaTime);
            }
            else if (num3 < 0f)
            {
                transform.position -= (Vector3) ((base.transform.right * speed) * Time.deltaTime);
            }
            if (InputManager.Key(InputHuman.HookLeft))
            {
                transform.position -= (Vector3) ((base.transform.up * speed) * Time.deltaTime);
            }
            else if (InputManager.Key(InputHuman.HookRight))
            {
                transform.position += (Vector3) ((base.transform.up * speed) * Time.deltaTime);
            }
        }
    }
}

