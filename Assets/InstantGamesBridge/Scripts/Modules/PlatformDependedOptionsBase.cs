using System;
using InstantGamesBridge.Common;
using UnityEngine;

namespace InstantGamesBridge.Modules
{
    [Serializable]
    public abstract class PlatformDependedOptionsBase
    {
        protected OptionsTargetPlatform _targetPlatform;

        public OptionsTargetPlatform GetTargetPlatform()
        {
            return _targetPlatform;
        }

        public string ToJson()
        {
            var platform = "";

            switch (_targetPlatform)
            {
                case OptionsTargetPlatform.VK:
                    platform = "vk";
                    break;

                case OptionsTargetPlatform.Yandex:
                    platform = "yandex";
                    break;

                case OptionsTargetPlatform.CrazyGames:
                    platform = "crazy_games";
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return JsonUtility.ToJson(this).SurroundWithKey(platform);
        }
    }
}