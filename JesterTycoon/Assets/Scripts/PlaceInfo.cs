using System.Collections.Generic;
using UnityEngine;

public class PlaceInfo : MonoBehaviour{
    public PlaceInfo parentPlace = null;
    [SerializeField]
    public List<PlaceInfo> childPlaces = new List<PlaceInfo>();
}