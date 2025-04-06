using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets._Code.Product
{
    public class ProductDeckManager : MonoBehaviour
    {
        [SerializeField] private List<ProductController> _startProductList;
        [SerializeField] private List<ProductPoint> _pointDataList;
        [SerializeField] private int _productLimit = 6;

        public bool IsCompleted;

        public int ProductCount => _pointDataList.Count(p => !p.IsEmpty);
        public bool IsFull => ProductCount == _productLimit;
        public int Remaining => _productLimit - ProductCount;
        public bool IsLocked { get; private set; }

        public void Init()
        {
            for (int index = 0; index < _startProductList.Count; index++)
            {
                var productPref = _startProductList[index];
                var pointData = _pointDataList[index];
                pointData.productController = Instantiate(productPref, pointData.productPoint.position, pointData.productPoint.rotation).GetComponent<ProductController>();
            }
        }

        public void RemoveProduct(List<ProductController> productsToRemove)
        {
            foreach (var newProduct in productsToRemove)
            {
                foreach (var curPointData in _pointDataList)
                {
                    if (curPointData.productController == newProduct)
                        curPointData.productController = null;
                }
            }
        }

        public void AddProduct(List<ProductController> productsToTransfer)
        {
            IsLocked = true;

            var transeferCount = 0;
            var seq = DOTween.Sequence();
            for (int index = 0; index < productsToTransfer.Count; index++)
            {
                var newProduct = productsToTransfer[index];
                foreach (var curPointData in _pointDataList)
                {
                    if (curPointData.productController == null)
                    {
                        curPointData.productController = newProduct;
                        productsToTransfer.RemoveAt(index--);

                        // Stop Floating Tween
                        newProduct.Release();

                        // Jump Product
                        var jumpTween = newProduct.transform.DOJump(curPointData.productPoint.position, 5, 1, 0.5f);
                        if (productsToTransfer.Count == 0)
                            jumpTween.OnComplete(() => IsLocked = false);
                        seq.Insert(transeferCount * 0.1f, jumpTween);

                        transeferCount++;
                        break;
                    }
                }
            }
        }

        public ProductId SelectProductIdByPosition(Vector3 hitPos)
        {
            // Order Product by distance between hitPos
            var filteredProductAr = _pointDataList
                 .Where(p => !p.IsEmpty)
                 .OrderBy(p =>
                 {
                     Debug.DrawLine(hitPos, p.productPoint.position, Color.red, 5);
                     return Vector3.Distance(hitPos, p.productPoint.position);
                 })
            .ToList();

            return filteredProductAr[0].productController.ProductId;
        }

        public List<ProductController> SelectAll(ProductId targetProductId)
        {
            return _pointDataList
                .Where(p => p.productController && p.productController.ProductId == targetProductId)
                .Select(p => p.productController)
                .ToList();
        }

        [Serializable]
        public class ProductPoint
        {
            public Transform productPoint;
            public ProductController productController;

            public bool IsEmpty => productController == null;
        }
    }
}