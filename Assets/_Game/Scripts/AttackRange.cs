using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField]
    private Character character;

    private SphereCollider sphereCollider;
    private List<Character> characterInRangeList = new List<Character>();

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        character.OnCharacterStarted += Character_OnCharacterStart;
        character.OnCharacterDeath += Character_OnCharacterDeath;
    }

    private void Update()
    {
        UpdateTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Obstacle>(out Obstacle obstacle) && this.character is Player)
        {
            obstacle.SetTrasparent();
        }

        if (other.TryGetComponent<Character>(out Character character))
        {
            if (character != this.character && !character.IsDead() && !characterInRangeList.Contains(character))
            {
                characterInRangeList.Add(character);

                if (this.character is Player)
                {
                    ((Bot)character).EnterPlayerAttackRange();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Obstacle>(out Obstacle obstacle) && this.character is Player)
        {
            obstacle.SetNormal();
        }

        if (other.TryGetComponent<Character>(out Character character))
        {
            if (character != this.character && !character.IsDead() && characterInRangeList.Contains(character))
            {
                characterInRangeList.Remove(character);

                if (this.character is Player)
                {
                    ((Bot)character).ExitPlayerAttackRange();
                }
            }
        }
    }

    private void Character_OnCharacterStart(object sender, EventArgs args)
    {
        characterInRangeList.Clear();

        Show();
    }

    private void Character_OnCharacterDeath(object sender, EventArgs args)
    {
        Hide();
    }

    private void UpdateTarget()
    {
        characterInRangeList.RemoveAll(character => character.IsDead());

        if (characterInRangeList.Count > 0)
        {
            character.SetTarget(characterInRangeList[0]);
        }
        else
        {
            character.SetTarget(null);
        }
    }

    private void Show()
    {
        sphereCollider.enabled = true;
    }

    private void Hide()
    {
        sphereCollider.enabled = false;
    }
}
