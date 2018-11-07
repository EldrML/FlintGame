using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyController : MonoSingleton<PartyController>
{
    [SerializeField]
    private List<EntityData> _debugParty;

    protected override PartyController GetSingletonInstance { get { return this; } }

    [SerializeField]
    private List<EntityData> _partyMembers;

    public IReadOnlyList<EntityData> PartyMembers { get { return _partyMembers; } }

    private void Start()
    {
        foreach (var entity in _debugParty)
            _partyMembers.Add(Instantiate(entity));
    }

    public void AddMember(EntityData partyMember)
    {
        _partyMembers.Add(Instantiate(partyMember));
    }
}
