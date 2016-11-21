using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GetFreeGold : MonoBehaviour
{
    public void Get100FreeGold()
    {
        EventManager.Instance.TriggerEvent(new ChangeResources(premium: 100));
    }
}