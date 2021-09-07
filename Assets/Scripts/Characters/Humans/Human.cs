namespace Assets.Scripts.Characters.Humans
{
    /// <summary>
    /// The abstract class for humans. <see cref="Hero"/> only contains logic for player controlled humans, yet logic should be abstracted and placed here, as Humans can also be AI controlled in the future.
    /// </summary>
    public abstract class Human : Entity
    {
        public HumanBody Body;
    }
}
