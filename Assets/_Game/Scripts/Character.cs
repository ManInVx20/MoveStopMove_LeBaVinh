using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class Character : PoolableObject
{
    public class OnCharacterInfoChangedArgs : EventArgs
    {
        public string Name;
        public int Level;
    }
    public event EventHandler<OnCharacterInfoChangedArgs> OnCharacterInfoChanged;
    public event EventHandler OnCharacterStarted;
    public event EventHandler OnCharacterDeath;

    [SerializeField]
    private CharacterAnimator characterAnimator;
    [SerializeField]
    private CharacterVisual characterVisual;
    [SerializeField]
    private Transform weaponHolder;
    [SerializeField]
    private Transform throwPoint;
    [SerializeField]
    private float sizeDelta = 0.025f;
    [SerializeField]
    private ParticleSystem hitVFX;
    [SerializeField]
    private ParticleSystem levelUpVFX;

    private NavMeshAgent agent;
    private CharacterInfoUI characterInfoUI;
    private ObjectColorSO objectColorSO;
    private WeaponSO weaponSO;
    private Weapon weapon;
    private Character target;
    private SkinSetSO skinSetSO;
    private bool isDead;
    private bool attacked;
    private bool despawned;
    private float despawnTimer;
    private float despawnTime = 3.0f;
    private float multiplier = 1.0f;
    private string charName = "Name";
    private int charLevel = 1;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        Execute();
    }

    public void ChangeAnim(CharacterAnimator.Anim newAnim)
    {
        characterAnimator.ChangeAnim(newAnim);
    }

    public void ResetState()
    {
        Begin();

        ChangeSize();

        OnCharacterStarted?.Invoke(this, EventArgs.Empty);
    }

    public bool IsDead()
    {
        return isDead || gameObject == null;
    }

    public NavMeshAgent GetAgent()
    {
        return agent;
    }

    public void SetName(string name)
    {
        charName = name;

        OnCharacterInfoChanged?.Invoke(this, new OnCharacterInfoChangedArgs
        {
            Name = charName,
            Level = charLevel
        });
    }

    public string GetName()
    {
        return charName;
    }

    public void SetLevel(int level)
    {
        charLevel = level;

        OnCharacterInfoChanged?.Invoke(this, new OnCharacterInfoChangedArgs
        {
            Name = charName,
            Level = charLevel
        });
    }

    public int GetLevel()
    {
        return charLevel;
    }

    public float GetMultiplier()
    {
        return multiplier;
    }

    public Transform GetThrowPoint()
    {
        return throwPoint;
    }

    public ObjectColorSO GetObjectColorSO()
    {
        return objectColorSO;
    }

    public Color GetNormalColor()
    {
        return objectColorSO.NormalMaterial.color;
    }

    public Color GetDeathColor()
    {
        return objectColorSO.DeathMaterial.color;
    }

    public void SetTarget(Character target)
    {
        this.target = target;
    }

    public bool HasTarget()
    {
        return target != null;
    }

    public bool Attacked()
    {
        return attacked;
    }

    public void RotateToTarget()
    {
        if (!HasTarget())
        {
            return;
        }

        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

        if (directionToTarget != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(directionToTarget);
        }
    }

    public void DespawnCharacterInfoUI()
    {
        if (characterInfoUI != null)
        {
            characterInfoUI.Despawn();
            characterInfoUI = null;
        }
    }

    public void DespawnWeapon()
    {
        weapon?.Despawn();
        weapon = null;
    }

    public void UpdateSkinSetSO(SkinSetSO skinSetSO)
    {
        characterVisual.ResetSkinSet();

        if (objectColorSO != null)
        {
            characterVisual.SetBodyMaterial(objectColorSO.NormalMaterial);
        }

        this.skinSetSO = skinSetSO;

        if (skinSetSO != null)
        {
            characterVisual.SetSkinSet(skinSetSO);
        }
    }

    public void UpdateWeaponSO(WeaponSO weaponSO)
    {
        DespawnWeapon();

        this.weaponSO = weaponSO;

        GameObject weaponGameObject = Instantiate(this.weaponSO.Prefab, weaponHolder);
        weapon = weaponGameObject.GetComponent<Weapon>();
    }

    public virtual void Victory()
    {
        ChangeAnim(CharacterAnimator.Anim.Victory);
    }

    public virtual void LevelUp(Character target)
    {
        SetLevel(charLevel + 1);

        ChangeSize();

        levelUpVFX.Play();

        SoundManager.Instance.PlaySizeUpSound(transform.position);
    }

    public virtual void Attack()
    {
        attacked = true;

        RotateToTarget();

        ChangeAnim(CharacterAnimator.Anim.Attack);
    }

    public virtual void ResetAttack()
    {
        attacked = false;
    }

    public virtual void Throw()
    {
        weapon.FireBullet(throwPoint, this);

        SoundManager.Instance.PlayThrowSound(throwPoint.position);
    }

    public virtual void Hit(Character killer)
    {
        isDead = true;
        agent.enabled = false;
        despawnTimer = 0.0f;

        if (objectColorSO != null && skinSetSO == null)
        {
            characterVisual.SetBodyMaterial(objectColorSO.DeathMaterial);
        }

        hitVFX.Play();

        ChangeAnim(CharacterAnimator.Anim.Death);

        DespawnCharacterInfoUI();

        OnCharacterDeath?.Invoke(this, EventArgs.Empty);

        SoundManager.Instance.PlayDeathSound(transform.position);
    }

    protected virtual void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Begin()
    {
        isDead = false;
        agent.enabled = true;
        despawnTimer = 0.0f;
        despawned = false;
        target = null;

        ResetAttack();

        GetObjectColorSO(out objectColorSO);
        GetSkinSetSO(out skinSetSO);
        GetWeaponSO(out weaponSO);

        if (objectColorSO != null)
        {
            characterVisual.SetBodyMaterial(objectColorSO.NormalMaterial);
        }

        UpdateSkinSetSO(skinSetSO);

        if (weaponSO != null && weapon == null)
        {
            GameObject weaponGameObject = Instantiate(weaponSO.Prefab, weaponHolder);
            weapon = weaponGameObject.GetComponent<Weapon>();
        }

        ParticleSystem.MainModule mainModule = hitVFX.main;
        mainModule.startColor = GetNormalColor();

        mainModule = levelUpVFX.main;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(GetNormalColor(), GetDeathColor());

        ChangeAnim(CharacterAnimator.Anim.Idle);

        if (characterInfoUI == null)
        {
            characterInfoUI = ResourceManager.Instance.CharacterInfoUIPool.GetPrefabInstance();
            characterInfoUI.Initialize(this, GetNormalColor());
        }
        else
        {
            characterInfoUI.ChangeColor(GetNormalColor());
        }
    }

    protected virtual void Execute()
    {
        if (isDead && !despawned)
        {
            despawnTimer += Time.deltaTime;
            if (despawnTimer >= despawnTime)
            {
                Despawn();
            }
        }
    }

    protected virtual void Despawn()
    {
        despawned = true;
    }

    protected virtual void GetObjectColorSO(out ObjectColorSO objectColorSO)
    {
        objectColorSO = null;
    }

    protected virtual void GetSkinSetSO(out SkinSetSO skinSetSO)
    {
        skinSetSO = null;
    }

    protected virtual void GetWeaponSO(out WeaponSO weaponSO)
    {
        weaponSO = null;
    }

    private void ChangeSize()
    {
        multiplier = 1.0f + sizeDelta * (charLevel - 1);
        transform.localScale = Vector3.one * multiplier;
    }
}
