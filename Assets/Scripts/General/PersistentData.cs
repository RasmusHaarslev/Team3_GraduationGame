using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PersistentData
{
    public static string GetPath()
    {
        return Application.isEditor ? Application.persistentDataPath : "/data/data/dk.dadiu.neonomads/files";
    }

}
