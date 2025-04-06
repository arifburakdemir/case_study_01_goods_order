using Assets._Code.Box;
using Assets._Code.Product;
using System;
using UnityEngine;

namespace Assets._Code.Box
{
    [RequireComponent(typeof(ProductDeckManager))]
    public class BoxStateController : MonoBehaviour
    {
        public Action onStateChange;

        [Header("Lock Settings")]
        [SerializeField] private BoxStateController[] lockBoxAr;

        [Header("Animator Settings")]
        [SerializeField] private Animator boxAnimator;

        [Header("Render Settings")]
        [SerializeField] private Renderer boxRenderer;
        [SerializeField] private Material lockedMat;
        [SerializeField] private Material unlockedMat;

        public ProductDeckManager ProductDeckManager { get; private set; }
        public bool IsLocked { get; set; }

        void Awake()
        {
            ProductDeckManager = GetComponent<ProductDeckManager>();
            boxAnimator = GetComponent<Animator>();
        }

        public void Init()
        {
            RefreshState();
        }

        private void RefreshState()
        {
            foreach (var curBox in lockBoxAr)
            {
                if (curBox && !curBox.ProductDeckManager.IsCompleted)
                {
                    boxRenderer.material = lockedMat;
                    boxAnimator.SetTrigger("Closed");
                    IsLocked = true;
                    return;
                }
            }

            boxRenderer.material = unlockedMat;
            boxAnimator.SetTrigger("Opened");
            IsLocked = false;
        }

    }
}