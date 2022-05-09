using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Events.Args;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using UnityEngine;

namespace Assets.Scripts.Achievements
{
    //TODO: Should be moved to the Services folder, as it behaves the same as a service does.
    /// <summary>
    /// A service which manages Achievements.
    /// </summary>
    public class AchievementManager : MonoBehaviour
    {
        private readonly IPlayerService playerService = Service.Player;

        private void Start()
        {
            playerService.OnTitanDamaged += OnTitanDamage;
        }

        private void OnDestroy()
        {
            playerService.OnTitanDamaged -= OnTitanDamage;
        }

        /// <summary>
        /// This is for damage related achievements.
        /// </summary>
        /// <param name="ev"></param>
        public void OnTitanDamage(TitanDamagedEvent ev)
        {
            switch (ev.Titan.Type)
            {
                case TitanType.Female:
                case TitanType.Armored:
                case TitanType.Beast:
                case TitanType.Colossal:
                case TitanType.Eren:
                case TitanType.Mindless:
                    if (ev.Damage >= 1000)
                    {
                        UnlockAchievement(Achievement.DMG_1K);
                    }
                    else return;

                    if (ev.Damage >= 2000)
                    {
                        UnlockAchievement(Achievement.DMG_2K);
                    }
                    else return;

                    break;
                default:
                    break;
            }
        }

        public static void UnlockAchievement(Achievement achievement)
        {
            switch (achievement)
            {
                case Achievement.DMG_2K:
                case Achievement.DMG_1K:
                case Achievement.SPD_280:
                case Achievement.KILL_3_TITANS:
                case Achievement.COLOSSAL_5_HITS:
                    // Achievement unlocked logic here
                    Debug.Log($"Unlocked {achievement}");
                    break;
                default:
                    break;
            }
        }
    }
}
