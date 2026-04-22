namespace GachaGame.Azure.Core.DataTypes;
/// <summary>
/// Represents the context of a roll and the data that is relevant to it
/// </summary>
/// <param name="playerData">The <see cref="PlayerData"/> that may be mutated by the roll</param>
/// <param name="banner">The <see cref="Banner"/> the player is attempting to roll</param>
public class RollContext(PlayerData playerData, Banner banner, string bannerName)
{
    public string BannerName { get; private set; } = bannerName;
    /// <summary>
    /// The <see cref="PlayerData"/> that may be mutated by the roll
    /// </summary>
    public PlayerData PlayerData { get; private set; } = playerData;
    /// <summary>
    /// The <see cref="Banner"/> the player is attempting to roll
    /// </summary>
    public Banner Banner { get; private set; } = banner;
    readonly List<Action<PlayerData>> m_rollDataMutations = [];
    /// <summary>
    /// The mutations to apply to the <see cref="PlayerData"/>
    /// </summary>
    public IReadOnlyList<Action<PlayerData>> RollDataMutations => m_rollDataMutations;
    /// <summary>
    /// Adds a mutation to the <see cref="RollDataMutations"/>
    /// </summary>
    /// <param name="playerDataMutation">The mutation to add</param>
    /// <returns>A <see cref="bool"/> indicating whether the mutation was added successfully</returns>
    public bool TryAddMutation(Action<PlayerData> playerDataMutation)
    {
        m_rollDataMutations.Add(playerDataMutation);
        return true;
    }
    /// <summary>
    /// Removes a mutation from the <see cref="RollDataMutations"/>
    /// </summary>
    /// <param name="playerDataMutation">The mutation to remove</param>
    /// <returns>A <see cref="bool"/> indicating whether the mutation was removed successfully</returns>
    public bool TryRemoveMutation(Action<PlayerData> playerDataMutation)
    {
        return m_rollDataMutations.Remove(playerDataMutation);
    }
    /// <summary>
    /// Applies all mutations from <see cref="RollDataMutations"/> to the <see cref="PlayerData"/>
    /// </summary>
    public void ApplyPlayerDataMutations()
    {
        logger.LogInformation("Before Apply: " + rollContext.PlayerData.BannerData[rollContext.BannerName].CurrentPity);
        foreach (Action<PlayerData> mutation in m_rollDataMutations) mutation(PlayerData);
        m_rollDataMutations.Clear();
        logger.LogInformation("After Apply: " + rollContext.PlayerData.BannerData[rollContext.BannerName].CurrentPity);
    }
}
