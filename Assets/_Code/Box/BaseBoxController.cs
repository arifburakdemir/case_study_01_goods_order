using Assets._Code;
using Assets._Code.Box;
using Assets._Code.Product;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProductDeckManager))]
public class BaseBoxController : MonoBehaviour
{
    [SerializeReference] protected ProductDeckManager _productDeckManager;

    public ProductDeckManager productDeckManager => _productDeckManager;

    public Action<BaseBoxController> onDestroy;

    private void Reset()
    {
        _productDeckManager = GetComponent<ProductDeckManager>();
    }

    private void Awake()
    {
        _productDeckManager.OnComplete += OnComplete;
        _productDeckManager.OnEmpty += OnEmpty;
    }

    public virtual bool IsLocked()
    {
        return _productDeckManager.IsLocked;
    }

    public virtual void OnEmpty()
    {
        Debug.Log("On Empty");
        onDestroy?.Invoke(this);
    }

    public virtual void OnComplete()
    {
        Debug.Log("On Complete");
        onDestroy?.Invoke(this);
    }

}