using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public Action<InteractableObject,Order, Customer> GiveOrder;
    public Action<Order, Customer> OnDestroyCustomer;
    public Order order;
    public int ID;
    public string pathToAvatar;
    private void Start()
    {
        order.OnDestroyOrder += TransferOrder;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food") && collision.GetComponent<InteractableObject>().ItemType == InteractableObject.Type.Plate)
            GiveOrder?.Invoke(collision.GetComponent<InteractableObject>(), order, this);
    }
    private void TransferOrder(Order order)
    {
        OnDestroyCustomer?.Invoke(order, this);
    }
}
