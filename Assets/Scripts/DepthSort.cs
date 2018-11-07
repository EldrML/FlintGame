using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SortingGroup))]
public class DepthSort : MonoBehaviour
{
    private SortingGroup _sortingGroup;

    [SerializeField]
    private bool _dynamic = false;

    public bool Dynamic { get { return _dynamic; } set { _dynamic = value; } }
    
    // Use this for initialization
    void Start()
    {
        _sortingGroup = GetComponent<SortingGroup>();
        _sortingGroup.sortingOrder = GetSortingForTransform(transform);
        enabled = Dynamic;
    }

    // Update is called once per frame
    void Update()
    {
        _sortingGroup.sortingOrder = GetSortingForTransform(transform);
    }

    public static int GetSortingForTransform(Transform transform)
    {
        return short.MaxValue - Mathf.RoundToInt(transform.position.y * 20f);
    }
}