using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans
{
    [Obsolete("This is only used for TitanHair currently")]
    public class CharacterMaterials
    {
        private static bool inited = true;
        public static Dictionary<string, Material> materials;

        public static void init()
        {
            if (inited)
            {
                inited = true;
                materials = new Dictionary<string, Material>();
                newMaterial("AOTTG_HERO_3DMG");
                newMaterial("aottg_hero_AHSS_3dmg");
                newMaterial("aottg_hero_annie_cap_causal");
                newMaterial("aottg_hero_annie_cap_uniform");
                newMaterial("aottg_hero_brand_sc");
                newMaterial("aottg_hero_brand_mp");
                newMaterial("aottg_hero_brand_g");
                newMaterial("aottg_hero_brand_ts");
                newMaterial("aottg_hero_skin_1");
                newMaterial("aottg_hero_skin_2");
                newMaterial("aottg_hero_skin_3");
                newMaterial("aottg_hero_casual_fa_1");
                newMaterial("aottg_hero_casual_fa_2");
                newMaterial("aottg_hero_casual_fa_3");
                newMaterial("aottg_hero_casual_fb_1");
                newMaterial("aottg_hero_casual_fb_2");
                newMaterial("aottg_hero_casual_ma_1");
                newMaterial("aottg_hero_casual_ma_1_ahss");
                newMaterial("aottg_hero_casual_ma_2");
                newMaterial("aottg_hero_casual_ma_3");
                newMaterial("aottg_hero_casual_mb_1");
                newMaterial("aottg_hero_casual_mb_2");
                newMaterial("aottg_hero_casual_mb_3");
                newMaterial("aottg_hero_casual_mb_4");
                newMaterial("aottg_hero_uniform_fa_1");
                newMaterial("aottg_hero_uniform_fa_2");
                newMaterial("aottg_hero_uniform_fa_3");
                newMaterial("aottg_hero_uniform_fb_1");
                newMaterial("aottg_hero_uniform_fb_2");
                newMaterial("aottg_hero_uniform_ma_1");
                newMaterial("aottg_hero_uniform_ma_2");
                newMaterial("aottg_hero_uniform_ma_3");
                newMaterial("aottg_hero_uniform_mb_1");
                newMaterial("aottg_hero_uniform_mb_2");
                newMaterial("aottg_hero_uniform_mb_3");
                newMaterial("aottg_hero_uniform_mb_4");
                newMaterial("hair_annie");
                newMaterial("hair_armin");
                newMaterial("hair_boy1");
                newMaterial("hair_boy2");
                newMaterial("hair_boy3");
                newMaterial("hair_boy4");
                newMaterial("hair_eren");
                newMaterial("hair_girl1");
                newMaterial("hair_girl2");
                newMaterial("hair_girl3");
                newMaterial("hair_girl4");
                newMaterial("hair_girl5");
                newMaterial("hair_hanji");
                newMaterial("hair_jean");
                newMaterial("hair_levi");
                newMaterial("hair_marco");
                newMaterial("hair_mike");
                newMaterial("hair_petra");
                newMaterial("hair_rico");
                newMaterial("hair_sasha");
                newMaterial("hair_mikasa");
                Texture texture = (Texture) UnityEngine.Object.Instantiate(Resources.Load("NewTexture/aottg_hero_eyes"));
                Material material = (Material) UnityEngine.Object.Instantiate(Resources.Load("NewTexture/MaterialGLASS"));
                material.mainTexture = texture;
                materials.Add("aottg_hero_eyes", material);
            }
        }

        private static void newMaterial(string pref)
        {

            var texture = (Texture) UnityEngine.Object.Instantiate(Resources.Load("NewTexture/" + pref));
            var material = (Material) UnityEngine.Object.Instantiate(Resources.Load("NewTexture/MaterialCharacter"));
            material.mainTexture = texture;
            materials.Add(pref, material);
        }
    }
}

