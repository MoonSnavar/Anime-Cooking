using InstantGamesBridge;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OrderGenerator : MonoBehaviour
{    
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
        timeBetweenOrders = 15f - (levelCharacteristics.levelDifficult / 3);
        countOrders = 3 + (levelCharacteristics.levelDifficult / 2);
        maxIngridients = 6 + (levelCharacteristics.levelDifficult / 4);
        orderTime = 60 - levelCharacteristics.levelDifficult;

        recivedMoney = 0;
        moneyForOrder = 5 + (levelCharacteristics.levelDifficult / 2);
        moneyToCompleteLevel = (countOrders * moneyForOrder);

        storyBlock.OnStoryTextEnd += StartGenerator;
        timer.OnTimerEnd += CheckResults;

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

    private void StartGenerator()
    {
        timer.timerOn = true;
        StartCoroutine(SpawnOrders());
    }

    private void CheckOrder(InteractableObject interactableObject, Order order, Customer customer)
    {
        List<InteractableObject.Type> ingridients = new();
        ingridients.Add(interactableObject.itemType);
        for (int i = 0; i < interactableObject.transform.childCount; i++)
        {
            ingridients.Add(interactableObject.transform.GetChild(i).GetComponent<InteractableObject>().itemType);
            if (interactableObject.transform.GetChild(i).transform.childCount != 0)
            {
                for (int j = 0; j < interactableObject.transform.GetChild(i).transform.childCount; j++)
                {
                    ingridients.Add(interactableObject.transform.GetChild(i).transform.GetChild(j).GetComponent<InteractableObject>().itemType);
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
            yield return new WaitUntil(() => panelOrder.transform.childCount < 4 && countOrders > 0 && panelOrder.transform.childCount < countOrders);
            if (panelOrder.transform.childCount < 4 && countOrders > 0 && panelOrder.transform.childCount < countOrders)
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
