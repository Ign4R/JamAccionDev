using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TorretImpresora : WeaponBehaviour
{
    public GameObject prefabProyectil = null;


    private float nextFireTime;
    private float fireRate;

    void Update()
    {
        //Busca enemigos dentro del radio de ataque
        //Si hay objetivo, entonces dispara
        currentCooldownDuration += Time.deltaTime;
        if (currentCooldownDuration >= 5) 
        {
            Attack(); // Actualizar el tiempo en el que se podr� disparar nuevamente
            currentCooldownDuration = 0;
        }

    }


    private void Attack()
    {
        //Configuro con direccion y da�o
        GameObject spawnedDiploma = Instantiate(prefabProyectil, transform.position, transform.rotation);
        spawnedDiploma.GetComponent<DiplomaBehaviour>().DirectionChecker(transform.right);
    }
}
