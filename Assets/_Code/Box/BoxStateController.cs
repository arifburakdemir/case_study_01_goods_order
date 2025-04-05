using Assets._Code.Box;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxProductManager))]
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

    [SerializeReference, HideInInspector] public BoxController boxController;
    
    public BoxProductManager BoxProductManager { get; private set; }

    void Awake()
    {
        BoxProductManager = GetComponent<BoxProductManager>();
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
            if (curBox && !curBox.BoxProductManager.IsCompleted)
            {
                boxRenderer.material = lockedMat;
                boxAnimator.SetTrigger("Closed");

                return;
            }
        }

        boxRenderer.material = unlockedMat;
        boxAnimator.SetTrigger("Opened");
    }

}
