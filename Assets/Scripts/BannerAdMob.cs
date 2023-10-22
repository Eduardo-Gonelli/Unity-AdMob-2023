using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAdMob : MonoBehaviour
{
    private BannerView bannerView;
    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        RequestBanner();
        Invoke("DestroyBannerView", 10f);
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#else
        string adUnitId = "unexpected_platform";
#endif
        // se o banner já existir, destrói ele
        if(bannerView != null)
        {
            bannerView.Destroy();
        }
        // Cria um bannerView e carrega o banner com a unidade de anúncios especificada.
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

        // cria um request vazio
        AdRequest request = new AdRequest();
        // associa os eventos do banner
        ListenToAdEvents();
        // carrega o banner com o request
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
