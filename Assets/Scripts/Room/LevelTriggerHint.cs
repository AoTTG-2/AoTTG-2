using System;
using UnityEngine;

[Obsolete("AoTTG Legacy Tutorial script. In AoTTG2 the tutorial will be handled by the training gamemode.")]
public class LevelTriggerHint : MonoBehaviour
{
    public string content;
    public HintType myhint;
    private bool on;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.on = true;
        }
    }

    private void Start()
    {
        throw new NotImplementedException("This is the old tutorial level. Will be replaced by a new tutorial in the future.");
        //if (!FengGameManagerMKII.Gamemode.Settings.Hint)
        //{
        //    base.enabled = false;
        //}
        //if (this.content == string.Empty)
        //{
        //    switch (this.myhint)
        //    {
        //        case HintType.MOVE:
        //        {
        //            string[] textArray1 = new string[] { "Hello soldier!\nWelcome to Attack On Titan Tribute Game!\n Press [F7D358]", GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().inputString[InputCode.up], GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().inputString[InputCode.left], GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().inputString[InputCode.down], GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().inputString[InputCode.right], "[-] to Move." };
        //            this.content = string.Concat(textArray1);
        //            break;
        //        }
        //        case HintType.TELE:
        //            this.content = "Move to [82FA58]green warp point[-] to proceed.";
        //            break;

        //        case HintType.CAMA:
        //        {
        //            string[] textArray2 = new string[]
        //            {
        //                $"Press [F7D358] {InputManager.GetKey(InputUi.Camera)} [-] to change camera mode\n" +
        //                $"Press [F7D358] {InputManager.GetKey(InputUi.ToggleCursor)} [-] to hide or show the cursor."
        //            };
        //            this.content = string.Concat(textArray2);
        //            break;
        //        }
        //        case HintType.JUMP:
        //            this.content = "Press [F7D358]" + GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().inputString[InputCode.jump] + "[-] to Jump.";
        //            break;

        //        case HintType.JUMP2:
        //            this.content = "Press [F7D358]" + GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().inputString[InputCode.up] + "[-] towards a wall to perform a wall-run.";
        //            break;

        //        case HintType.HOOK:
        //        {
        //            string[] textArray3 = new string[] { "Press and Hold[F7D358] ", GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().inputString[InputCode.leftRope], "[-] or [F7D358]", GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().inputString[InputCode.rightRope], "[-] to launch your grapple.\nNow Try hooking to the [>3<] box. " };
        //            this.content = string.Concat(textArray3);
        //            break;
        //        }
        //        case HintType.HOOK2:
        //        {
        //            string[] textArray4 = new string[] { "Press and Hold[F7D358] ", GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().inputString[InputCode.bothRope], "[-] to launch both of your grapples at the same Time.\n\nNow aim between the two black blocks. \nYou will see the mark '<' and '>' appearing on the blocks. \nThen press ", GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().inputString[InputCode.bothRope], " to hook the blocks." };
        //            this.content = string.Concat(textArray4);
        //            break;
        //        }
        //        case HintType.SUPPLY:
        //            this.content = "Press [F7D358]" + GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().inputString[InputCode.reload] + "[-] to reload your blades.\n Move to the supply station to refill your gas and blades.";
        //            break;

        //        case HintType.DODGE:
        //            this.content = "Press [F7D358]" + GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().inputString[InputCode.dodge] + "[-] to Dodge.";
        //            break;

        //        case HintType.ATTACK:
        //        {
        //            string[] textArray5 = new string[] { "Press [F7D358]", GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().inputString[InputCode.attack0], "[-] to Attack. \nPress [F7D358]", GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().inputString[InputCode.attack1], "[-] to use special attack.\n***You can only kill a titan by slashing his [FA5858]NAPE[-].***\n\n" };
        //            this.content = string.Concat(textArray5);
        //            break;
        //        }
        //    }
        //}
    }

    private void Update()
    {
        if (this.on)
        {
            //GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ShowHUDInfoCenter(this.content + "\n\n\n\n\n");
            this.on = false;
        }
    }
}

