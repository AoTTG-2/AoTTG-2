using System;

namespace Assets.Scripts.Characters.Humans
{
    [Obsolete("No longer used for Humans, however Titan hair does still rely on this. Should thus be refactored.")]
    public class CostumeHair
    {
        public string hair = string.Empty;
        public string hair_1 = string.Empty;
        public static CostumeHair[] hairsF;
        public static CostumeHair[] hairsM;
        public bool hasCloth;
        public int id;
        public string texture = string.Empty;

        public static void init()
        {
            hairsM = new CostumeHair[11];
            hairsM[0] = new CostumeHair();
            hairsM[0].hair = hairsM[0].texture = "hair_boy1";
            hairsM[1] = new CostumeHair();
            hairsM[1].hair = hairsM[1].texture = "hair_boy2";
            hairsM[2] = new CostumeHair();
            hairsM[2].hair = hairsM[2].texture = "hair_boy3";
            hairsM[3] = new CostumeHair();
            hairsM[3].hair = hairsM[3].texture = "hair_boy4";
            hairsM[4] = new CostumeHair();
            hairsM[4].hair = hairsM[4].texture = "hair_eren";
            hairsM[5] = new CostumeHair();
            hairsM[5].hair = hairsM[5].texture = "hair_armin";
            hairsM[6] = new CostumeHair();
            hairsM[6].hair = hairsM[6].texture = "hair_jean";
            hairsM[7] = new CostumeHair();
            hairsM[7].hair = hairsM[7].texture = "hair_levi";
            hairsM[8] = new CostumeHair();
            hairsM[8].hair = hairsM[8].texture = "hair_marco";
            hairsM[9] = new CostumeHair();
            hairsM[9].hair = hairsM[9].texture = "hair_mike";
            hairsM[10] = new CostumeHair();
            hairsM[10].hair = hairsM[10].texture = string.Empty;
            for (int i = 0; i <= 10; i++)
            {
                hairsM[i].id = i;
            }
            hairsF = new CostumeHair[11];
            hairsF[0] = new CostumeHair();
            hairsF[0].hair = hairsF[0].texture = "hair_girl1";
            hairsF[1] = new CostumeHair();
            hairsF[1].hair = hairsF[1].texture = "hair_girl2";
            hairsF[2] = new CostumeHair();
            hairsF[2].hair = hairsF[2].texture = "hair_girl3";
            hairsF[3] = new CostumeHair();
            hairsF[3].hair = hairsF[3].texture = "hair_girl4";
            hairsF[4] = new CostumeHair();
            hairsF[4].hair = hairsF[4].texture = "hair_girl5";
            hairsF[4].hasCloth = true;
            hairsF[4].hair_1 = "hair_girl5_cloth";
            hairsF[5] = new CostumeHair();
            hairsF[5].hair = hairsF[5].texture = "hair_annie";
            hairsF[6] = new CostumeHair();
            hairsF[6].hair = hairsF[6].texture = "hair_hanji";
            hairsF[6].hasCloth = true;
            hairsF[6].hair_1 = "hair_hanji_cloth";
            hairsF[7] = new CostumeHair();
            hairsF[7].hair = hairsF[7].texture = "hair_mikasa";
            hairsF[8] = new CostumeHair();
            hairsF[8].hair = hairsF[8].texture = "hair_petra";
            hairsF[9] = new CostumeHair();
            hairsF[9].hair = hairsF[9].texture = "hair_rico";
            hairsF[10] = new CostumeHair();
            hairsF[10].hair = hairsF[10].texture = "hair_sasha";
            hairsF[10].hasCloth = true;
            hairsF[10].hair_1 = "hair_sasha_cloth";
            for (int j = 0; j <= 10; j++)
            {
                hairsF[j].id = j;
            }
        }
    }
}

