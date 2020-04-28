using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : SelectableObject
{
    private NodeContainer Container;

    public List<Node> Neighbours { get; set; } = new List<Node>();
    public List<Node> TmpNeighbours { get; set; }
    public Texture Stone_tex, Blue_tex, Brown_tex;

    void Start()
    {
        Stone_tex = (Texture)Resources.Load("Images/Scrab_col");
        Blue_tex = (Texture)Resources.Load("Images/Scarab_blue");
        Brown_tex = (Texture)Resources.Load("Images/Scarab_brown");

        Container = GetComponentInParent<NodeContainer>();
        TmpNeighbours = new List<Node>(Neighbours);
    }

    public void Restart()
    {
        TmpNeighbours = new List<Node>(Neighbours);
        Available = true;
        ChangeTexture(1);
    }
    public void ChangeTexture(int i)
    {
        switch(i)
        {
            case 1:
                {
                    gameObject.GetComponentInChildren<Renderer>().material.SetTexture("_MainTex", Stone_tex);
                    break;
                }
            case 2:
                {
                    gameObject.GetComponentInChildren<Renderer>().material.SetTexture("_MainTex", Brown_tex);
                    break;
                }
            case 3:
                {
                    gameObject.GetComponentInChildren<Renderer>().material.SetTexture("_MainTex", Blue_tex);
                    break;
                }
            default:
                {
                    gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", Stone_tex);
                    break;
                }
        }
    }

    public override void Action()
    {
        if (!Container.Started)
        {
            Container.StartPuzzle(this);
        }
        else if(!Container.Completed.Contains(this))
        {
            Container.SetNewSelectedNode(this);
            Container.PuzzleAction();

        }
        Container.CheckNiegbourhood();
        Container.CheckEndCondition();
    }
}
