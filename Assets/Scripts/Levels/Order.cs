using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Order : MonoBehaviour
{
    public float orderTime = 60f;
    public List<InteractableObject.Type> Ingredients;
    public Action<Order> OnDestroyOrder;
    public Image avatar;

    [SerializeField] private GameObject imagePrefab;
    [SerializeField] private Image timeLineFront;


    private float timeLeft = 0f;

    private void Start()
    {
        timeLeft = orderTime;
        timeLineFront.fillAmount = 1f;


        Sprite[] SpritesAtlas;
        Dictionary<string, Sprite> spritesByName;
        SpritesAtlas = Resources.LoadAll<Sprite>("Sprites/IngridientsSprites");
        foreach (var item in Ingredients)
        {
            var imgIngr = Instantiate(imagePrefab, transform);
            spritesByName = SpritesAtlas.ToDictionary(s => s.name, s => s);
            imgIngr.transform.GetChild(0).GetComponent<Image>().sprite = spritesByName[item.ToString()];
        }
    }
    private void Update()
    {

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timeLineFront.fillAmount = (timeLeft / orderTime);
        }
        else
        {
            OnDestroyOrder?.Invoke(this);
        }
    }
}
