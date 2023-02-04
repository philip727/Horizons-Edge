using Philip.Utilities.Extras;
using Philip.Utilities.Maths;
using Philip.WorldGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractionManager : MonoBehaviour
{
    public readonly List<IInteractable> interactables = new List<IInteractable>();

    public void Update()
    {
        GetBestInteractable();
    }

    private IInteractable GetBestInteractable()
    {
        Vector3 mousePosition = PMouse.GetScreenMouseWorldPosition(Camera.main);
        WorldNode mouseNode = WorldGenerationHandler.s_worldData.WorldGrid.GetGridObject(mousePosition);
        IInteractable bestInteractable = null;
        foreach (IInteractable interactable in interactables)
        {
            Vector3 objectPosition = interactable.InteractObject.transform.position;
            WorldNode objectNode = WorldGenerationHandler.s_worldData.WorldGrid.GetGridObject(objectPosition);

            if (mouseNode.Coordinates == objectNode.Coordinates)
            {
                bestInteractable = interactable;
                break;
            }
        }

        return bestInteractable;
    }
}
