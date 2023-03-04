using InstantGamesBridge;
using TMPro;
using UnityEngine;

public class OnOffMusicWithGameVisible : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    void Start()
    {
        Bridge.game.visibilityStateChanged += state => 
        { 
            Debug.Log(state);
            if (state == InstantGamesBridge.Modules.Game.VisibilityState.Hidden)
            {
                music.Pause();
            }
            else
            {
                music.UnPause();
            }
        };
    }

    //private void OnDestroy()
    //{
    //    Bridge.game.visibilityStateChanged -= state => { };
    //}
}

