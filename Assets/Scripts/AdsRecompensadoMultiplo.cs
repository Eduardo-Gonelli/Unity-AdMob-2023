using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsRecompensadoMultiplo : MonoBehaviour
{
    private RewardedAd gameOverRewardedAd;
    private RewardedAd extraCoinsRewardedAd;
    
    // Observação! Para configurar os anúncios premiados você deve ter um adUnitId para cada anúncio premiado.
    // Ver slides 124 à 131 do material disponibilizado no blackboard.

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
        gameOverRewardedAd = CreateAndLoadRewardedAd(adUnitId, gameOverRewardedAd);
        extraCoinsRewardedAd = CreateAndLoadRewardedAd(adUnitId, extraCoinsRewardedAd);
    }

    public RewardedAd CreateAndLoadRewardedAd(string adUnitId, RewardedAd rewardedAd)
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
            ListenToAdEvents(rewardedAd);
        });
        return rewardedAd;
    }

    public void ShowExtraCoinsRewardedAd()
    {
        const string rewardMsg =
        "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (extraCoinsRewardedAd != null && extraCoinsRewardedAd.CanShowAd())
        {
            extraCoinsRewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(System.String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }

    public void ShowGameOverRewardedAd()
    {
        const string rewardMsg =
        "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (gameOverRewardedAd != null && gameOverRewardedAd.CanShowAd())
        {
            gameOverRewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(System.String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }

    public void ListenToAdEvents(RewardedAd rewardedAd)
    {
        rewardedAd.OnAdClicked += HandleOnAdClicked;
        rewardedAd.OnAdFullScreenContentClosed += () => HandleOnAdFullScreenContentClosed(rewardedAd);
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

    public void HandleOnAdFullScreenContentClosed(RewardedAd rewardedAd)
    {
        Debug.Log("Ad fechado.");
        CreateAndLoadRewardedAd(adUnitId,rewardedAd);
    }

    public void HandleOnAdScreenContentFailed(AdError error)
    {
        Debug.Log("Falha ao exibir o ad.");        
    }
}
