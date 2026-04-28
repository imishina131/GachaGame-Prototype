using System;
using UnityEngine;
/// <summary>
/// Represents the data sent to the remote playfab system in order to roll a character
/// </summary>
public struct BannerRollRequestData
{
    /// <summary>
    /// The ID of the banner to roll
    /// </summary>
    public string BannerID { get; }
    public BannerRollRequestData(string bannerID)
    {
        BannerID = bannerID;
    }
}
