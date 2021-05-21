using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake() =>
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    public void BuyTurret(GameObject selectedTurret)
    {
        if (selectedTurret.GetComponent<Turret>().turretCost > gameManager.currency)
            return;
        else
            Instantiate(selectedTurret);
    }
}
