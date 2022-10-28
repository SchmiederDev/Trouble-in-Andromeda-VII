using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UnionFighter : MonoBehaviour
{

    public Vector3 FighterPosition;

    [SerializeField]
    private Animator UnionFighterAnim;

    [SerializeField]
    private Light2D ShipEngineLight;

    [SerializeField]
    private ShipWeapons MyShipWeapons;

    [SerializeField]
    private List<UnionFighterWeapon> FighterWeapons;

    public UnionFighterWeapon ActiveWeapon;

    private string standardWeaponName = "Phaser_Level_01";

    public delegate void OnWeaponChanged();
    public OnWeaponChanged weaponChanged;

    int currentWeaponIndex = 0;

    public int StructuralIntegrity { get; private set; }
    public int IntegrityMax { get; } = 100;
    public int IntegrityMin { get; } = 0;

    private bool WasDestroyed = false;

    public int Energy { get; private set; }
    public int EnergyMax { get; } = 100;
    public int EnergyMin { get; } = 0;
    

    public int ShieldPower { get; private set; }
    public int ShieldPowerMax { get; } = 100;
    public int ShieldPowerMin { get; } = 0;
    public bool IsShieldActive { get; private set; } = false;

    public delegate void OnShieldStateChanged();
    public OnShieldStateChanged ShieldStateChanged;


    private int XP;
    public delegate void OnXPChanged();
    public OnXPChanged XP_Changed;

    // Start is called before the first frame update
    void Awake()
    {
        UnionFighterAnim = GetComponent<Animator>();
        ShipEngineLight = GetComponentInChildren<Light2D>();
        
        MyShipWeapons = GetComponentInChildren<ShipWeapons>();
        
        MyShipWeapons.onWeaponFired += FireActiveWeapon;
        
        Reset_Stats();
        
        ActiveWeapon = Get_StandardWeapon(standardWeaponName);
    }

    // Update is called once per frame
    void Update()
    {
        FighterPosition = gameObject.transform.position;
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            ChangeWeapon();
            
        }
    }    

    private UnionFighterWeapon Get_StandardWeapon(string StandardWeaponName)
    {
        UnionFighterWeapon StandardWeapon = FighterWeapons.Find(WeaponElement => WeaponElement.WeaponName == StandardWeaponName);
        return StandardWeapon;
    }

    public void SufferedCollison(int impactMagnitude)
    {
        StructuralIntegrity -= impactMagnitude;
        Check_IntegrityState();
    }

    private void Check_IntegrityState()
    {
        if(StructuralIntegrity <= IntegrityMin)
        {
            FighterWasDestroyed();
        }
    }

    private void Check_ShieldState()
    {
        if (ShieldPower > 0)
        {
            IsShieldActive = true;
        }

        else IsShieldActive = false;

        ShieldStateChanged.Invoke();
    }

    private void FighterWasDestroyed()
    {
        WasDestroyed = true;
        UnionFighterAnim.SetBool("WasDestroyed", WasDestroyed);

        ShipEngineLight.intensity = 0f;

        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(0.25f);       
        
        TheGame.theGameInst.StartLevel();

        ShipEngineLight.intensity = 1.25f;
        
        WasDestroyed = false;
        UnionFighterAnim.SetBool("WasDestroyed", WasDestroyed);
    }

    public bool PickUpItem(CollectableItem item)
    {
        switch(item.target)
        {
            case "StructuralIntegrity":
                {
                    Repair_Ship(item.charge);
                    return true;
                }

            case "Energy": 
                {
                    Recharge_Ship(item.charge);
                    return true;
                }

            case "Shield":
                {
                    PowerShield(item.charge);
                    return true;
                }            

            case "ShipWeapon":
                {
                    return true;
                }

            default:  
                {
                    Debug.LogWarning("Target not found");
                    return false;
                }
        }       

    }

    private void Repair_Ship(int chargeLoad)
    {
        StructuralIntegrity += chargeLoad;

        if (StructuralIntegrity > IntegrityMax)
            StructuralIntegrity = IntegrityMax;
    }

    private void Recharge_Ship(int chargeLoad)
    {
        Energy += chargeLoad;

        if (Energy > EnergyMax)
            Energy = EnergyMax;
    }

    private void PowerShield(int chargeLoad)
    {
        if (!IsShieldActive)
            IsShieldActive = true;

        ShieldPower += chargeLoad;

        if (ShieldPower > ShieldPowerMax)
            ShieldPower = ShieldPowerMax;

        ShieldStateChanged.Invoke();
    }

   private void ChangeWeapon()
    {
        int maxIndex = FighterWeapons.Count - 1;
        
        currentWeaponIndex++;
        if (currentWeaponIndex > maxIndex)
            currentWeaponIndex = 0;
        
        SelectWeapon();
        
        weaponChanged.Invoke();
    }

    private void SelectWeapon()
    {
        if (FighterWeapons[currentWeaponIndex].IsActivated)
            ActiveWeapon = FighterWeapons[currentWeaponIndex];
        else
            ChangeWeapon();
    }

    private void FireActiveWeapon()
    {
        int PreviewEnergyLoad = Energy;
        PreviewEnergyLoad -= ActiveWeapon.EnergyLoad;

        if (PreviewEnergyLoad >= EnergyMin)
            Energy -= ActiveWeapon.EnergyLoad;
    }

    public void SufferDamage(int damageRaw, int shieldBreachCapacity)
    {
        Check_ShieldState();

        if (IsShieldActive)
        {
            int shieldDamage = damageRaw - shieldBreachCapacity;
            ShieldPower -= shieldDamage;
            StructuralIntegrity -= shieldBreachCapacity;
        }

        else StructuralIntegrity -= damageRaw;

        Check_IntegrityState();
    }

    public void Gain_XP(int amount)
    {
        XP += amount;
        XP_Changed.Invoke();
    }

    public int Get_XP()
    {
        return XP;
    }    

    private void Reset_Stats()
    {
        StructuralIntegrity = IntegrityMax;
        ShieldPower = ShieldPowerMin;
        Energy = EnergyMax;
    }
    
    private void Reset_XP()
    {
        XP = 0;
        XP_Changed.Invoke();
    }

    public void Reset_UnionFighter_OnLevelLoad()
    {
        Reset_XP();
        Reset_Stats();
    }

}
