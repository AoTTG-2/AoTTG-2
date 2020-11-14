using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Obsolete]
public class CharacterCreateAnimationControl : MonoBehaviour
{
    [CompilerGenerated]
    private static Dictionary<string, int> f__switchSmap0;
    private string currentAnimation;
    private float interval = 10f;
    private HERO_SETUP setup;
    private float timeElapsed;

    private void play(string id)
    {
        this.currentAnimation = id;
        base.GetComponent<Animation>().Play(id);
    }

    public void playAttack(string id)
    {
        string key = id;
        if (key != null)
        {
            int num;
            if (f__switchSmap0 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(7);
                dictionary.Add("mikasa", 0);
                dictionary.Add("levi", 1);
                dictionary.Add("sasha", 2);
                dictionary.Add("jean", 3);
                dictionary.Add("marco", 4);
                dictionary.Add("armin", 5);
                dictionary.Add("petra", 6);
                f__switchSmap0 = dictionary;
            }
            if (f__switchSmap0.TryGetValue(key, out num))
            {
                switch (num)
                {
                    case 0:
                        this.currentAnimation = "attack3_1";
                        break;

                    case 1:
                        this.currentAnimation = "attack5";
                        break;

                    case 2:
                        this.currentAnimation = "special_sasha";
                        break;

                    case 3:
                        this.currentAnimation = "grabbed_jean";
                        break;

                    case 4:
                        this.currentAnimation = "special_marco_0";
                        break;

                    case 5:
                        this.currentAnimation = "special_armin";
                        break;

                    case 6:
                        this.currentAnimation = "special_petra";
                        break;
                }
            }
        }
        base.GetComponent<Animation>().Play(this.currentAnimation);
    }

    private void Start()
    {
        this.setup = base.gameObject.GetComponent<HERO_SETUP>();
        this.currentAnimation = "stand_levi";
        this.play(this.currentAnimation);
    }

    public void toStand()
    {
        if (this.setup.myCostume.sex == SEX.FEMALE)
        {
            this.currentAnimation = "stand";
        }
        else
        {
            this.currentAnimation = "stand_levi";
        }
        base.GetComponent<Animation>().CrossFade(this.currentAnimation, 0.1f);
        this.timeElapsed = 0f;
    }

    private void Update()
    {
        if ((this.currentAnimation != "stand") && (this.currentAnimation != "stand_levi"))
        {
            if (base.GetComponent<Animation>()[this.currentAnimation].normalizedTime >= 1f)
            {
                if (this.currentAnimation == "attack3_1")
                {
                    this.play("attack3_2");
                }
                else if (this.currentAnimation == "special_sasha")
                {
                    this.play("run_sasha");
                }
                else
                {
                    this.toStand();
                }
            }
        }
        else
        {
            this.timeElapsed += Time.deltaTime;
            if (this.timeElapsed > this.interval)
            {
                this.timeElapsed = 0f;
                if (UnityEngine.Random.Range(1, 0x3e8) < 350)
                {
                    this.play("salute");
                }
                else if (UnityEngine.Random.Range(1, 0x3e8) < 350)
                {
                    this.play("supply");
                }
                else
                {
                    this.play("dodge");
                }
            }
        }
    }
}

