using Assets.Scripts;
using Assets.Scripts.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Hero
{
    public void LoadSkin()
    {
        if (photonView.isMine)
        {
            if (((int) FengGameManagerMKII.settings[93]) == 1)
            {
                foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
                {
                    if (renderer.name.Contains("speed"))
                    {
                        renderer.enabled = false;
                    }
                }
            }
            if (((int) FengGameManagerMKII.settings[0]) == 1)
            {
                int index = 14;
                int num3 = 4;
                int num4 = 5;
                int num5 = 6;
                int num6 = 7;
                int num7 = 8;
                int num8 = 9;
                int num9 = 10;
                int num10 = 11;
                int num11 = 12;
                int num12 = 13;
                int num13 = 3;
                int num14 = 94;
                if (((int) FengGameManagerMKII.settings[133]) == 1)
                {
                    num13 = 134;
                    num3 = 135;
                    num4 = 136;
                    num5 = 137;
                    num6 = 138;
                    num7 = 139;
                    num8 = 140;
                    num9 = 141;
                    num10 = 142;
                    num11 = 143;
                    num12 = 144;
                    index = 145;
                    num14 = 146;
                }
                else if (((int) FengGameManagerMKII.settings[133]) == 2)
                {
                    num13 = 147;
                    num3 = 148;
                    num4 = 149;
                    num5 = 150;
                    num6 = 151;
                    num7 = 152;
                    num8 = 153;
                    num9 = 154;
                    num10 = 155;
                    num11 = 156;
                    num12 = 157;
                    index = 158;
                    num14 = 159;
                }
                string str = (string) FengGameManagerMKII.settings[index];
                string str2 = (string) FengGameManagerMKII.settings[num3];
                string str3 = (string) FengGameManagerMKII.settings[num4];
                string str4 = (string) FengGameManagerMKII.settings[num5];
                string str5 = (string) FengGameManagerMKII.settings[num6];
                string str6 = (string) FengGameManagerMKII.settings[num7];
                string str7 = (string) FengGameManagerMKII.settings[num8];
                string str8 = (string) FengGameManagerMKII.settings[num9];
                string str9 = (string) FengGameManagerMKII.settings[num10];
                string str10 = (string) FengGameManagerMKII.settings[num11];
                string str11 = (string) FengGameManagerMKII.settings[num12];
                string str12 = (string) FengGameManagerMKII.settings[num13];
                string str13 = (string) FengGameManagerMKII.settings[num14];
                string url = str12 + "," + str2 + "," + str3 + "," + str4 + "," + str5 + "," + str6 + "," + str7 + "," + str8 + "," + str9 + "," + str10 + "," + str11 + "," + str + "," + str13;
                int viewID = -1;
                if (myHorse)
                {
                    viewID = myHorse.photonView.viewID;
                }
                photonView.RPC(nameof(LoadSkinRPC), PhotonTargets.AllBuffered, new object[] { viewID, url });
            }
        }
    }

    public IEnumerator LoadSkinE(int horseId, string url)
    {
        while (!hasspawn)
        {
            yield return null;
        }
        bool mipmap = true;
        bool iteratorVariable1 = false;
        if (((int) FengGameManagerMKII.settings[63]) == 1)
        {
            mipmap = false;
        }
        string[] iteratorVariable2 = url.Split(new char[] { ',' });
        bool iteratorVariable3 = false;
        if (((int) FengGameManagerMKII.settings[15]) == 0)
        {
            iteratorVariable3 = true;
        }
        bool iteratorVariable4 = false;
        if (GameSettings.Horse.Enabled.Value)
        {
            iteratorVariable4 = true;
        }
        bool iteratorVariable5 = false;
        if (photonView.isMine)
        {
            iteratorVariable5 = true;
        }
        if (setup.part_hair_1 != null)
        {
            Renderer renderer = setup.part_hair_1.GetComponent<Renderer>();
            if ((iteratorVariable2[1].EndsWith(".jpg") || iteratorVariable2[1].EndsWith(".png")) || iteratorVariable2[1].EndsWith(".jpeg"))
            {
                if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
                {
                    WWW link = new WWW(iteratorVariable2[1]);
                    yield return link;
                    Texture2D iteratorVariable8 = RCextensions.loadimage(link, mipmap, 0x30d40);
                    link.Dispose();
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
                    {
                        iteratorVariable1 = true;
                        if (setup.myCostume.hairInfo.id >= 0)
                        {
                            renderer.material = CharacterMaterials.materials[setup.myCostume.hairInfo.texture];
                        }
                        renderer.material.mainTexture = iteratorVariable8;
                        FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[1], renderer.material);
                        renderer.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                    }
                    else
                    {
                        renderer.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                    }
                }
                else
                {
                    renderer.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                }
            }
            else if (iteratorVariable2[1].ToLower() == "transparent")
            {
                renderer.enabled = false;
            }
        }
        if (setup.part_cape != null)
        {
            Renderer iteratorVariable9 = setup.part_cape.GetComponent<Renderer>();
            if ((iteratorVariable2[7].EndsWith(".jpg") || iteratorVariable2[7].EndsWith(".png")) || iteratorVariable2[7].EndsWith(".jpeg"))
            {
                if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
                {
                    WWW iteratorVariable10 = new WWW(iteratorVariable2[7]);
                    yield return iteratorVariable10;
                    Texture2D iteratorVariable11 = RCextensions.loadimage(iteratorVariable10, mipmap, 0x30d40);
                    iteratorVariable10.Dispose();
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
                    {
                        iteratorVariable1 = true;
                        iteratorVariable9.material.mainTexture = iteratorVariable11;
                        FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[7], iteratorVariable9.material);
                        iteratorVariable9.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                    }
                    else
                    {
                        iteratorVariable9.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                    }
                }
                else
                {
                    iteratorVariable9.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                }
            }
            else if (iteratorVariable2[7].ToLower() == "transparent")
            {
                iteratorVariable9.enabled = false;
            }
        }
        if (setup.part_chest_3 != null)
        {
            Renderer iteratorVariable12 = setup.part_chest_3.GetComponent<Renderer>();
            if ((iteratorVariable2[6].EndsWith(".jpg") || iteratorVariable2[6].EndsWith(".png")) || iteratorVariable2[6].EndsWith(".jpeg"))
            {
                if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
                {
                    WWW iteratorVariable13 = new WWW(iteratorVariable2[6]);
                    yield return iteratorVariable13;
                    Texture2D iteratorVariable14 = RCextensions.loadimage(iteratorVariable13, mipmap, 0x7a120);
                    iteratorVariable13.Dispose();
                    if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
                    {
                        iteratorVariable1 = true;
                        iteratorVariable12.material.mainTexture = iteratorVariable14;
                        FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[6], iteratorVariable12.material);
                        iteratorVariable12.material = (Material) FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                    }
                    else
                    {
                        iteratorVariable12.material = (Material) FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                    }
                }
                else
                {
                    iteratorVariable12.material = (Material) FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                }
            }
            else if (iteratorVariable2[6].ToLower() == "transparent")
            {
                iteratorVariable12.enabled = false;
            }
        }
        foreach (Renderer iteratorVariable15 in GetComponentsInChildren<Renderer>())
        {
            if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[1]))
            {
                if ((iteratorVariable2[1].EndsWith(".jpg") || iteratorVariable2[1].EndsWith(".png")) || iteratorVariable2[1].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
                    {
                        WWW iteratorVariable16 = new WWW(iteratorVariable2[1]);
                        yield return iteratorVariable16;
                        Texture2D iteratorVariable17 = RCextensions.loadimage(iteratorVariable16, mipmap, 0x30d40);
                        iteratorVariable16.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
                        {
                            iteratorVariable1 = true;
                            if (setup.myCostume.hairInfo.id >= 0)
                            {
                                iteratorVariable15.material = CharacterMaterials.materials[setup.myCostume.hairInfo.texture];
                            }
                            iteratorVariable15.material.mainTexture = iteratorVariable17;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[1], iteratorVariable15.material);
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                    }
                }
                else if (iteratorVariable2[1].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[2]))
            {
                if ((iteratorVariable2[2].EndsWith(".jpg") || iteratorVariable2[2].EndsWith(".png")) || iteratorVariable2[2].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[2]))
                    {
                        WWW iteratorVariable18 = new WWW(iteratorVariable2[2]);
                        yield return iteratorVariable18;
                        Texture2D iteratorVariable19 = RCextensions.loadimage(iteratorVariable18, mipmap, 0x30d40);
                        iteratorVariable18.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[2]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTextureScale = (Vector2) (iteratorVariable15.material.mainTextureScale * 8f);
                            iteratorVariable15.material.mainTextureOffset = new Vector2(0f, 0f);
                            iteratorVariable15.material.mainTexture = iteratorVariable19;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[2], iteratorVariable15.material);
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[2]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[2]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[2]];
                    }
                }
                else if (iteratorVariable2[2].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[3]))
            {
                if ((iteratorVariable2[3].EndsWith(".jpg") || iteratorVariable2[3].EndsWith(".png")) || iteratorVariable2[3].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[3]))
                    {
                        WWW iteratorVariable20 = new WWW(iteratorVariable2[3]);
                        yield return iteratorVariable20;
                        Texture2D iteratorVariable21 = RCextensions.loadimage(iteratorVariable20, mipmap, 0x30d40);
                        iteratorVariable20.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[3]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTextureScale = (Vector2) (iteratorVariable15.material.mainTextureScale * 8f);
                            iteratorVariable15.material.mainTextureOffset = new Vector2(0f, 0f);
                            iteratorVariable15.material.mainTexture = iteratorVariable21;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[3], iteratorVariable15.material);
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[3]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[3]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[3]];
                    }
                }
                else if (iteratorVariable2[3].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[4]))
            {
                if ((iteratorVariable2[4].EndsWith(".jpg") || iteratorVariable2[4].EndsWith(".png")) || iteratorVariable2[4].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[4]))
                    {
                        WWW iteratorVariable22 = new WWW(iteratorVariable2[4]);
                        yield return iteratorVariable22;
                        Texture2D iteratorVariable23 = RCextensions.loadimage(iteratorVariable22, mipmap, 0x30d40);
                        iteratorVariable22.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[4]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTextureScale = (Vector2) (iteratorVariable15.material.mainTextureScale * 8f);
                            iteratorVariable15.material.mainTextureOffset = new Vector2(0f, 0f);
                            iteratorVariable15.material.mainTexture = iteratorVariable23;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[4], iteratorVariable15.material);
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[4]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[4]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[4]];
                    }
                }
                else if (iteratorVariable2[4].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if ((iteratorVariable15.name.Contains(FengGameManagerMKII.s[5]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[6])) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[10]))
            {
                if ((iteratorVariable2[5].EndsWith(".jpg") || iteratorVariable2[5].EndsWith(".png")) || iteratorVariable2[5].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[5]))
                    {
                        WWW iteratorVariable24 = new WWW(iteratorVariable2[5]);
                        yield return iteratorVariable24;
                        Texture2D iteratorVariable25 = RCextensions.loadimage(iteratorVariable24, mipmap, 0x30d40);
                        iteratorVariable24.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[5]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable25;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[5], iteratorVariable15.material);
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[5]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[5]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[5]];
                    }
                }
                else if (iteratorVariable2[5].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (((iteratorVariable15.name.Contains(FengGameManagerMKII.s[7]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[8])) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[9])) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[24]))
            {
                if ((iteratorVariable2[6].EndsWith(".jpg") || iteratorVariable2[6].EndsWith(".png")) || iteratorVariable2[6].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
                    {
                        WWW iteratorVariable26 = new WWW(iteratorVariable2[6]);
                        yield return iteratorVariable26;
                        Texture2D iteratorVariable27 = RCextensions.loadimage(iteratorVariable26, mipmap, 0x7a120);
                        iteratorVariable26.Dispose();
                        if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable27;
                            FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[6], iteratorVariable15.material);
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                    }
                }
                else if (iteratorVariable2[6].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[11]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[12]))
            {
                if ((iteratorVariable2[7].EndsWith(".jpg") || iteratorVariable2[7].EndsWith(".png")) || iteratorVariable2[7].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
                    {
                        WWW iteratorVariable28 = new WWW(iteratorVariable2[7]);
                        yield return iteratorVariable28;
                        Texture2D iteratorVariable29 = RCextensions.loadimage(iteratorVariable28, mipmap, 0x30d40);
                        iteratorVariable28.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable29;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[7], iteratorVariable15.material);
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                    }
                }
                else if (iteratorVariable2[7].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[15]) || ((iteratorVariable15.name.Contains(FengGameManagerMKII.s[13]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[26])) && !iteratorVariable15.name.Contains("_r")))
            {
                if ((iteratorVariable2[8].EndsWith(".jpg") || iteratorVariable2[8].EndsWith(".png")) || iteratorVariable2[8].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[8]))
                    {
                        WWW iteratorVariable30 = new WWW(iteratorVariable2[8]);
                        yield return iteratorVariable30;
                        Texture2D iteratorVariable31 = RCextensions.loadimage(iteratorVariable30, mipmap, 0x7a120);
                        iteratorVariable30.Dispose();
                        if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[8]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable31;
                            FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[8], iteratorVariable15.material);
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[1][iteratorVariable2[8]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[1][iteratorVariable2[8]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[1][iteratorVariable2[8]];
                    }
                }
                else if (iteratorVariable2[8].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if ((iteratorVariable15.name.Contains(FengGameManagerMKII.s[17]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[16])) || (iteratorVariable15.name.Contains(FengGameManagerMKII.s[26]) && iteratorVariable15.name.Contains("_r")))
            {
                if ((iteratorVariable2[9].EndsWith(".jpg") || iteratorVariable2[9].EndsWith(".png")) || iteratorVariable2[9].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[9]))
                    {
                        WWW iteratorVariable32 = new WWW(iteratorVariable2[9]);
                        yield return iteratorVariable32;
                        Texture2D iteratorVariable33 = RCextensions.loadimage(iteratorVariable32, mipmap, 0x7a120);
                        iteratorVariable32.Dispose();
                        if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[9]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable33;
                            FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[9], iteratorVariable15.material);
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[1][iteratorVariable2[9]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[1][iteratorVariable2[9]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[1][iteratorVariable2[9]];
                    }
                }
                else if (iteratorVariable2[9].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if ((iteratorVariable15.name == FengGameManagerMKII.s[18]) && iteratorVariable3)
            {
                if ((iteratorVariable2[10].EndsWith(".jpg") || iteratorVariable2[10].EndsWith(".png")) || iteratorVariable2[10].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[10]))
                    {
                        WWW iteratorVariable34 = new WWW(iteratorVariable2[10]);
                        yield return iteratorVariable34;
                        Texture2D iteratorVariable35 = RCextensions.loadimage(iteratorVariable34, mipmap, 0x30d40);
                        iteratorVariable34.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[10]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable35;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[10], iteratorVariable15.material);
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[10]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[10]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[10]];
                    }
                }
                else if (iteratorVariable2[10].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[25]))
            {
                if ((iteratorVariable2[11].EndsWith(".jpg") || iteratorVariable2[11].EndsWith(".png")) || iteratorVariable2[11].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[11]))
                    {
                        WWW iteratorVariable36 = new WWW(iteratorVariable2[11]);
                        yield return iteratorVariable36;
                        Texture2D iteratorVariable37 = RCextensions.loadimage(iteratorVariable36, mipmap, 0x30d40);
                        iteratorVariable36.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[11]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable37;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[11], iteratorVariable15.material);
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[11]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[11]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material) FengGameManagerMKII.linkHash[0][iteratorVariable2[11]];
                    }
                }
                else if (iteratorVariable2[11].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
        }
        if (iteratorVariable4 && (horseId >= 0))
        {
            GameObject gameObject = PhotonView.Find(horseId).gameObject;
            if (gameObject != null)
            {
                foreach (Renderer iteratorVariable39 in gameObject.GetComponentsInChildren<Renderer>())
                {
                    if (iteratorVariable39.name.Contains(FengGameManagerMKII.s[19]))
                    {
                        if ((iteratorVariable2[0].EndsWith(".jpg") || iteratorVariable2[0].EndsWith(".png")) || iteratorVariable2[0].EndsWith(".jpeg"))
                        {
                            if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[0]))
                            {
                                WWW iteratorVariable40 = new WWW(iteratorVariable2[0]);
                                yield return iteratorVariable40;
                                Texture2D iteratorVariable41 = RCextensions.loadimage(iteratorVariable40, mipmap, 0x7a120);
                                iteratorVariable40.Dispose();
                                if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[0]))
                                {
                                    iteratorVariable1 = true;
                                    iteratorVariable39.material.mainTexture = iteratorVariable41;
                                    FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[0], iteratorVariable39.material);
                                    iteratorVariable39.material = (Material) FengGameManagerMKII.linkHash[1][iteratorVariable2[0]];
                                }
                                else
                                {
                                    iteratorVariable39.material = (Material) FengGameManagerMKII.linkHash[1][iteratorVariable2[0]];
                                }
                            }
                            else
                            {
                                iteratorVariable39.material = (Material) FengGameManagerMKII.linkHash[1][iteratorVariable2[0]];
                            }
                        }
                        else if (iteratorVariable2[0].ToLower() == "transparent")
                        {
                            iteratorVariable39.enabled = false;
                        }
                    }
                }
            }
        }
        if (iteratorVariable5 && ((iteratorVariable2[12].EndsWith(".jpg") || iteratorVariable2[12].EndsWith(".png")) || iteratorVariable2[12].EndsWith(".jpeg")))
        {
            if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[12]))
            {
                WWW iteratorVariable42 = new WWW(iteratorVariable2[12]);
                yield return iteratorVariable42;
                Texture2D iteratorVariable43 = RCextensions.loadimage(iteratorVariable42, mipmap, 0x30d40);
                iteratorVariable42.Dispose();
                if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[12]))
                {
                    iteratorVariable1 = true;
                    /*
                    leftbladetrail.MyMaterial.mainTexture = iteratorVariable43;
                    rightbladetrail.MyMaterial.mainTexture = iteratorVariable43;
                    FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[12], leftbladetrail.MyMaterial);
                    leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    leftbladetrail2.MyMaterial = leftbladetrail.MyMaterial;
                    rightbladetrail2.MyMaterial = leftbladetrail.MyMaterial;
                    */
                }
                else
                {
                    /*
                    leftbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    rightbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    */
                }
            }
            else
            {
                /*
                leftbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                rightbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                */
            }
        }
        if (iteratorVariable1)
        {
            FengGameManagerMKII.instance.unloadAssets();
        }
    }

}
