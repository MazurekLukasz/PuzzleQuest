using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeContainer : MonoBehaviour
{
    [SerializeField] private Lever Butt;
    Node[] Nodes;
    Edge[] Edges;
    public bool Started { get; set; } = false;
    public List<Node> Completed { get; private set; } = new List<Node>();
    [SerializeField] LineRenderer Line;
    int LinePointIndex = 0;
    [SerializeField] GameObject TextPanel;
    bool ShowCommunicate = false;
    public Node Selected { get; set; }
    public Node LastSelected { get; set; }
    [SerializeField] GameObject[] Effects;
    public bool Solved { get; private set;}
    NodeContainer[] AllPuzzles;

    void Awake()
    {
        Nodes = gameObject.GetComponentsInChildren<Node>();
        Edges = gameObject.GetComponentsInChildren<Edge>();
        InitEdges();
        SetAllNeighbourhood();
        InitLineRenderer();

        AllPuzzles = FindObjectsOfType<NodeContainer>();
    }
    void InitLineRenderer()
    {
        Line = GetComponent<LineRenderer>();
        Line.material = new Material(Shader.Find("Sprites/Default"));
        Line.widthMultiplier = 0.1f;
        Line.positionCount = 0;
        ChangeLineColor(Color.blue);
    }

    public void ChangeLineColor(Color col)
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(col, 0.0f), new GradientColorKey(col, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(1f, 1.0f) }
        );
        Line.colorGradient = gradient;
    }

    void InitEdges()
    {
        foreach (Edge edge in Edges)
        {
            edge.Init();
        }
    }

    public void ResetPuzzle()
    {
        Butt.Available = false;
        Selected = null;
        LastSelected = null;
        InitNodes();
        Started = false;
        Completed.Clear();
        RestartLine();
    }

    public void StartPuzzle(Node StartNode)
    {
        Started = true;
        Selected = StartNode;
        AddLinePoint(StartNode);
        SetAllNodesAvailability(false);
        SetAvailable(StartNode.TmpNeighbours, true);
        Butt.Available = true;
    }


    void SetAllNeighbourhood()
    {
        
        foreach (Edge conn in Edges)
        {
            conn.Node1.Neighbours.Add(conn.Node2);
            conn.Node2.Neighbours.Add(conn.Node1);
        }
    }

    public void SetAvailable(List<Node> list, bool active)
    {
        if (active)
        {
            foreach (Node item in list)
            {
                if (!Completed.Contains(item))
                {
                    item.Available = active;
                }
            }
        }
        else
        {
            foreach (Node item in list)
            {
                item.Available = active;
            }
        }
    }

    public void SetAllNodesAvailability(bool active)
    {
        foreach (Node node in Nodes)
        {
            node.Available = active;
        }
    }
    public void AddLinePoint(Node node)
    {
        Line.positionCount++;
        Line.SetPosition(LinePointIndex, node.transform.position);
        LinePointIndex++;
    }
    void RestartLine()
    {
        LinePointIndex = 0;
        Line.positionCount = 0;
        ChangeLineColor(Color.blue);

    }
    public void CheckEndCondition()
    {
        if (WinCondition())
        {
            Butt.Available = false;
            ChangeLineColor(Color.cyan);
            
            ActivateTorches(true);
            Solved = true;

            if (EndGame())
            {
                StartCoroutine(ShowEndGameText("You have just solved all of the puzzle quests! The game will quit automatically!"));
            }
            else
            {
                ShowCommunicate = true;
                StartCoroutine(ShowText("You have just solved one of the puzzle quests!"));
            }
        }
        else if (Selected.TmpNeighbours.Count <= 0)
        {
            ChangeLineColor(Color.red);
            ShowCommunicate = true;
            StartCoroutine(ShowText("You have failed. Find and click restart button!"));
        }
    }

    bool EndGame()
    {
        foreach (var item in AllPuzzles)
        {
            if (!item.Solved)
            {
                return false;
            }
        }
        return true;
    }

    bool WinCondition()
    {
        foreach (var node in Nodes)
        {
            if (!Completed.Contains(node))
            { return false; } 
        }
        return true;
    }

    void InitNodes()
    {
        foreach (Node node in Nodes)
        {
            node.Restart();
        }
    }

    public void SetNewSelectedNode(Node newNode)
    {
        LastSelected = Selected;
        Selected = newNode;
    }

    public void PuzzleAction()
    {
        LastSelected.TmpNeighbours.Remove(Selected);
        Selected.TmpNeighbours.Remove(LastSelected);
        AddLinePoint(Selected);

        SetAvailable(LastSelected.TmpNeighbours, false);
        SetAvailable(Selected.TmpNeighbours, true);
        Selected.Available = false;
        LastSelected.Available = false;
    }

    public void CheckNiegbourhood()
    {
        if (LastSelected != null)
        {
            LastSelected.ChangeTexture(2);
            if (LastSelected.TmpNeighbours.Count <= 0)
            {
                Completed.Add(LastSelected);
                LastSelected.ChangeTexture(3);
            }
        }
        Selected.ChangeTexture(2);
        if (Selected.TmpNeighbours.Count <= 0)
        {
            Completed.Add(Selected);
            Selected.ChangeTexture(3);
        }
    }

    IEnumerator ShowText(string txt)
    {
        while(ShowCommunicate)
        {
            TextPanel.GetComponentInChildren<Text>().text = txt;
            TextPanel.SetActive(true);
            
            yield return new WaitForSeconds(6f);
            TextPanel.SetActive(false);
            ShowCommunicate = false;
        }
    }
    IEnumerator ShowEndGameText(string txt)
    {
            TextPanel.GetComponentInChildren<Text>().text = txt;
            TextPanel.SetActive(true);

            yield return new WaitForSeconds(10f);
            Application.Quit();
    }

    void ActivateTorches(bool acti)
    {
        foreach (var item in Effects)
        {
            item.SetActive(acti);
        }
    }
}
