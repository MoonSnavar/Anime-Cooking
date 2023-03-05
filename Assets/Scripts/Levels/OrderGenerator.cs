using InstantGamesBridge;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class OrderGenerator : MonoBehaviour
{
    [SerializeField] private int maxCustomers = 4;
    [SerializeField] private TMP_Text possibilityText;
    [SerializeField] private GameObject[] ingridientsPrefabs;
    [SerializeField] private Transform hambagaSpawnPoint;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private GameObject EndMenu;
    [SerializeField] private TMP_Text endMenuMoneyText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private Timer timer;
    [SerializeField] private StoryBlock storyBlock;
    [SerializeField] private LevelCharacteristics levelCharacteristics;
    [SerializeField] private GameObject[] prefabCustomers;
    [SerializeField] private GameObject prefabOrder;
    [SerializeField] private GameObject panelOrder;
    private List<Order> spawnedOrders = new();
    private List<int> spawnedCustomers = new();
    private float timeBetweenOrders;
    private int countOrders;
    private int maxIngridients;
    private float orderTime;
    private int recivedMoney;
    private int moneyForOrder;
    private int moneyToCompleteLevel;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetStats();

        storyBlock.OnStoryTextEnd += StartGenerator;
        timer.OnTimerEnd += CheckResults;

        UpdateText();

        foreach (var item in stars)
        {
            item.SetActive(false);
        }

        Bridge.advertisement.rewardedStateChanged += state =>
        {
            if (state == InstantGamesBridge.Modules.Advertisement.RewardedState.Rewarded)
            {
                MultipleMoney();
            }
        };
    }

    private void SetStats()
    {
        if (levelCharacteristics.levelDifficult > 0)
        {
            timeBetweenOrders = 15f - (levelCharacteristics.levelDifficult / 3);
            countOrders = 3 + (levelCharacteristics.levelDifficult / 2);
            maxIngridients = 6 + (levelCharacteristics.levelDifficult / 4);
            orderTime = 60 - levelCharacteristics.levelDifficult;

            recivedMoney = 0;
            moneyForOrder = 5 + (levelCharacteristics.levelDifficult / 2);
            moneyToCompleteLevel = (countOrders * moneyForOrder);
        }
        else
        {
            timeBetweenOrders = 15f;
            countOrders = 5;
            maxIngridients = 20;
            orderTime = 60f;

            recivedMoney = 0;
            moneyForOrder = 50;
            moneyToCompleteLevel = (countOrders * moneyForOrder);
        }
    }

    private void StartGenerator()
    {
        timer.timerOn = true;
        StartCoroutine(SpawnOrders());
    }

    private void CheckOrder(InteractableObject interactableObject, Order order, Customer customer)
    {
        List<InteractableObject.Type> ingridients = new();
        ingridients.Add(interactableObject.ItemType);
        for (int i = 0; i < interactableObject.transform.childCount; i++)
        {
            ingridients.Add(interactableObject.transform.GetChild(i).GetComponent<InteractableObject>().ItemType);
            if (interactableObject.transform.GetChild(i).transform.childCount != 0)
            {
                for (int j = 0; j < interactableObject.transform.GetChild(i).transform.childCount; j++)
                {
                    ingridients.Add(interactableObject.transform.GetChild(i).transform.GetChild(j).GetComponent<InteractableObject>().ItemType);
                }
            }
        }

        //проверить ингридиенты с заказом
        var countIngridientsINOrder = order.Ingredients.Count;
        var coincidences = 0; 
        foreach (var itemNeed in order.Ingredients)
        {
            foreach (var itemHave in ingridients)
            {
                if (itemNeed == itemHave)
                {
                    coincidences++;
                    ingridients.Remove(itemHave);
                    break;
                }
            }
        }

        if (coincidences == countIngridientsINOrder && ingridients.Count == 1)
        {
            audioSource.Play();
            print("ЗАКАЗ ПРАВИЛЬНЫЙ");
            customer.GiveOrder -= CheckOrder;
            customer.OnDestroyCustomer -= DestroyCustomerAndOrder;

            recivedMoney += moneyForOrder;
            moneyText.text = recivedMoney.ToString();

            spawnedCustomers.Remove(customer.ID);
            Destroy(interactableObject.transform.gameObject);
            Destroy(customer.gameObject);
            Destroy(order.gameObject);

            StopCoroutine(SpawnOrders());
            StartCoroutine(SpawnOrders());

            countOrders--;
            if (countOrders <= 0)
            {
                //ЗАКАЗЫ КОНЧИЛИСЬ ----------------------------------------------------------------------------------------------------------------------------
                CheckResults();
            }
        }
        else
            print("ЗАКАЗ НЕ ПРАВИЛЬНЫЙ");
    }

    private void CheckResults()
    {
        Time.timeScale = 0f;
        endMenuMoneyText.text = recivedMoney.ToString();
        int lenght = StarCalculate();
        for (int i = 0; i < lenght; i++)
        {
            stars[i].SetActive(true);
        }


        if (PlayerPrefs.GetInt(levelCharacteristics.levelDifficult + "Level", 0) < lenght)
            PlayerPrefs.SetInt(levelCharacteristics.levelDifficult + "Level", lenght);

        EndMenu.SetActive(true);

        if (lenght > 0 && !PlayerPrefs.HasKey((levelCharacteristics.levelDifficult + 1).ToString() + "LevelUnblock"))
            PlayerPrefs.SetInt((levelCharacteristics.levelDifficult + 1).ToString() + "LevelUnblock", 1);
    }
        
    private int StarCalculate()
    {
        int stars = 0;
        float oneStartPercent = 60f;
        float twoStartPercent = 80f;
        float threeStartPercent = 100f;

        float receivedPercent = ((float)recivedMoney / (float)moneyToCompleteLevel) * 100f;
        print("receivedPercent = " + receivedPercent);
        if (receivedPercent > oneStartPercent)
        {
            if (receivedPercent > twoStartPercent)
            {
                if (receivedPercent == threeStartPercent)
                {
                    stars = 3;
                }
                else
                    stars = 2;
            }
            else
                stars = 1;
        }
        return stars;
    }

    public void SaveMoney()
    {
        if (PlayerPrefs.GetInt(levelCharacteristics.levelDifficult + "Level", 0) != 0)
        {
            int currentMoney = PlayerPrefs.GetInt("Money");
            PlayerPrefs.SetInt("Money", currentMoney + recivedMoney);
        }
    }

    public void ShowADVideo()
    {
        Bridge.advertisement.ShowRewarded();
    }

    public void MultipleMoney()
    {
        recivedMoney *= 2;
        endMenuMoneyText.text = recivedMoney.ToString();
    }

    public void DisableObject(GameObject obj)
    {
        obj.SetActive(false);
    }
    private IEnumerator SpawnOrders()
    {
        while (true)
        {
            yield return new WaitUntil(() => panelOrder.transform.childCount < maxCustomers && countOrders > 0 && panelOrder.transform.childCount < countOrders);
            if (panelOrder.transform.childCount < maxCustomers && countOrders > 0 && panelOrder.transform.childCount < countOrders)
            {
                var order = Instantiate(prefabOrder, panelOrder.transform).GetComponent<Order>();
                //количество, мин 3, первая и последняя булка не рандомятся
                var length = Random.Range(3, maxIngridients);
                for (int i = 1; i <= length; i++)
                {
                    if (i == 1)
                        order.Ingredients.Add(InteractableObject.Type.BunBot);
                    else if (i == length)
                        order.Ingredients.Add(InteractableObject.Type.BunTop);
                    else
                    {
                        order.Ingredients.Add(GetTypeByInt(Random.Range(3, 7)));
                    }
                }
                order.orderTime = orderTime;
                //блок с кастомерами
                List<Transform> emptySpawnPoints = new();
                foreach (var item in spawnPoints)
                {
                    if (item.childCount == 0)
                        emptySpawnPoints.Add(item);
                }
                var spawnPoint = emptySpawnPoints[Random.Range(0, emptySpawnPoints.Count)];
                //проверка на наличие кастомера на сцене
                bool needToAdd = true;
                List<int> emptyCustomers = new();
                for (int i = 0; i < prefabCustomers.Length; i++)
                {
                    needToAdd = true;
                    for (int j = 0; j < spawnedCustomers.Count; j++)
                    {
                        if (i == spawnedCustomers[j])
                        {
                            needToAdd = false;                            
                            break;
                        }
                    }
                    if (needToAdd)
                        emptyCustomers.Add(i);
                }

                int customerID = Random.Range(0, emptyCustomers.Count);

                spawnedCustomers.Add(emptyCustomers[customerID]); 

                var customer = Instantiate(prefabCustomers[emptyCustomers[customerID]], spawnPoint.position, transform.rotation, spawnPoint).GetComponent<Customer>();
                customer.order = order;
                customer.GiveOrder += CheckOrder;
                customer.OnDestroyCustomer += DestroyCustomerAndOrder;
                customer.ID = emptyCustomers[customerID];

                order.avatar.sprite = Resources.Load<Sprite>(customer.pathToAvatar);

                spawnedOrders.Add(order);

                if (countOrders > 0)
                    yield return new WaitForSeconds(timeBetweenOrders);
                else
                    break;
            }
        }
        yield return null;
    }

    private InteractableObject.Type GetTypeByInt(int i)
    {
        switch (i)
        {
            case 2:
                return InteractableObject.Type.Cheese;
            case 3:
                return InteractableObject.Type.Cutlet;
            case 4:
                return InteractableObject.Type.Egg;
            case 5:
                return InteractableObject.Type.Onion;
            case 6:
                return InteractableObject.Type.Salad;
            case 7:
                return InteractableObject.Type.Tomate;
            default:
                return InteractableObject.Type.Cheese;
        }
    }

    public void SpawnFullOrder()
    {        
        if (panelOrder.transform.childCount > 0 && PlayerPrefs.GetInt("Possibility") > 0)
        {
            float offset = 0.6f;
            Instantiate(GetIngredientPrefabByType(InteractableObject.Type.Plate), hambagaSpawnPoint.transform.position, hambagaSpawnPoint.transform.rotation)
                .GetComponent<InteractableObject>().AnotherSpawn = true; 

            var orderIngridents = panelOrder.transform.GetChild(0).GetComponent<Order>().Ingredients;
            foreach (var item in orderIngridents)
            {
                Instantiate(GetIngredientPrefabByType(item), new Vector3(hambagaSpawnPoint.transform.position.x, hambagaSpawnPoint.transform.position.y + offset), hambagaSpawnPoint.transform.rotation)
                    .GetComponent<InteractableObject>().AnotherSpawn = true;
                offset += 0.6f;                
            }

            PlayerPrefs.SetInt("Possibility", PlayerPrefs.GetInt("Possibility") - 1);
            UpdateText();
        }
    }

    private void UpdateText()
    {
        possibilityText.text = PlayerPrefs.GetInt("Possibility", 0).ToString();
    }

    private GameObject GetIngredientPrefabByType(InteractableObject.Type type)
    {
        foreach (var item in ingridientsPrefabs)
        {
            if (item.GetComponent<InteractableObject>().ItemType == type)
                return item;
        }
        return null;
    }

    private void DestroyCustomerAndOrder(Order order, Customer customer)
    {
        spawnedCustomers.Remove(customer.ID);
        Destroy(customer.gameObject);
        Destroy(order.gameObject);
    }

    private void OnDestroy()
    {
        storyBlock.OnStoryTextEnd -= StartGenerator;
        timer.OnTimerEnd -= CheckResults;
    }
}
