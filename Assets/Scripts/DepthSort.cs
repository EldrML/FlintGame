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

    [SerializeField]
    private Transform _anchor = null;

    public bool Dynamic { get { return _dynamic; } set { _dynamic = value; } }

    // Use this for initialization
    void Start()
    {
        _sortingGroup = GetComponent<SortingGroup>();
        _anchor = _anchor == null ? transform : _anchor;
        _sortingGroup.sortingOrder = GetSortingForTransform(_anchor);
        enabled = Dynamic;
    }

    // Update is called once per frame
    void Update()
    {
        _sortingGroup.sortingOrder = GetSortingForTransform(_anchor);
    }

    public static int GetSortingForTransform(Transform anchor)
    {
        return short.MaxValue - Mathf.RoundToInt(anchor.position.y * 20f);
    }
}