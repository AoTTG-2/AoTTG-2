namespace Assets.Scripts.Services.Interface
{
    public interface IDiscordService
    {
        Discord.Discord GetSocket();
        void JoinViaDiscord(string roomID);
        void UpdateDiscordActivity(global::Room room);
        void CloseSocket();
    }
}