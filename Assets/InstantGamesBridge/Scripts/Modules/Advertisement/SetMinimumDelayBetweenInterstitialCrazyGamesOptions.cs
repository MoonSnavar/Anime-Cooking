using System;

namespace InstantGamesBridge.Modules.Advertisement
{
    [Serializable]
    public class SetMinimumDelayBetweenInterstitialCrazyGamesOptions : SetMinimumDelayBetweenInterstitialPlatformDependedOptions
    {
        public SetMinimumDelayBetweenInterstitialCrazyGamesOptions(int seconds) : base(seconds)
        {
            _targetPlatform = OptionsTargetPlatform.CrazyGames;
        }
    }
}