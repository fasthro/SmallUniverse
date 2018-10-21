using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SU.Behaviour;

public class CharacterControler : GameBehaviour {

    private string characterName;

    public static CharacterControler Create(string _characterName)
    {
        GameObject go = new GameObject();
        go.name = "CharacterControler";
        var controler = go.AddComponent<CharacterControler>();
        controler.Initialize(_characterName);
        return controler;
    }

    private void Initialize(string _characterName)
    {
        characterName = _characterName;
    }

    public void Born()
    {

    }
}
