/// <summary>
/// Represents a system that can roll a banner
/// </summary>
public interface IRollHandler
{
    /// <summary>
    /// Rolls the active banner
    /// </summary>
    void Roll();
    /// <summary>
    /// Updates the current banner to roll
    /// </summary>
    /// <param name="bannerID">The new banner to roll</param>
    void UpdateBannerToRoll(string bannerID);
}