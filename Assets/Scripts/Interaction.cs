using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    [SerializeField] float Range = 15f;
    SelectableObject Selected;
    [SerializeField] Image pointer;


    // Update is called once per frame
    void Update()
    {
        CheckObject();
    }

    void CheckObject()
    {
        if(Selected != null)
        {
            Selected.LookAtMe = false;
            ChangePointerColor(Color.white);
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Range, LayerMask.GetMask("Selectable")))
        {
            Selected = hit.transform.GetComponent<SelectableObject>();
            if (Selected != null)
            {
                if (Selected.Available)
                {
                    Selected.LookAtMe = true;
                    ChangePointerColor(Color.yellow);

                    if (Input.GetButtonDown("Fire1"))
                    {
                        Selected.Action();
                    }
                }
            }
        }

        void ChangePointerColor(Color col)
        {
            pointer.color = col;
        }
    }
}
