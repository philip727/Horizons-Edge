using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    public CharacterInputActions CharacterInputActions { private set; get; }
    void Awake()
    {
        CharacterInputActions = new CharacterInputActions();
    }

    // Makes sure the input actions aren't null
    public IEnumerator WaitForInputController(Action func)
    {
        yield return new WaitUntil(() => CharacterInputActions != null);
        func();
    }
}
