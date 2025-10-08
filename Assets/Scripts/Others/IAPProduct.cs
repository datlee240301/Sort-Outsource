using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class MyPurchaseID
{
    public const string RemoveAds = "com.diamondsort.removeads";
    public const string Pack1 = "com.diamondsort.pack1";
    public const string Pack2 = "com.diamondsort.pack2";
    public const string Pack3 = "com.diamondsort.pack3";
    public const string Pack4 = "com.diamondsort.pack4";
    public const string Pack5 = "com.diamondsort.pack5";
    public const string Pack6 = "com.diamondsort.pack6";
    public const string Pack7 = "com.diamondsort.pack7";
    public const string Pack8 = "com.diamondsort.pack8";
    public const string Pack9 = "com.diamondsort.pack9";
}

public class IAPProduct : MonoBehaviour
{
    [SerializeField] private string _purchaseID;
    [SerializeField] private Button _purchaseButton;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _discount;
    [SerializeField] private Sprite _icon;

    public string PurchaseID => _purchaseID;

    public delegate void PurchaseEvent(Product Model, Action OnComplete);

    public event PurchaseEvent OnPurchase;
    private Product _model;

    private void Start()
    {
        RegisterPurchase();
        RegisterEventButton();
    }

    protected virtual void RegisterPurchase()
    {
        StartCoroutine(IAPManager.Instance.CreateHandleProduct(this));
    }

    public void Setup(Product product, string code, string price)
    {
        _model = product;
        if (_price != null)
        {
            _price.text = price + " " + code;
        }

        if (_discount != null)
        {
            if (code.Equals("VND"))
            {
                var round = Mathf.Round(float.Parse(price) + float.Parse(price) * .4f);
                _discount.text = code + " " + round;
            }
            else
            {
                var priceFormat = $"{float.Parse(price) + float.Parse(price) * .4f:0.00}";
                _discount.text = code + " " + priceFormat;
            }
        }
    }

    private void RegisterEventButton()
    {
        _purchaseButton.onClick.AddListener(() =>
        {
            AudioManager.PlaySound("Click");
            Purchase();
        });
    }

    private void Purchase()
    {
        OnPurchase?.Invoke(_model, HandlePurchaseComplete);
    }

    private void HandlePurchaseComplete()
    {
        switch (_purchaseID)
        {
            case MyPurchaseID.RemoveAds:
                RemoveAdsPack();
                break;
            case MyPurchaseID.Pack1:
                AddCoinsPack(300);
                break;
            case MyPurchaseID.Pack2:
                AddCoinsPack(500);
                break;
            case MyPurchaseID.Pack3:
                AddCoinsPack(800);
                break;
            case MyPurchaseID.Pack4:
                AddCoinsPack(1000);
                break;
            case MyPurchaseID.Pack5:
                AddCoinsPack(1500);
                break;
            case MyPurchaseID.Pack6:
                AddCoinsPack(1800);
                break;
            case MyPurchaseID.Pack7:
                AddCoinsPack(2200);
                break;
            case MyPurchaseID.Pack8:
                AddCoinsPack(2500);
                break;
            case MyPurchaseID.Pack9:
                AddCoinsPack(3000);
                break;
        }

        if (_icon != null)
        {
            _purchaseButton.gameObject.GetComponent<Image>().sprite = _icon;
            _purchaseButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            _purchaseButton.interactable = false;
        }
    }
    
    private void RemoveAdsPack()
    {
        ResourceManager.RemoveAds = true;
        GameEventManager.PurchaseAds?.Invoke();
    }
    
    private void AddCoinsPack(int amount)
    {
        ResourceManager.Coins += amount;
        GameEventManager.UpdateCoins?.Invoke();
    }
}