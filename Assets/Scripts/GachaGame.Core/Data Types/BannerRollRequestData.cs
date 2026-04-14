using System;
using UnityEngine;

public struct BannerRollRequestData
{
    public string BannerID { get; }
    public BannerRollRequestData(string bannerID)
    {
        BannerID = bannerID;
    }
}
