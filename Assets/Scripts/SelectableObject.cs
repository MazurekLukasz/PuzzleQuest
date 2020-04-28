using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectableObject : MonoBehaviour
{
    public bool Available { get; set; }  = true;
    public bool LookAtMe {set; get;} = false;
    public int RedCol=255;
    public int BlueCol =255;
    public int GreenCol = 255;
    public int ColMultiplier = 1;
    public Color ObjectCol = Color.white;
    bool FlashingIn = false;

    public abstract void Action();

    protected void Update()
    {
        SetColor(ObjectCol);

        if (LookAtMe)
        {
            StopCoroutine(FlashObject());
            ObjectCol = Color.yellow;
            FlashingIn = false;
        }
        else
        {
            if (!Available)
            {
                StopCoroutine(FlashObject());
                ObjectCol = Color.white;
                FlashingIn = false;
            }
            else
            {
                if (!FlashingIn)
                {                    
                    StartCoroutine(FlashObject());
                    FlashingIn = true;
                }
            }
        }
    }

    protected IEnumerator FlashObject()
    {
        while (Available && !LookAtMe)
        {
            yield return new WaitForSeconds(0.05f);

            if (BlueCol <= 100)
            {
                ColMultiplier = 1;
            }
            else if (BlueCol >= 250)
            {
                ColMultiplier = -1;
            }
            BlueCol = BlueCol + (ColMultiplier) * 5;
            //RedCol = RedCol + (ColMultiplier) * 10;
            //GreenCol = GreenCol + (ColMultiplier) * 10;

            ObjectCol =  new Color32(255, (byte)BlueCol, (byte)BlueCol, 255);
        }
    }

    void SetColor(Color col)
    {
        if (gameObject.GetComponent<Renderer>())
        {
            gameObject.GetComponent<Renderer>().material.color = col;
        }
        else
        {
            gameObject.GetComponentInChildren<Renderer>().material.color = col;
        }
        
        
    }
}
