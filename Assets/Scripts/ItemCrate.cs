using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCrate : MonoBehaviour {

    [Header("Settings")]
    public GameObject m_storedObject;

	public GameObject GetObject()
    {
        return Instantiate(m_storedObject, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
    }
}
