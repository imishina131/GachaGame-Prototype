using System;
using System.Collections.Generic;
using MatrixUtils.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BannerSelector : MonoBehaviour
{
    [SerializeField] List<BannerSelectorData> m_bannerSelectorData;
    [SerializeField, RequiredField] TMP_Text m_bannerName;
    [SerializeField, RequiredField] Image m_bannerBackground;
    [SerializeField, RequiredField] Image m_rollBackground;
    [SerializeField, RequiredField] Button m_pullButton;
    [SerializeField, RequiredField] Button m_detailsButton;
    [SerializeField, RequiredField] InterfaceReference<IRollHandler> m_rollHandler;
    [SerializeField, RequiredField] InterfaceReference<ICurrencyDisplay> m_currencyHandler;
    
    void Awake()
    {
        foreach (BannerSelectorData data in m_bannerSelectorData)
        {
            data.SelectionButton.onClick.AddListener(delegate { UpdateBannerData(data.BannerData); });
        }
    }
    void UpdateBannerData(BannerDataSO bannerData)
    {
        m_bannerName.text = bannerData.BannerName;
        m_bannerBackground.color = bannerData.BannerBackgroundColor;
        m_rollBackground.color = bannerData.BannerRollAreaColor;
        m_pullButton.image.color = bannerData.BannerRollButtonColor;
        m_detailsButton.image.color = bannerData.BannerDetailsButtonColor;
        m_currencyHandler.Value.UpdateDisplayedCurrency(bannerData.BannerCurrency);
        m_rollHandler.Value.UpdateBannerToRoll(bannerData.BannerName);
    }
    [Serializable]
    struct BannerSelectorData
    {
        public Button SelectionButton;
        public BannerDataSO BannerData;
    }
}
