using UnityEngine;

public class scrEHealth : MonoBehaviour{
    public float currentHP;
    public float maxHP;

    private scrEnemyEco ecoComp;
    public float D;

    void Start(){
        ecoComp = GetComponent<scrEnemyEco>();
        D = 0.5f;

        if (ecoComp != null && ecoComp.ecoData != null){
            maxHP = ecoComp.ecoData.Vida;
        }

        currentHP = maxHP;
    }

    public void changeHP(float amount){
        currentHP -= amount / (ecoComp.ecoData.Defesa * D) / 2 + 1;

        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        Debug.Log("HP Inimigo: " + currentHP);

        if (currentHP>maxHP||currentHP<=0){
            currentHP=maxHP;
        }
    }
}
