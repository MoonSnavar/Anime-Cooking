#if UNITY_WEBGL
#if !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
#endif

namespace InstantGamesBridge.Modules.Platform
{
    public class PlatformModule
    {
#if !UNITY_EDITOR
        public string id { get; } = InstantGamesBridgeGetPlatformId();

        public string language { get; } = InstantGamesBridgeGetPlatformLanguage();

        public string payload { get; } = InstantGamesBridgeGetPlatformPayload();

        [DllImport("__Internal")]
        private static extern string InstantGamesBridgeGetPlatformId();

        [DllImport("__Internal")]
        private static extern string InstantGamesBridgeGetPlatformLanguage();

        [DllImport("__Internal")]
        private static extern string InstantGamesBridgeGetPlatformPayload();
        
        [DllImport("__Internal")]
        private static extern void InstantGamesBridgeSendMessageToPlatform(string message);
#else
        public string id { get; } = "mock";

        public string language { get; } = "en";

        public string payload { get; } = null;
#endif

        public void SendMessage(PlatformMessage message)
        {
#if !UNITY_EDITOR
            var messageString = "";

            switch (message)
            {
                case PlatformMessage.GameLoadingStarted:
                    messageString = "game_loading_started";
                    break;

                case PlatformMessage.GameLoadingStopped:
                    messageString = "game_loading_stopped";
                    break;

                case PlatformMessage.GameplayStarted:
                    messageString = "gameplay_started";
                    break;

                case PlatformMessage.GameplayStopped:
                    messageString = "gameplay_stopped";
                    break;

                case PlatformMessage.PlayerGotAchievement:
                    messageString = "player_got_achievement";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(message), message, null);
            }

            InstantGamesBridgeSendMessageToPlatform(messageString);
#endif

        }
    }
}
#endif