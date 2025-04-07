using Assets._Code.Product;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Code.Box
{
    [RequireComponent(typeof(ProductDeckManager))]
    public class BoxController : BaseBoxController
    {
        [SerializeReference] private BoxStateController _stateController;

        public BoxStateController stateController => _stateController;

        private void Reset()
        {
            _productDeckManager = GetComponent<ProductDeckManager>();
            _stateController = GetComponent<BoxStateController>();
        }

        private void Start()
        {
            _stateController.Init();
            _productDeckManager.Init();
        }

        public override bool IsLocked()
        {
            return _stateController.IsLocked || base.IsLocked();
        }


    }
}