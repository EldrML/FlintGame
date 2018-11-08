using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Battle/Weapon")]
public class Weapon  :Item {

    [SerializeField]
    private Unleash _unleash;
    public Unleash Unleash { get { return _unleash; } }
}
