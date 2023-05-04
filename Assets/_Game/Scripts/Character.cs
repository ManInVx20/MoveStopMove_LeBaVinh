using System;
using System.Collections;
using System.Collections.Generic;
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
    private Transform knifeWeaponHolder;
    [SerializeField]
    private Transform hammerWeaponHolder;
    [SerializeField]
    private Transform boomerangWeaponHolder;
    [SerializeField]
    private Transform throwPoint;
    [SerializeField]
    private float sizeDelta = 0.025f;
    [SerializeField]
    private ParticleSystem hitVFX;
    [SerializeField]
    private ParticleSystem levelUpVFX;

    private NavMeshAgent agent;
    private CapsuleCollider capsuleCollider;
    private CharacterInfoUI characterInfoUI;
    private ObjectColorSO objectColorSO;
    private WeaponSO weaponSO;
    private Weapon weapon;
    private Character target;
    private SkinSO hatSkinSO;
    private SkinSO pantSkinSO;
    private SkinSO shieldSkinSO;
    private SkinSO fullSetSkinSO;
    private bool isDead;
    private bool attacked;
    private bool despawned;
    private float despawnTimer;
    private float despawnTimerMax = 3.0f;
    private bool boosted;
    private float boostTimer;
    private float boostTimerMax = 3.0f;
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

        ChangeSize(charLevel);

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

    public void UpdateSkinSO(SkinSO hatSkinSO, SkinSO pantSkinSO, SkinSO shieldSkinSO, SkinSO fullSetSkinSO, bool pernament = true)
    {
        characterVisual.ResetSkin();

        if (objectColorSO != null)
        {
            characterVisual.SetBodyMaterial(objectColorSO.NormalMaterial);
        }

        if (pernament)
        {
            if (hatSkinSO != null)
            {
                this.hatSkinSO = hatSkinSO;
            }
            if (pantSkinSO != null)
            {
                this.pantSkinSO = pantSkinSO;
            }
            if (shieldSkinSO != null)
            {
                this.shieldSkinSO = shieldSkinSO;
            }
            if (fullSetSkinSO != null)
            {
                this.fullSetSkinSO = fullSetSkinSO;
            }
        }

        if (fullSetSkinSO != null)
        {
            characterVisual.SetSet(fullSetSkinSO);
        }
        else
        {
            characterVisual.SetHat(hatSkinSO);
            characterVisual.SetPant(pantSkinSO);
            characterVisual.SetShield(shieldSkinSO);
        }
    }

    public void UpdateWeaponSO(WeaponSO weaponSO)
    {
        DespawnWeapon();

        this.weaponSO = weaponSO;

        SpawnWeapon();
    }

    public virtual void Victory()
    {
        ChangeAnim(CharacterAnimator.Anim.Victory);
    }

    public virtual void LevelUp(Character target)
    {
        if (charLevel >= target.GetLevel())
        {
            SetLevel(charLevel + 1);
        }
        else
        {
            SetLevel(charLevel + 2);
        }

        ChangeSize(charLevel);

        levelUpVFX.Play();

        SoundManager.Instance.PlaySizeUpSound(transform.position);
    }

    public virtual void Attack()
    {
        attacked = true;

        RotateToTarget();

        weapon.PrepareToFire();

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

        if (objectColorSO != null && fullSetSkinSO == null)
        {
            characterVisual.SetBodyMaterial(objectColorSO.DeathMaterial);
        }

        hitVFX.Play();

        ChangeAnim(CharacterAnimator.Anim.Death);

        DespawnCharacterInfoUI();

        OnCharacterDeath?.Invoke(this, EventArgs.Empty);

        SoundManager.Instance.PlayDeathSound(transform.position);
    }

    public virtual void Boost()
    {
        boosted = true;
        boostTimer = 0.0f;

        int modifier = charLevel / 5;
        int tempCharLevel = charLevel * (2 - modifier / (modifier + 1));
        ChangeSize(tempCharLevel);
    }

    protected virtual void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    protected virtual void Begin()
    {
        isDead = false;
        agent.enabled = true;
        capsuleCollider.enabled = true;
        despawnTimer = 0.0f;
        despawned = false;
        target = null;
        boosted = false;

        ResetAttack();

        GetObjectColorSO(out objectColorSO);
        GetSkinSO(out hatSkinSO, out pantSkinSO, out shieldSkinSO, out fullSetSkinSO);
        GetWeaponSO(out weaponSO);

        if (objectColorSO != null)
        {
            characterVisual.SetBodyMaterial(objectColorSO.NormalMaterial);
        }

        UpdateSkinSO(hatSkinSO, pantSkinSO, shieldSkinSO, fullSetSkinSO);

        if (weaponSO != null && weapon == null)
        {
            SpawnWeapon();
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
        if (IsDead() && !despawned)
        {
            despawnTimer += Time.deltaTime;
            if (despawnTimer >= despawnTimerMax)
            {
                Despawn();
            }
        }

        if (!IsDead() && boosted)
        {
            boostTimer += Time.deltaTime;
            if (boostTimer >= boostTimerMax)
            {
                boosted = false;

                ChangeSize(charLevel);
            }
        }
    }

    protected virtual void Despawn()
    {
        despawned = true;
        capsuleCollider.enabled = false;

    }


    protected virtual void GetObjectColorSO(out ObjectColorSO objectColorSO)
    {
        objectColorSO = null;
    }

    protected virtual void GetSkinSO(out SkinSO hatSkinSO, out SkinSO pantSkinSO, out SkinSO shieldSkinSO, out SkinSO fullSetSkinSO)
    {
        hatSkinSO = null;
        pantSkinSO = null;
        shieldSkinSO = null;
        fullSetSkinSO = null;
    }

    protected virtual void GetWeaponSO(out WeaponSO weaponSO)
    {
        weaponSO = null;
    }

    private void ChangeSize(int charLevel)
    {
        multiplier = 1.0f + sizeDelta * (charLevel - 1);
        transform.localScale = Vector3.one * multiplier;
    }

    private void SpawnWeapon()
    {
        GameObject weaponGameObject;

        switch (weaponSO.WeaponType)
        {
            case WeaponType.Knife:
                weaponGameObject = Instantiate(weaponSO.Prefab, knifeWeaponHolder);
                break;
            case WeaponType.Hammer:
                weaponGameObject = Instantiate(weaponSO.Prefab, hammerWeaponHolder);
                break;
            case WeaponType.Boomerang:
                weaponGameObject = Instantiate(weaponSO.Prefab, boomerangWeaponHolder);
                break;
            default:
                weaponGameObject = new GameObject();
                break;
        }

        weapon = weaponGameObject.GetComponent<Weapon>();
    }
}
