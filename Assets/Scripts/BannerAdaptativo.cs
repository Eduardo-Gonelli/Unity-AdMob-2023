using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAdaptativo : MonoBehaviour
{
    BannerView bannerView;    
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        RequestBanner();
    }

    private void RequestBanner()
    {
        #if UNITY_EDITOR
        string adUnitId = "unused";
        #elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        #elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
        #else
        string adUnitId = "unexpected_platform";
        #endif

        // se o banner já existir, destrói ele
        if(bannerView != null)
        {
            bannerView.Destroy();
        }

        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        // Cria um bannerView e carrega o banner com a unidade de anúncios especificada.
        bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);
        AdRequest request = new AdRequest();
        ListenToAdEvents();
        bannerView.LoadAd(request);
    }

    private void ListenToAdEvents()
    {
        bannerView.OnBannerAdLoaded += HandleOnAdLoadedEvent;
        bannerView.OnBannerAdLoadFailed += HandleOnAdFailedToLoadEvent;
        bannerView.OnAdClicked += HandleOnAdClickedEvent;
        bannerView.OnAdFullScreenContentOpened += HandleOnAdFullScreenContentOpenedEvent;
        bannerView.OnAdFullScreenContentClosed += HandleOnAdFullScreenContentClosedEvent;
    }

    private void RemoveListenToAdEvents()
    {
        bannerView.OnBannerAdLoaded -= HandleOnAdLoadedEvent;
        bannerView.OnBannerAdLoadFailed -= HandleOnAdFailedToLoadEvent;
        bannerView.OnAdClicked -= HandleOnAdClickedEvent;
        bannerView.OnAdFullScreenContentOpened -= HandleOnAdFullScreenContentOpenedEvent;
        bannerView.OnAdFullScreenContentClosed -= HandleOnAdFullScreenContentClosedEvent;
    }

    private void HandleOnAdLoadedEvent()
    {
        Debug.Log("O banner finalizou o carregamento.");
        Debug.Log(string.Format("Altura do Ad: {0}. Largura: {1}", bannerView.GetHeightInPixels(), bannerView.GetWidthInPixels()));
    }

    private void HandleOnAdFailedToLoadEvent(LoadAdError error)
    {
        Debug.Log("O banner falhou ao carregar: " + error);
    }

    private void HandleOnAdClickedEvent()
    {
        Debug.Log("O banner foi clicado.");
    }

    private void HandleOnAdFullScreenContentOpenedEvent()
    {
        Debug.Log("O banner foi aberto.");
    }

    private void HandleOnAdFullScreenContentClosedEvent()
    {
        Debug.Log("O banner foi fechado.");
    }

    public void DestroyBannerView()
    {
        if (bannerView != null)
        {
            Debug.Log("Destruindo o bannerView.");
            RemoveListenToAdEvents();
            bannerView.Destroy();
            bannerView = null;
        }
    }
}
