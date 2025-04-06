using Assets._Code.Product;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Assets._Code
{
    public class ProductController : MonoBehaviour
    {
        [SerializeField] private ProductId _productId;
        [SerializeField] private Transform _productBody;

        public ProductId ProductId => _productId;

        public void Pick()
        {
            _productBody.DOKill();
            _productBody.DOLocalMoveY(1, 0.3f)
                .OnComplete(()=> _productBody.DOLocalMoveY(0.1f, 1f)
                                    .SetRelative()
                                    .SetEase(Ease.InOutQuad)
                                    .SetLoops(-1,LoopType.Yoyo));
            
        }

        public void Release()
        {
            _productBody.DOKill();
            _productBody.DOLocalMoveY(0, 0.15f);
        }

    }
}