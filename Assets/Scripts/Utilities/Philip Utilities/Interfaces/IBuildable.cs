using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All buildable prefabs need a manager object that holds all the scripts
public interface IBuildable
{
    GameObject BuiltObject { get; }
    StructureObjectSettings StructureObjectSettings { get; }
    bool IsBuilt { get; }
    void OnBuilt();
    bool HasBuildRequirements();
    void SetIsBuilt(bool value);
}
