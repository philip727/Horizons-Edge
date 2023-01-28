using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    public static CharacterInputActions CharacterInputActions { private set; get; }
    void Awake()
    {
        CharacterInputActions = new CharacterInputActions();
    }

    public static IEnumerator WaitForInputController(Action func)
    {
        yield return new WaitUntil(() => CharacterInputActions != null);
        func();
    }
}
