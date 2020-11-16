namespace Assets.Scripts.Services.Interface
{
    public interface IDiscordService
    {
        Discord.Discord GetSocket();
        void JoinViaDiscord(string roomID);
        void UpdateSinglePlayerActivity(global::Room room);
        void UpdateMultiPlayerActivity(global::Room room);
        void CloseSocket();
    }
}