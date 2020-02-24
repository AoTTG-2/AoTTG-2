using System;
using UnityEngine;

public class Language
{
    public static string[] abnormal = new string[0x19];
    public static string[] btn_back = new string[0x19];
    public static string[] btn_continue = new string[0x19];
    public static string[] btn_create_game = new string[0x19];
    public static string[] btn_credits = new string[0x19];
    public static string[] btn_default = new string[0x19];
    public static string[] btn_join = new string[0x19];
    public static string[] btn_LAN = new string[0x19];
    public static string[] btn_multiplayer = new string[0x19];
    public static string[] btn_option = new string[0x19];
    public static string[] btn_QUICK_MATCH = new string[0x19];
    public static string[] btn_quit = new string[0x19];
    public static string[] btn_ready = new string[0x19];
    public static string[] btn_refresh = new string[0x19];
    public static string[] btn_server_ASIA = new string[0x19];
    public static string[] btn_server_EU = new string[0x19];
    public static string[] btn_server_JAPAN = new string[0x19];
    public static string[] btn_server_US = new string[0x19];
    public static string[] btn_single = new string[0x19];
    public static string[] btn_start = new string[0x19];
    public static string[] camera_info = new string[0x19];
    public static string[] camera_original = new string[0x19];
    public static string[] camera_tilt = new string[0x19];
    public static string[] camera_tps = new string[0x19];
    public static string[] camera_type = new string[0x19];
    public static string[] camera_wow = new string[0x19];
    public static string[] change_quality = new string[0x19];
    public static string[] choose_character = new string[0x19];
    public static string[] choose_map = new string[0x19];
    public static string[] choose_region_server = new string[0x19];
    public static string[] difficulty = new string[0x19];
    public static string[] game_time = new string[0x19];
    public static string[] hard = new string[0x19];
    public static string[] invert_mouse = new string[0x19];
    public static string[] key_set_info_1 = new string[0x19];
    public static string[] key_set_info_2 = new string[0x19];
    public static string[] max_player = new string[0x19];
    public static string[] max_Time = new string[0x19];
    public static string[] mouse_sensitivity = new string[0x19];
    public static string[] normal = new string[0x19];
    public static string[] port = new string[0x19];
    public static string[] select_titan = new string[0x19];
    public static string[] server_ip = new string[0x19];
    public static string[] server_name = new string[0x19];
    public static string[] soldier = new string[0x19];
    public static string[] titan = new string[0x19];
    public static int type = -1;
    public static string[] waiting_for_input = new string[0x19];

    public static string GetLang(int id)
    {
        if (id != 0)
        {
            if (id == 1)
            {
                return "简体中文";
            }
            if (id == 2)
            {
                return "SPANISH";
            }
            if (id == 3)
            {
                return "POLSKI";
            }
            if (id == 4)
            {
                return "ITALIANO";
            }
            if (id == 5)
            {
                return "NORWEGIAN";
            }
            if (id == 6)
            {
                return "PORTUGUESE";
            }
            if (id == 7)
            {
                return "PORTUGUESE_BR";
            }
            if (id == 8)
            {
                return "繁體中文_台";
            }
            if (id == 9)
            {
                return "繁體中文_港";
            }
            if (id == 10)
            {
                return "SLOVAK";
            }
            if (id == 11)
            {
                return "GERMAN";
            }
            if (id == 12)
            {
                return "FRANCAIS";
            }
            if (id == 13)
            {
                return "T\x00dcRK\x00c7E";
            }
            if (id == 14)
            {
                return "ARABIC";
            }
            if (id == 15)
            {
                return "Thai";
            }
            if (id == 0x10)
            {
                return "Русский";
            }
            if (id == 0x11)
            {
                return "NEDERLANDS";
            }
            if (id == 0x12)
            {
                return "Hebrew";
            }
            if (id == 0x13)
            {
                return "DANSK";
            }
        }
        return "ENGLISH";
    }

    public static int GetLangIndex(string txt)
    {
        if (txt != "ENGLISH")
        {
            if (txt == "SPANISH")
            {
                return 2;
            }
            if (txt == "POLSKI")
            {
                return 3;
            }
            if (txt == "ITALIANO")
            {
                return 4;
            }
            if (txt == "NORWEGIAN")
            {
                return 5;
            }
            if (txt == "PORTUGUESE")
            {
                return 6;
            }
            if (txt == "PORTUGUESE_BR")
            {
                return 7;
            }
            if (txt == "SLOVAK")
            {
                return 10;
            }
            if (txt == "GERMAN")
            {
                return 11;
            }
            if (txt == "FRANCAIS")
            {
                return 12;
            }
            if (txt == "T\x00dcRK\x00c7E")
            {
                return 13;
            }
            if (txt == "ARABIC")
            {
                return 14;
            }
            if (txt == "Thai")
            {
                return 15;
            }
            if (txt == "Русский")
            {
                return 0x10;
            }
            if (txt == "NEDERLANDS")
            {
                return 0x11;
            }
            if (txt == "Hebrew")
            {
                return 0x12;
            }
            if (txt == "DANSK")
            {
                return 0x13;
            }
            if (txt == "简体中文")
            {
                return 1;
            }
            if (txt == "繁體中文_台")
            {
                return 8;
            }
            if (txt == "繁體中文_港")
            {
                return 9;
            }
        }
        return 0;
    }

    public static void init()
    {
        char[] separator = new char[] { "\n"[0] };
        string[] strArray = ((TextAsset) Resources.Load("lang")).text.Split(separator);
        string txt = string.Empty;
        int index = 0;
        string str3 = string.Empty;
        string str4 = string.Empty;
        for (int i = 0; i < strArray.Length; i++)
        {
            string str5 = strArray[i];
            if (!str5.Contains("//"))
            {
                if (str5.Contains("#START"))
                {
                    char[] chArray2 = new char[] { "@"[0] };
                    txt = str5.Split(chArray2)[1];
                    index = GetLangIndex(txt);
                }
                else if (str5.Contains("#END"))
                {
                    txt = string.Empty;
                }
                else if ((txt != string.Empty) && str5.Contains("@"))
                {
                    char[] chArray3 = new char[] { "@"[0] };
                    str3 = str5.Split(chArray3)[0];
                    char[] chArray4 = new char[] { "@"[0] };
                    str4 = str5.Split(chArray4)[1];
                    switch (str3)
                    {
                        case "btn_single":
                            btn_single[index] = str4;
                            break;

                        case "btn_multiplayer":
                            btn_multiplayer[index] = str4;
                            break;

                        case "btn_option":
                            btn_option[index] = str4;
                            break;

                        case "btn_credits":
                            btn_credits[index] = str4;
                            break;

                        case "btn_back":
                            btn_back[index] = str4;
                            break;

                        case "btn_refresh":
                            btn_refresh[index] = str4;
                            break;

                        case "btn_join":
                            btn_join[index] = str4;
                            break;

                        case "btn_start":
                            btn_start[index] = str4;
                            break;

                        case "btn_create_game":
                            btn_create_game[index] = str4;
                            break;

                        case "btn_LAN":
                            btn_LAN[index] = str4;
                            break;

                        case "btn_server_US":
                            btn_server_US[index] = str4;
                            break;

                        case "btn_server_EU":
                            btn_server_EU[index] = str4;
                            break;

                        case "btn_server_ASIA":
                            btn_server_ASIA[index] = str4;
                            break;

                        case "btn_server_JAPAN":
                            btn_server_JAPAN[index] = str4;
                            break;

                        case "btn_QUICK_MATCH":
                            btn_QUICK_MATCH[index] = str4;
                            break;

                        case "btn_default":
                            btn_default[index] = str4;
                            break;

                        case "btn_ready":
                            btn_ready[index] = str4;
                            break;

                        case "server_name":
                            server_name[index] = str4;
                            break;

                        case "server_ip":
                            server_ip[index] = str4;
                            break;

                        case "port":
                            port[index] = str4;
                            break;

                        case "choose_map":
                            choose_map[index] = str4;
                            break;

                        case "choose_character":
                            choose_character[index] = str4;
                            break;

                        case "camera_type":
                            camera_type[index] = str4;
                            break;

                        case "camera_original":
                            camera_original[index] = str4;
                            break;

                        case "camera_wow":
                            camera_wow[index] = str4;
                            break;

                        case "camera_tps":
                            camera_tps[index] = str4;
                            break;

                        case "max_player":
                            max_player[index] = str4;
                            break;

                        case "max_Time":
                            max_Time[index] = str4;
                            break;

                        case "game_time":
                            game_time[index] = str4;
                            break;

                        case "difficulty":
                            difficulty[index] = str4;
                            break;

                        case "normal":
                            normal[index] = str4;
                            break;

                        case "hard":
                            hard[index] = str4;
                            break;

                        case "abnormal":
                            abnormal[index] = str4;
                            break;

                        case "mouse_sensitivity":
                            mouse_sensitivity[index] = str4;
                            break;

                        case "change_quality":
                            change_quality[index] = str4;
                            break;

                        case "camera_tilt":
                            camera_tilt[index] = str4;
                            break;

                        case "invert_mouse":
                            invert_mouse[index] = str4;
                            break;

                        case "waiting_for_input":
                            waiting_for_input[index] = str4;
                            break;

                        case "key_set_info_1":
                            key_set_info_1[index] = str4;
                            break;

                        case "key_set_info_2":
                            key_set_info_2[index] = str4;
                            break;

                        case "soldier":
                            soldier[index] = str4;
                            break;

                        case "titan":
                            titan[index] = str4;
                            break;

                        case "select_titan":
                            select_titan[index] = str4;
                            break;

                        case "camera_info":
                            camera_info[index] = str4;
                            break;

                        case "btn_continue":
                            btn_continue[index] = str4;
                            break;

                        case "btn_quit":
                            btn_quit[index] = str4;
                            break;

                        case "choose_region_server":
                            choose_region_server[index] = str4;
                            break;
                    }
                }
            }
        }
    }
}

