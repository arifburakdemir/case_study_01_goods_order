using Assets._Code.Product;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Assets._Code
{
    public class ProductController: MonoBehaviour
    {
        [SerializeField] private ProductId _productId;
        [SerializeField] private Transform _productBody;

        public ProductId ProductId => _productId;

        public void Pick()
        {
            _productBody.DOLocalMoveY(1,0.3f);
        }

        public void Release()
        {
            _productBody.DOLocalMoveY(0, 0.3f);
        }

    }
}