using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunModeSpawningridients : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] ingridientsPrefabs;
    [SerializeField] private Transform orderPanel;
    private Order lastOrder;
    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitUntil(() => orderPanel.transform.childCount > 0);
            //считывает заказ и киает ингридиенты по порядку нужные
            lastOrder = orderPanel.GetChild(0).GetComponent<Order>();
            foreach (var item in lastOrder.Ingredients)
            {
                yield return new WaitForSeconds(1f);
                if (lastOrder != orderPanel.GetChild(0).GetComponent<Order>()) //если заказ закончился или выполнился, а цикл старый спавнит то прерываем
                    break;

                var spawnPointID = Random.Range(0, spawnPoints.Length);
                var io = Instantiate(GetObjectByType(item), spawnPoints[spawnPointID].position, spawnPoints[spawnPointID].rotation).GetComponent<InteractableObject>();
                io.AnotherSpawn = true;
                io.FunMode = true;

            }

            yield return new WaitForSeconds(1f);
        }
    }

    private GameObject GetObjectByType(InteractableObject.Type type)
    {
        foreach (var item in ingridientsPrefabs)
        {
            if (item.GetComponent<InteractableObject>().ItemType == type)
                return item;
        }
        return null;
    }
}
