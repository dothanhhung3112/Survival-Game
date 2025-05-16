using ACEPlay.Native;
using UnityEngine;

public class NativeAdsController : MonoBehaviour
{
    public static NativeAdsController Instance;
    [SerializeField] NativeAds NativeAdDefault;
    [SerializeField] NativeAds NativeAdsIngame;
    [SerializeField] NativeAds NativeAdsLose;
    public enum MiniGame
    {
        GameFight,Dalgona,GlassStepping,GreenRedLight,MarbleShoot,PrisionEscape,SixLegged,SquidGame,TugOfWar
    }
    public MiniGame miniGame;

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    public void DisplayNativeAdsDefault(bool enable)
    {
        NativeAdDefault.DisplayNativeAds(false);
    }

    public void DisplayNativeAdsLose(bool enable)
    {
        NativeAdsLose.DisplayNativeAds(false);
    }

    public void DisplayNativeAdsInGame(bool enable)
    {
        if(miniGame == MiniGame.Dalgona || miniGame == MiniGame.TugOfWar || miniGame == MiniGame.GreenRedLight) return;
        NativeAdsIngame.DisplayNativeAds(false);
    }
}
