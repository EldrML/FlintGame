using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill:ScriptableObject
{
    [SerializeField]
    private Djinn.DjinnElement _element;
    public Djinn.DjinnElement Element { get { return _element; } }
    [SerializeField]
    private RangeType _range;
    public RangeType Range { get { return _range; } }
    [SerializeField]
    private AttackType _attackType;
    public AttackType AttackType { get { return _attackType; } }
    [SerializeField]
    private TargetType _target;
    public TargetType Target { get { return _target; } }
    [SerializeField]
    private int _power;
    public int Power { get { return _power; } }
    [SerializeField]
    private string _name;
    public string Name { get { return _name; } }
    [SerializeField]
    private string _description;
    public string Description { get { return _description; } }
}