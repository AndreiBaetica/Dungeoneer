using UnityEngine;

public class SavedGameIndicator : MonoBehaviour
{

    public static void Create(Vector3 position, GameObject savedGameIndicatorPrefab)
    {
        Instantiate(savedGameIndicatorPrefab, position, Quaternion.identity);
    }

}