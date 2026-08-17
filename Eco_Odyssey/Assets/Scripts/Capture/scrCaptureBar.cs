using UnityEngine;
using UnityEngine.UI;

public class scrCaptureBar : MonoBehaviour
{
    public RectTransform pointer;
    public RectTransform centerZone;

    public float speed = 1000f;
    private bool movingRight = true;

    private float minX;
    private float maxX;
    private scrEHealth enemyHealth;
    
    void Start()
    {
        minX = -200f;
        maxX = 200f;
    }

    public void StartBar(GameObject enemy)
    {
        enemyHealth = enemy.GetComponent<scrEHealth>();
        pointer.anchoredPosition = new Vector2(minX, pointer.anchoredPosition.y);
        movingRight = true;
    }

    void Update()
    {
        MovePointer();
        ChangeSpeed();
    }

    void MovePointer()
    {
        float direction = movingRight ? 1 : -1;

        pointer.anchoredPosition += new Vector2(direction * speed * Time.deltaTime, 0);

        if (pointer.anchoredPosition.x >= maxX)
            movingRight = false;
        else if (pointer.anchoredPosition.x <= minX)
            movingRight = true;
    }

    public bool IsInCenter()
    {
        float pointerX = pointer.anchoredPosition.x;

        float min = centerZone.anchoredPosition.x - (centerZone.sizeDelta.x / 2);
        float max = centerZone.anchoredPosition.x + (centerZone.sizeDelta.x / 2);

        return pointerX >= min && pointerX <= max;
    }


    void ChangeSpeed()
    {
        if (enemyHealth == null)
            return;

        Debug.Log("HP: " + enemyHealth.currentHP + " / " + enemyHealth.maxHP);

        speed = (enemyHealth.currentHP * 100 / enemyHealth.maxHP)* 50;
    }
}