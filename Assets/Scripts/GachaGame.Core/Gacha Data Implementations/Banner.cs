using System.Collections.Generic;
using MatrixUtils.Attributes;
using UnityEngine;
/// <summary>
/// Represents a singular banner in the gacha system
/// </summary>
public class Banner : MonoBehaviour
{
    
    public List<TierRoll> Tiers;
    [SerializeReference, ClassSelector] IRollResolver<TierRoll> m_tierResolver;
}