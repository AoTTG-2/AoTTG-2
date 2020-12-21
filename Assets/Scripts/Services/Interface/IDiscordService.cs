namespace Assets.Scripts.Services.Interface
{
    public interface IDiscordService
    {
        void JoinViaDiscord(string roomID);
        void UpdateDiscordActivity(global::Room room);
    }
}