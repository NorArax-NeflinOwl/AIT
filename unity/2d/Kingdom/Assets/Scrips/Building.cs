using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool IsBuild = false;
    public List<string> RequiredMaterialsNames;
    public List<int> RequiredMaterialsAmount;

    [HideInInspector]
    public bool IsOccupied = false;

    private Dictionary<Material, int> RequiredMaterials;
    private Dictionary<Material, int> Storage;

    private void Start()
    {
        RequiredMaterials = new Dictionary<Material, int>();
        Storage = new Dictionary<Material, int>();

        FillDictionaries();
    }

    private void FillDictionaries()
    {
        Material[] materials= FindObjectsByType<Material>(FindObjectsSortMode.InstanceID);
        for (int i = 0; i < RequiredMaterialsNames.Count; i++)
        {
            foreach (Material mat in materials)
            {
                if(mat.name == RequiredMaterialsNames[i])
                {
                    RequiredMaterials.Add(mat, RequiredMaterialsAmount[i]);
                    Storage.Add(mat, 0);
                }
            }
        }
    }
}
