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

    public Type ItemType;
    public bool FunMode = false;
    public bool AnotherSpawn = false;
    public bool MouseDown;
    private AudioSource click;
    private void Start()
    {
        click = GetComponent<AudioSource>();

        MouseDown = AnotherSpawn == false;
        if (MouseDown)
            click.Play();

    }

    private void Update()
    {
        if (MouseDown)
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
        //если отдельный ингридиент столкнулся с подобным
        if (collision.transform.CompareTag("Food") && transform.parent == null && ItemType != Type.Plate && transform.tag != "Table")
        {
            if (collision.transform.GetComponent<InteractableObject>().ItemType == Type.Plate)
            {
                collision.transform.parent = null;
                transform.SetParent(collision.transform);                
            }
            else
            {
                transform.SetParent(collision.transform.root);                
            }
            
        }
        else if (collision.transform.CompareTag("Table") && FunMode)
        {
            if (ItemType != Type.Plate)
                GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        MouseDown = false;
        if (ItemType == Type.Plate)
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(ItemType == Type.Plate)
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        click.Play();
        MouseDown = true;
        if (transform.parent != null)
        {
            transform.DetachChildren();
            transform.parent = null;

        }
    }
}
