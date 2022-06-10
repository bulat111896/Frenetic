using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class PurchaseManager : MonoBehaviour, IStoreListener
{
    public GameObject[] panelPush;
    public GameObject debiting;
    public Button ads;
    public Text money;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;
    private int currentProductIndex;

    public string[] NC_PRODUCTS;

    public string[] C_PRODUCTS;

    public static event OnSuccessConsumable OnPurchaseConsumable;

    public static event OnSuccessNonConsumable OnPurchaseNonConsumable;

    public static event OnFailedPurchase PurchaseFailed;

    void PanelPush(int count)
    {
        panelPush[1].SetActive(false);
        panelPush[0].SetActive(false);
        panelPush[0].SetActive(true);
        if (count > 0)
        {
            debiting.SetActive(false);
            debiting.SetActive(true);
            debiting.GetComponent<Text>().text = "+" + count;
        }
    }

    void Awake()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        foreach (string s in C_PRODUCTS)
            builder.AddProduct(s, ProductType.Consumable);
        foreach (string s in NC_PRODUCTS)
            builder.AddProduct(s, ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyConsumable(int index)
    {
        currentProductIndex = index;
        BuyProductID(C_PRODUCTS[index]);
    }

    public void BuyNonConsumable(int index)
    {
        currentProductIndex = index;
        BuyProductID(NC_PRODUCTS[index]);
    }

    void BuyProductID(string productId)
    {
        if (m_StoreController != null && m_StoreExtensionProvider != null)
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
                m_StoreController.InitiatePurchase(product);
            else
                OnPurchaseFailed(product, PurchaseFailureReason.ProductUnavailable);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {

    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (C_PRODUCTS.Length > 0 && string.Equals(args.purchasedProduct.definition.id, C_PRODUCTS[currentProductIndex], StringComparison.Ordinal))
            OnSuccessC(args);
        else if (NC_PRODUCTS.Length > 0 && string.Equals(args.purchasedProduct.definition.id, NC_PRODUCTS[currentProductIndex], StringComparison.Ordinal))
            OnSuccessNC(args);
        return PurchaseProcessingResult.Complete;
    }

    public delegate void OnSuccessConsumable(PurchaseEventArgs args);

    protected virtual void OnSuccessC(PurchaseEventArgs args)
    {
        if (OnPurchaseConsumable != null)
            OnPurchaseConsumable(args);
        switch (C_PRODUCTS[currentProductIndex])
        {
            case "one":
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 30);
                PanelPush(30);
                break;
            case "two":
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 100);
                PanelPush(100);
                break;
            case "three":
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 500);
                PanelPush(500);
                break;
            case "four":
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 1000);
                PanelPush(1000);
                break;
        }
        money.text = PlayerPrefs.GetInt("Money").ToString();
    }

    public delegate void OnSuccessNonConsumable(PurchaseEventArgs args);

    protected virtual void OnSuccessNC(PurchaseEventArgs args)
    {
        if (OnPurchaseNonConsumable != null)
            OnPurchaseNonConsumable(args);
        if (NC_PRODUCTS[currentProductIndex] == "ads")
        {
            PlayerPrefs.SetInt("Ads", 1);
            ads.interactable = false;
            PanelPush(0);
        }
    }

    public delegate void OnFailedPurchase(Product product, PurchaseFailureReason failureReason);

    protected virtual void OnFailedP(Product product, PurchaseFailureReason failureReason)
    {
        PurchaseFailed(product, failureReason);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        OnFailedP(product, failureReason);
    }
}