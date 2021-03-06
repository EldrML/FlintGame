﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(menuName = "Gameplay/Battle/Character")]
public class Character : ScriptableObject
{
    public AnimationClip AnimIdle;
    public AnimationClip AnimIdleReverse;
    public AnimationClip AnimAttack;
    public AnimationClip AnimAttackReverse;
    public AnimationClip AnimHit;
    public AnimationClip AnimHitReverse;
    public AnimationClip AnimSummon;
    public AnimationClip AnimSummonReverse;

    public float x;
    public float y;

    [SerializeField]
    private List<Psy> _psy = new List<Psy>();
    [SerializeField]
    private List<Djinn> _djinn = new List<Djinn>();
    [SerializeField]
    private List<ItemSkill> _item = new List<ItemSkill>();
    [SerializeField]
    private WeaponSkill _weapon;
    public WeaponSkill Weapon { get { return _weapon; } }

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
    public List<ItemSkill> GetItems()//Todo:change to item type
    {
        return _item.ToList();
    }
    public bool CanAttack()
    {
        return _weapon != null;
    }
}