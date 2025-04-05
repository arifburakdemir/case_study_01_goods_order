using Assets._Code.Product;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Assets._Code.Box
{
    [RequireComponent(typeof(BoxController))]
    public class BoxProductManager : MonoBehaviour
    {
        private const int ProductLimit = 6;

        [SerializeField] private List<ProductController> _startProductList;
        [SerializeField] private List<ProductPoint> _pointDataList;

        public bool IsCompleted;
        public BoxController boxController;

        public int ProductCount => _pointDataList.Count(p => !p.IsEmpty);

        public void Init()
        {
            for (int index = 0; index < _startProductList.Count; index++)
            {
                var productPref = _startProductList[index];
                var pointData = _pointDataList[index];
                pointData.productController = Instantiate(productPref, pointData.productPoint.position, pointData.productPoint.rotation).GetComponent<ProductController>();
            }
        }

        public List<ProductController> OnSelectProduct(Vector3 hitPos)
        {
            var filteredProductAr = _pointDataList
                 .Where(p => !p.IsEmpty)
                 .OrderBy(p => Vector3.Distance(hitPos, p.productPoint.position))
                 .ToList();

            if (filteredProductAr.Count > 0)
            {
                var productList = SelectAll(filteredProductAr[0].productController.ProductId);
                foreach (var curProduct in productList)
                    curProduct.Pick();

                return productList;
            }

            return null;
        }

        public void OnTransfer(List<ProductController> productsToTransfer, BoxProductManager targetBox)
        {
            foreach (var curPointData in _pointDataList)
            {
                if (productsToTransfer.Contains(curPointData.productController))
                    curPointData.productController = null;

            }

            var remeaning = ProductLimit - targetBox.ProductCount;
            for (int index = 0; index < remeaning; index++)
            {
            }
        }

        public List<ProductController> SelectAll(ProductId targetProductId)
        {
            return _startProductList.Where(p => p.ProductId == targetProductId)
                .ToList();
        }

        public void Release()
        {

        }

    }

    [Serializable]
    public class ProductPoint
    {
        public Transform productPoint;
        public ProductController productController;

        public bool IsEmpty => productController == null;
    }

}