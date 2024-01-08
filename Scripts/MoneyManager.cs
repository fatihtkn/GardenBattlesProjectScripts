using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoSingleton<MoneyManager>
{
    public Transform currencyIconTransform;
    public List<Money> moneys;
    public Button buyButton;
    
    //public Button buySkillButton;
    public List<NewStack> stacksPrefab;
   
    public TMP_Text moneyText;
    [SerializeField]private float currentMoney;
    public int stackCost = 50;
    [SerializeField] int killBonus = 300;
   
    public GameObject snowFlakePrefab;
    private Action OnMoneyRanOutOf;
    private void Start()
    {
        GridManager.Instance.OnAllPlantsArranged += ActivateButtons;
        GameManager.Instance.StartingBattle += DisableButtons;
        buyButton.onClick.AddListener(() => { BuyStack(); });
        buyButton.onClick.AddListener(() => { FadeBuyButton(); });
        
        moneyText.SetText(currentMoney.ToString("0"));
        OnMoneyRanOutOf += DisableButtons;
    }

    public void StartMoneyInterpolation(Vector3 moneySpawnPos)
    {
        StartCoroutine(DropMoney(moneySpawnPos));
    }

    public IEnumerator DropMoney(Vector3 moneySpawnPos)
    {

        for (int i = 0; i < moneys.Count; i++)
        {
            Money money = moneys[i];
            money.gameObject.SetActive(true);
            money.CreateMoney(moneySpawnPos+new Vector3(0,1f,0));

            yield return new WaitForSeconds(0.2f);
        }
        SetMoney(killBonus);
    }

    private void SetActiveBuyButton()
    {
        buyButton.gameObject.SetActive(true);
    }
    private void BuyStack()
    {
        bool isBuyable= (currentMoney - stackCost) >= 0;
       
        if (currentMoney>0& isBuyable)
        {
            List<Tile> tiles = TileManager.Instance.GetEmptyTiles();
            int stackIndex=UnityEngine.Random.Range(0,stacksPrefab.Count);
            NewStack stack = Instantiate(stacksPrefab[stackIndex]);
            Tile randomTile = tiles[0];
            randomTile.SetTileStackOnBought(stack);
            
            
           
            SetMoney(-stackCost);
        }
        
        
    }
    private void SetMoney(int increaseAmount)
    {
        float refMoneyValue = currentMoney;
        currentMoney += increaseAmount;
        float targetAmount = currentMoney;
        DOTween.To(() => refMoneyValue, x => refMoneyValue = x, targetAmount, 0.4f).OnUpdate(() =>
        {
            moneyText.SetText(refMoneyValue.ToString("0"));
        });
        ControlTheMoney();
    }

    private void FadeBuyButton()
    {
        if(currentMoney<=0) 
        {
            Image buttonImage = buyButton.GetComponent<Image>();
            TMP_Text buttonText = buyButton.GetComponentInChildren<TMP_Text>();
            buttonImage.raycastTarget = false;
            buttonText.DOFade(0, 1f);
            buttonImage.DOFade(0, 1f);
        }
       
    }
    private void FadeImmediateBuyStackButton()
    {
       
         Image buttonImage = buyButton.GetComponent<Image>();
         TMP_Text buttonText = buyButton.GetComponentInChildren<TMP_Text>();
         buttonImage.raycastTarget = false;
         buttonText.DOFade(0, 1f);
         buttonImage.DOFade(0, 1f);
      

    }
    void BuySkillToRandomPlant()
    {
        bool isBuyable = (currentMoney - stackCost) >= 0;
        if(isBuyable)
        {
            
            NewStackManager stackManager = NewStackManager.Instance;
            List<NewStack> noskillstacks = new();
            foreach (NewStack stack in stackManager.stacks)
            {
                if (stack.Plant != null)
                {
                    if (!stack.Plant.HasSkill)
                    {
                        noskillstacks.Add(stack);
                    }
                }
                
            }
            NewStack randomStack = noskillstacks[UnityEngine.Random.Range(0, noskillstacks.Count)];
           
            if(randomStack != null)
            {
                randomStack.Plant.AddComponent<Freeze>();
                randomStack.Plant.InitializeSkill();

                GameObject snowFlake = Instantiate(snowFlakePrefab, randomStack.transform);
                snowFlake.transform.localPosition = new(0, randomStack.PlantDataController.Data.SnowflakeYOffset, 0);

                SetMoney(-stackCost);
            }
            
        }

    }
    void ActivateButtons()
    {
        SetActiveBuyButton();
       
    }

    
    void DisableButtons()
    {
      
        DisableBuyStackButton();
    }
   
    void DisableBuyStackButton()
    {
        FadeImmediateBuyStackButton();
    }

    void ControlTheMoney()
    {
        if (currentMoney <= 0)
        {
            OnMoneyRanOutOf?.Invoke();
        }

        
    }
}
