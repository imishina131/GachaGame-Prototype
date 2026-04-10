using System.Collections.Generic;
using MatrixUtils.Attributes;
using UnityEngine;

public class Banner : MonoBehaviour
{
    public List<TierRoll> Tiers;
    [SerializeReference, ClassSelector] IRollResolver<TierRoll> m_tierResolver;
}