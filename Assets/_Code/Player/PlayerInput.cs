using Assets._Code;
using Assets._Code.Box;
using Assets._Code.Product;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Camera _camera;

    private BaseBoxController _selBoxController;
    private List<ProductController> _selProductList;
    private ProductId _selProductId;

    private Sequence _selectionSeq;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out var hit, 99))
            {
                var hitObj = hit.transform;
                // If hit on Box
                if (hitObj.CompareTag("Box") || hitObj.CompareTag("PlayerDock"))
                {
                    var curBoxController = hitObj.GetComponent<BaseBoxController>();

                    // If box is Locked
                    if (curBoxController.IsLocked())
                        return;

                    if (_selBoxController == null)
                        FirstBoxSelect(curBoxController, hit.point);
                    else if (curBoxController != _selBoxController)
                        DiffirentBoxSelect(curBoxController, hit.point);
                    else
                        SameBoxSelect(curBoxController, hit.point);

                }
            }
            // ----- If hit nothing
            else if (_selBoxController != null)
                ReleasePrevious();
        }
    }

    private void SameBoxSelect(BaseBoxController boxController, Vector3 hitPos)
    {
        var curProductType = boxController.productDeckManager.SelectProductIdByPosition(hitPos);
        var productList = boxController.productDeckManager.SelectAll(curProductType);

        // If new productId is same with previous release
        if (curProductType == _selProductId)
        {
            ReleasePrevious();
        }
        // If new productId is NOT the same with previous release
        else
        {
            FirstBoxSelect(boxController, hitPos);
        }

    }

    private void FirstBoxSelect(BaseBoxController boxController, Vector3 hitPos)
    {
        // Release if there is already selected Products
        if (_selProductList != null)
            ReleasePrevious();

        _selBoxController = boxController;

        // Get products by click pos on Box
        _selProductId = boxController.productDeckManager.SelectProductIdByPosition(hitPos);
        _selProductList = boxController.productDeckManager.SelectAll(_selProductId);

        // Sequence to lift products
        _selectionSeq = DOTween.Sequence();
        foreach (var curProduct in _selProductList)
        {
            _selectionSeq.AppendCallback(curProduct.Pick)
                .AppendInterval(0.1f);
        }
    }

    private void DiffirentBoxSelect(BaseBoxController targetBox, Vector3 hitPos)
    {
        // If Deck is full start fresh selection
        if (targetBox.productDeckManager.IsFull)
        {
            FirstBoxSelect(targetBox, hitPos);
        }
        else
        {
            // Selected Product Count & Target Box Empty Count
            var transferList = new List<ProductController>();
            var objTransferCount = Mathf.Min(targetBox.productDeckManager.Remaining, _selProductList.Count);
            for (int index = 0; index < objTransferCount; index++)
            {
                transferList.Add(_selProductList[index]);

                _selProductList.RemoveAt(index);
                objTransferCount--;
                index--;
            }

            // Release remaining products if selected product count greater than empty count on target box
            foreach (var curProduct in _selProductList)
                curProduct.Release();

            _selBoxController.productDeckManager.RemoveProduct(transferList);
            targetBox.productDeckManager.AddProduct(transferList);


            _selBoxController = null;
            _selProductList = null;
        }
    }

    private void ReleasePrevious()
    {
        _selBoxController = null;

        _selectionSeq.Kill(); // Stop selection previous sequence to overlap

        foreach (var curProduct in _selProductList)
            curProduct.Release();
    }
}