using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(menuName = "Gameplay/Battle/Character")]
public class Character : ScriptableObject
{ 

    [SerializeField]
    private List<Psy> _psy = new List<Psy>();
    [SerializeField]
    private List<Djinn> _djinn = new List<Djinn>();
    [SerializeField]
    private List<Item> _item = new List<Item>();
    [SerializeField]
    private Weapon _weapon;
    public Weapon Weapon { get { return _weapon; } }

    public int pp = 100;
    public int hp = 100;
    public Skill ChosenSkill { get; set; }
    public string ChosenTarget { get; set; }
    public string Name = "";

    public List<Psy> GetPsy()
    {
        return _psy.Where(x => x.Cost <= pp).ToList();
    }
    public List<Djinn> GetDjinn()
    {
        return _djinn.ToList();
    }
    public List<Item> GetItems()//Todo:change to item type
    {
        return _item.ToList();
    }
    public bool CanAttack()
    {
        return _weapon != null;
    }
}