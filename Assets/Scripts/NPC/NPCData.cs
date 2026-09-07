using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class NPCData
{
    public NPCType npcType;

    public string npcName;

    public bool hasMet;

    public int relationship;

    public string summary;

    public List<string> discoveredFacts = new();

    public string GetRelationshipTitle()
	{
	    if (relationship <= -10) return "Hostile";
	    if (relationship < 0) return "Dislike";
	    if (relationship < 5) return "Neutral";
	    if (relationship < 15) return "Friendly";
	    if (relationship < 25) return "Companionable";

	    return "Trusted";
	}

	public void ChangeRelationship(int amount)
	{
	    relationship += amount;
	    relationship = Mathf.Clamp(relationship, -50, 50);
	}

		public void AddFact(string fact)
	{
	    if (!discoveredFacts.Contains(fact))
	    {
	        discoveredFacts.Add(fact);
	    }
	}
}