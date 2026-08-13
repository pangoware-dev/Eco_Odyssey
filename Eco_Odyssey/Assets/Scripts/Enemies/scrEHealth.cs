using UnityEngine;
using UnityEngine.UI;

public class scrEHealth : MonoBehaviour
{
    public float currentHP;
    public float maxHP;

    private scrEnemyEco ecoComp;
    private BlinkingSprite blink;
    public GameObject HealthBar;
    private Slider healthBarSlider;

    public float D, Damage;

    void Start()
    {
        ecoComp = GetComponent<scrEnemyEco>();
        D = 0.5f;

        if (ecoComp != null && ecoComp.ecoData != null)
        {
            maxHP = ecoComp.ecoData.Vida;
        }

        currentHP = maxHP;

        if (blink == null)
        {
            blink = gameObject.AddComponent<BlinkingSprite>();
        }
        blink = GetComponent<BlinkingSprite>();

        GameObject bar = Instantiate(HealthBar);
        healthBarSlider = HealthBar.GetComponentInChildren<Slider>();
    }

    public void FixedUpdate()
    {
        healthBarSlider.maxValue = maxHP;
        healthBarSlider.value = currentHP;
    }

    public void changeHP(float amount)
    {
        Damage = amount/(ecoComp.ecoData.Defesa*D)/2+1;
        currentHP -= Mathf.CeilToInt(Damage);

        Debug.Log("HP Inimigo: " + currentHP);
        blink.Blink();

        if(currentHP > maxHP || currentHP <= 0)
        {
            currentHP = maxHP;
        }
    }
}