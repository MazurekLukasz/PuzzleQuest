using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : SelectableObject
{
    [SerializeField] NodeContainer Container;

    void Start()
    {
        Available = false;
    }

    public override void Action()
    {
        if (Container.Started && Available)
        {
            Container.ResetPuzzle();
        }
    }
}

