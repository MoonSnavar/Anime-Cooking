using System;

namespace InstantGamesBridge.Modules.Advertisement
{
    [Serializable]
    public class ShowInterstitialCrazyGamesOptions : ShowInterstitialPlatformDependedOptions
    {
        public ShowInterstitialCrazyGamesOptions(bool ignoreDelay) : base(ignoreDelay)
        {
            _targetPlatform = OptionsTargetPlatform.CrazyGames;
        }
    }
}