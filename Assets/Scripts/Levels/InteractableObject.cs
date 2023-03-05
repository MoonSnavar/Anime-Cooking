using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum Type{
        Plate,
        BunBot,
        BunTop,
        Cheese,
        Cutlet,
        Egg,
        Onion,
        Salad,
        Tomate
    }

    public Type itemType;
    public bool mouseDown;
    private AudioSource click;
    private void Start()
    {
        mouseDown = true;
        click = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (mouseDown)
        {
            Vector2 cursor = Input.mousePosition;
            cursor = Camera.main.ScreenToWorldPoint(cursor);

            transform.position = cursor;
        }
        if (transform.position.y < -9f)
            Destroy(gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Food"))
        {
            if (collision.transform.GetComponent<InteractableObject>().itemType == Type.Plate)
            {
                collision.transform.parent = null;
                transform.SetParent(collision.transform);
                print("“¿–≈À ¿ Õ¿…ƒ≈Õ¿");
            }
            else
                transform.SetParent(collision.transform.root);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mouseDown = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        click.Play();
        mouseDown = true;
        if (transform.parent != null)
        {
            transform.DetachChildren();
            transform.parent = null;

        }
    }
}
