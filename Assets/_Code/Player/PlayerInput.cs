using Assets._Code;
using Assets._Code.Box;
using Assets._Code.Product;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Camera _camera;

    private BoxController _selBoxController;
    private List<ProductController> _selProductList;

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
                var curBoxController = hit.transform.GetComponent<BoxController>();
                var productList = curBoxController.productManager.OnSelectProduct(hit.point);
                if (productList.Count > 0)
                {
                    _selBoxController = curBoxController;
                    _selProductList = productList;
                }

                // First time collided with box
                if (_selBoxController == null)
                {

                }
                // First time collided with box
                else
                {

                }
            }
        }
    }
}