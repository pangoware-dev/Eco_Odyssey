using UnityEngine;

public class scrCaptureSystem : MonoBehaviour
{
    public GameObject captureUI; // Painel da barra
    public scrCaptureBar captureBar;

    public LayerMask enemyLayer;

    private bool isCapturing = false;
    private GameObject currentEnemy;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("C apertado");

            if (!isCapturing)
            {
                Debug.Log("Iniciando captura");
                TryStartCapture();
            } else if (isCapturing)
            {
                Debug.Log("Tentando capturar...");
                TryCapture();
            }
            else
            {
                Debug.Log("NAO está capturando");
            }
        }
    }

    void TryStartCapture()
    {
        Debug.Log("Tentando capturar");

        Collider2D enemy = Physics2D.OverlapCircle(transform.position, 5f, enemyLayer);

        if (enemy != null)
        {
            Debug.Log("Inimigo encontrado!");

            currentEnemy = enemy.gameObject;
            captureUI.SetActive(true);
            captureBar.StartBar(currentEnemy);
            isCapturing = true;
        }
        else
        {
            Debug.Log("Nenhum inimigo encontrado");
        }
    }

    void TryCapture()
    {
        if (captureBar.IsInCenter())
        {
            Debug.Log("ACERTOU!");
            CaptureSuccess();
        }
        else
        {
            Debug.Log("ERROU!");
            CaptureFail();
        }
    }

    void CaptureSuccess()
    {
        Debug.Log("Capturado!");

        SaveEnemyData(currentEnemy);

        Destroy(currentEnemy);

        EndCapture();
    }

    void CaptureFail()
    {
        Debug.Log("Falhou!");
        EndCapture();
    }

    void EndCapture()
    {
        captureUI.SetActive(false);
        isCapturing = false;
    }

    void SaveEnemyData(GameObject enemy)
    {
        scrEnemyEco ecoComp = enemy.GetComponent<scrEnemyEco>();

        if (ecoComp != null && ecoComp.ecoData != null)
        {
            scrGlobalStatus global =
                FindAnyObjectByType<scrGlobalStatus>();

            global.AddEco(ecoComp.ecoData);

            Debug.Log("Eco capturado!");
        }
        else
        {
            Debug.Log("Inimigo sem Eco");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}