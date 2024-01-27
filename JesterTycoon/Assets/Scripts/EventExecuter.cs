using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is used to call start the event chain 

public class EventExecuter : MonoBehaviour
{
    [SerializeField] int money;
    [SerializeField] int Multiplier;
    [SerializeField] int PopupDelay;
    void Start(){ GameManager.start(1000, 0, 10, this); }
}
