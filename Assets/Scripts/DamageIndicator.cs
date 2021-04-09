using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TEST PR CHANGES

public class DamageIndicator : MonoBehaviour
{

    public static void Create(Vector3 position, int damage, GameObject damageIndicatorPrefab)
{

            GameObject prefab = Instantiate(damageIndicatorPrefab, position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMesh>().text = damage.ToString();

    }

}
