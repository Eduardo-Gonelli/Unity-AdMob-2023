using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class IntersticialAd : MonoBehaviour
{
    private InterstitialAd interstitial;
    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        RequestInterstitial();
    }

    private void RequestInterstitial()
    {
        #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        #elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        #else
        string adUnitId = "unexpected_platform";
        #endif
        // destrói o intersticial se ele já existir
        if(interstitial != null)
        { 
            interstitial.Destroy(); 
            interstitial = null;
        }
        // Cria um intersticial e carrega o intersticial com a unidade de anúncios especificada.
        AdRequest request = new AdRequest();
        InterstitialAd.Load(adUnitId, request, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log("Falha ao carregar o intersticial: " + error);
                return;
            }
            Debug.Log("Intersticial carregado com sucesso." + ad.GetResponseInfo());
            interstitial = ad;            
        });

        ListenToAdEvents();        
    }

    public void ShowInterstitialAd()
    {
        if (interstitial != null && interstitial.CanShowAd())
        {
            Debug.Log("Exibindo o anúncio intersticial.");
            interstitial.Show();
        }
        else
        {
            Debug.LogError("O anúncio intersticial não está pronto ainda.");
        }
    }

    private void ListenToAdEvents()
    {
        interstitial.OnAdFullScreenContentOpened += HandleOnAdLoadedEvent;
        interstitial.OnAdFullScreenContentFailed += HandleOnAdFailedToLoadEvent;        
        interstitial.OnAdFullScreenContentClosed += HandleOnAdClosedEvent;
        interstitial.OnAdClicked += HandleOnAdClickedEvent;        
    }

    private void HandleOnAdLoadedEvent()
    {
        Debug.Log("O intersticial finalizou o carregamento.");
    }

    private void HandleOnAdFailedToLoadEvent(AdError error)
    {
        Debug.Log("O intersticial falhou ao abrir com o erro: " + error);
    }

    private void HandleOnAdClosedEvent()
    {
        Debug.Log("O intersticial foi fechado.");
        RequestInterstitial();
    }

    private void HandleOnAdClickedEvent()
    {
        Debug.Log("O intersticial foi clicado.");
    }
}
