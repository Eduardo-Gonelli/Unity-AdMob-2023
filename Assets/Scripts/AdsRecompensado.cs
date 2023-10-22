using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsRecompensado : MonoBehaviour
{
    private RewardedAd rewardedAd;

#if UNITY_ANDROID
    string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
    string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    string adUnitId = "unexpected_platform";
#endif

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        CreateAndLoadRewardedAd();
    }

    private void CreateAndLoadRewardedAd()
    {
        // testa se existe algum rewardedAd carregado e limpa a memoria
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        // Cria um rewardedAd e carrega o rewardedAd com a unidade de anúncios especificada.
        AdRequest request = new AdRequest();
        RewardedAd.Load(adUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log("Falha ao carregar o rewardedAd: " + error);
                return;
            }
            Debug.Log("RewardedAd carregado com sucesso." + ad.GetResponseInfo());
            rewardedAd = ad;
            ListenToAdEvents();
        });
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg ="Prêmios concedidos ao jogador. Tipo: {0}, Quantidade: {1}.";
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Lógica para premiar o jogador.
                // Exibe o prêmio recebido pelo jogador.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }

    public void ListenToAdEvents()
    {
        rewardedAd.OnAdClicked += HandleOnAdClicked;
        rewardedAd.OnAdFullScreenContentClosed += HandleOnAdFullScreenContentClosed;
        rewardedAd.OnAdFullScreenContentFailed += HandleOnAdScreenContentFailed;
        rewardedAd.OnAdFullScreenContentOpened += HandleOnAdFullScreenContentOpened;
    }

    // Event Handlers
    public void HandleOnAdClicked()
    {
        Debug.Log("O anúncio foi clicado.");
    }

    public void HandleOnAdFullScreenContentOpened()
    {
        Debug.Log("Ad aberto.");
    }

    public void HandleOnAdFullScreenContentClosed()
    {
        Debug.Log("Ad fechado.");
        CreateAndLoadRewardedAd();
    }

    public void HandleOnAdScreenContentFailed(AdError error)
    {
        Debug.Log("Falha ao exibir o ad.");
        CreateAndLoadRewardedAd();
    }
}
