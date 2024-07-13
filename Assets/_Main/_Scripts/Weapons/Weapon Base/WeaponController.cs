using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// base script for all weapon
/// </summary>

public class WeaponController : MonoBehaviour
{
    public RectTransform[] noTouchAreas;
    protected float modFireRate;
    protected float modDamage;
    private float nextFireTime=0;
    [SerializeField] protected Transform positionShoot;
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;
   


    protected Player player;

    [Header("Change guns")]
    int totalWeapons = 2;
    public static int currentWeaponIndex=0;
    [SerializeField] protected GameObject[] guns;
    [SerializeField] protected GameObject weaponHolder;
    [SerializeField] protected GameObject currentGun;
    [Space]
    [SerializeField] protected Sprite[] spriteGuns;

    private void Awake()
    {
        modFireRate = weaponData.FireRate;
        player = FindObjectOfType<Player>();

        // Inicializar la primera arma
        if (guns.Length > 0)
        {
           
            weaponHolder = guns[0];
            currentGun = guns[0];
        }
        else
        {
            Debug.LogError("No weapons found in guns array.");
        }
    }

    protected virtual void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(false);
        }
        guns[currentWeaponIndex].SetActive(true);
        //currentCooldown = weaponData.CooldownDuration;
    }

    void Update()
    {
        // Manejo del disparo
        var inputActive = Input.GetMouseButtonDown(0);
        if (MobileDetector.IsMobile())
        {
            inputActive = InputTouch();
        }
       
        if (inputActive && Time.time > nextFireTime)
        {
            Attack();
            nextFireTime = Time.time + modFireRate; // Actualizar el tiempo en el que se podr� disparar nuevamente
        }
        else if (Input.GetKeyDown(KeyCode.Q))  // Cambio de arma
        {
            ChangeWeapon();
        }
    }

    public bool InputTouch()
    {
        // Verificar si hay al menos dos toques para poder comparar a partir del segundo
        if (Input.touchCount > 1)
        {
            // Iterar sobre los toques a partir del segundo (�ndice 1)
            for (int i = 1; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                // Verificar si el toque NO est� dentro de alguna �rea de "no toque"
                if (!IsMouseInsideNoTouchAreas(touch.position))
                {
                    return true; // Devolver true si encontramos al menos un toque fuera de un �rea de "no toque"
                }
            }

            // Si todos los toques a partir del segundo est�n dentro de �reas de "no toque", devolver false
            return false;
        }

        // Verificar si el primer toque (�ndice 0) NO est� dentro de alguna �rea de "no toque"
        if (Input.touchCount > 0)
        {
            Touch firstTouch = Input.GetTouch(0);
            if (!IsMouseInsideNoTouchAreas(firstTouch.position))
            {
                return true; // Devolver true si el primer toque est� fuera de un �rea de "no toque"
            }
        }

        // Devolver false si no se detect� ning�n toque o si todos los toques est�n dentro de �reas de "no toque"
        return false;
    }

    bool IsMouseInsideNoTouchAreas(Vector2 touchPosition)
    {
        if (noTouchAreas == null || noTouchAreas.Length == 0)
            return false;

        foreach (RectTransform area in noTouchAreas)
        {
            Vector2 localTouchPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(area, touchPosition, null, out localTouchPosition))
            {
                if (area.rect.Contains(localTouchPosition))
                {
                    return true; // Devolver true si el toque est� dentro de alguna de las �reas de "no toque"
                }
            }
        }

        return false; // Devolver false si el toque no est� dentro de ninguna de las �reas de "no toque"
    }
    public void ChangeWeapon()
    {
        // Desactivar el arma actual
        guns[currentWeaponIndex].SetActive(false);

        // Incrementar el �ndice del arma actual
        currentWeaponIndex++;

        // Reiniciar el �ndice si es necesario
        if (currentWeaponIndex >= guns.Length)
        {
            currentWeaponIndex = 0;
        }

        // Activar la nueva arma
        guns[currentWeaponIndex].SetActive(true);

        // Chequear que no se pase del rango
        if (currentWeaponIndex <= spriteGuns.Length)
        {
            GameManager.instance.UpdateInfo(spriteGuns[currentWeaponIndex]);
        }
       
    }
    protected virtual void Attack()
    {
        //AudioManager.instance.Play("ShootPaper");

        // Cada arma podr�a tener su sonido.
        //AudioManager.instance.Play("Bullet");

        // Reinicia el ciclo de tiempo.
        //currentCooldown = weaponData.CooldownDuration;

    }

}
