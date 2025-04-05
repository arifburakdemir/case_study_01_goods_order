using Assets._Code.Box;
using UnityEngine;

[RequireComponent(typeof(BoxProductManager), typeof(BoxStateController))]
public class BoxController : MonoBehaviour
{
    [SerializeReference] private BoxProductManager _productManager;
    [SerializeReference] private BoxStateController _stateController;

    public BoxProductManager productManager => _productManager;
    public BoxStateController stateController => _stateController; 

    private void Reset()
    {
        _productManager = GetComponent<BoxProductManager>();
        _productManager.boxController = this;

        _stateController = GetComponent<BoxStateController>();
        _stateController.boxController = this;
    }

    private void Awake()
    {
        _stateController.Init();
        _productManager.Init();
    }

}