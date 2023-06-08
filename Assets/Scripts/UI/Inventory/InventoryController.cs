using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] ItemGrid selectedItemGrid;

    // Update is called once per frame
    void Update()
    {
        if (selectedItemGrid == null) { return; }

        Debug.Log(selectedItemGrid.GetTileGridPosition(Input.mousePosition));
    }
}
