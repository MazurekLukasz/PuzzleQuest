using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    public Node Node1 { get; private set; }
    public Node Node2 { get; private set; }

    [SerializeField] Transform tr1;
    [SerializeField] Transform tr2;
    [SerializeField] float radius = 0.2f;
    
    public void Init()
    {
        Node1 = CheckForNodes(tr1.position);
        Node2 = CheckForNodes(tr2.position);
    }

    Node CheckForNodes(Vector3 pos)
    {
        Collider[] hitColliders = Physics.OverlapSphere(pos, radius, LayerMask.GetMask("Selectable"));
        foreach (var item in hitColliders)
        {
            if (item.GetComponent<Node>())
            {
                return item.GetComponent<Node>();
            }
        }
        return null;
    }
}
