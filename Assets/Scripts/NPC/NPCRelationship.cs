using UnityEngine;
using System.Collections.Generic;

public class NPCRelationship : MonoBehaviour
{
    public NPCType npcType;
    [Range(-50, 50)]
    public int relationship = 0;
    [TextArea]
    public string summary;
    [TextArea]
    public List<string> discoveredFacts = new();
    public string npcName;
    public bool hasMet = false;
    

    private NPCData Data => NPCDatabase.Instance.GetNPC(npcType);

    public int Relationship => Data.relationship;

    public bool HasMet
    {
        get => Data.hasMet;
        set => Data.hasMet = value;
    }

    public List<string> Facts => Data.discoveredFacts;


    //METHODS TO INSTANTIATE INTO THE DATABASE ALONG WITH HELPER FUNCTIONS
    private void Start()
    {
        NPCDatabase.Instance.Register(this);
    }

    public void ChangeRelationship(int amount)
    {
        Data.relationship += amount;
        Data.relationship =
            Mathf.Clamp(Data.relationship, -50, 50);
    }

    public void AddFact(string fact)
    {
        if (!Data.discoveredFacts.Contains(fact))
        {
            Data.discoveredFacts.Add(fact);
        }
    }

    public string GetRelationshipTitle()
    {
        if (relationship <= -10) return "Hostile";
        if (relationship < 0) return "Dislike";
        if (relationship < 5) return "Neutral";
        if (relationship < 15) return "Friendly";
        if (relationship < 25) return "Companionable";

        return "Trusted";
    }
}