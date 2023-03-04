using InstantGamesBridge;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADInterstitial  : MonoBehaviour
{

    public void Start()
    {
        Bridge.advertisement.ShowInterstitial();
    }


}
