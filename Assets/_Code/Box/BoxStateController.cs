using Assets._Code.Box;
using Assets._Code.Product;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Code.Box
{
    [RequireComponent(typeof(ProductDeckManager))]
    public class BoxStateController : MonoBehaviour
    {
        [Header("Lock Settings")]
        [SerializeField] private List<BaseBoxController> lockBoxAr;

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

            foreach (var curBox in lockBoxAr)
            {
                curBox.onDestroy += RemoveLockedBox;
            }
        }

        private void RemoveLockedBox(BaseBoxController boxToRemove)
        {
            lockBoxAr.Remove(boxToRemove);
            RefreshState();
        }

        private void RefreshState()
        {
            if (lockBoxAr.Count > 0)
            {
                boxRenderer.material = lockedMat;
                boxAnimator.SetTrigger("Closed");
                IsLocked = true;
            }
            else
            {
                boxRenderer.material = unlockedMat;
                boxAnimator.SetTrigger("Opened");
                IsLocked = false;
            }
        }

    }
}