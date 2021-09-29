using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{

    public static void CreateIndicator(Vector3 position, int damage, GameObject damageIndicatorPrefab)
{

            GameObject prefab = Instantiate(damageIndicatorPrefab, position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMesh>().text = damage.ToString();

    }

}
