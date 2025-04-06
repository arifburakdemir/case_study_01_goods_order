using Assets._Code;
using Assets._Code.Box;
using Assets._Code.Product;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProductDeckManager))]
public class BaseBoxController : MonoBehaviour
{
    [SerializeReference] protected ProductDeckManager _productDeckManager;

    public ProductDeckManager productDeckManager => _productDeckManager;

    private void Reset()
    {
        _productDeckManager = GetComponent<ProductDeckManager>();
    }

    public virtual bool IsLocked()
    {
        return _productDeckManager.IsLocked;
    }

}