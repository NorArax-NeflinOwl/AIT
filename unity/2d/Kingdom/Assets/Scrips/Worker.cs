using System.Threading;
using UnityEngine;

public class Worker : MonoBehaviour
{
    private Building firstPoint;
    private GameObject SecondPoint;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        
    }

    private void Initialize()
    {
        Worker[] workers = FindObjectsByType<Worker>(FindObjectsSortMode.InstanceID);                // Find all workers on the scene.
        Thread.Sleep(Random.Range(0, workers.Length * 2));                                           // Wait random time until start.

        //Find first no occupied building
        Building[] buildings = FindObjectsByType<Building>(FindObjectsSortMode.InstanceID);
        foreach (Building building in buildings)
        {
            if (!building.IsOccupied)
            {
                building.IsOccupied = true;                                                          // Lock the building
                firstPoint = building;
                break;
            }
        }
    }

    private void TryGoToFirstPoint()
    {
        // Go to the building
        if (null != firstPoint)
        {

        }
        else
        {
            // Repeat searching
            Initialize();
        }
    }
}

/* TODO List of worker:
 * 1. Find building.
 * 2. Go to the building.
 * 3. Check if a building is build.
 * 4. If it is go the step no.
 * 5. Check if amount of material is enough ant start building.
 * 6. If materials must be collected then collect Required Materials from enviroment
 * 7. Check step no.5
 * 8. Start collecting materials into storage.
 */