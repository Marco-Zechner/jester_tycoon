using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCollisionChecker : MonoBehaviour
{
    public bool isOccupied = false;

    List<Collider> collisions = new List<Collider>();

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Placeable"))
        {
            isOccupied = true;
            collisions.Add(other.collider);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Placeable"))
        {
            collisions.Remove(other.collider);
            if (collisions.Count == 0)
            {
                isOccupied = false;
            }
        }
    }
}
