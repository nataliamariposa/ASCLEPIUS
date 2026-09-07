using UnityEngine;
using System.Collections.Generic;

public class NPCDatabase : MonoBehaviour
{
    public static NPCDatabase Instance;
    private Dictionary<NPCType, NPCData> npcs = new Dictionary<NPCType, NPCData>();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Register(NPCRelationship npc)
    {
        if (!npcs.ContainsKey(npc.npcType))
        {
            NPCData data = new NPCData();

            data.npcType = npc.npcType;
            data.npcName = npc.npcName;
            data.summary = npc.summary;

            npcs.Add(npc.npcType, data);
        }
    }

    public NPCData GetNPC(NPCType type)
    {
        return npcs[type];
    }

    public IEnumerable<NPCData> GetAllNPCs()
    {
        return npcs.Values;
    }
}
