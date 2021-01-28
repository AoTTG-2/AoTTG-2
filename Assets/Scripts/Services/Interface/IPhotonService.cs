namespace Assets.Scripts.Services.Interface
{
    public interface IPhotonService
    {
        void Initialize();

        void UpdateConnectionType(bool isLocal);

        void OnDisconnectFromPhoton();

        void ChangeRegionDisconnect();
    }
}