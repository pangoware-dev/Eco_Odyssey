using UnityEngine;

public class NPC_Trainer : MonoBehaviour
{
        // PARTY DE ECOS
    public scrEcoFather[] ecoParty = new scrEcoFather[7];

    // HP ATUAL DE CADA ECO
    public float[] vidaAtualParty = new float[7];

    // ECO ATUALMENTE SELECIONADO
    public int currentEcoIndex = 0;
    public scrEcoFather currentEco;
    private levelCheck levelCheck;
    private NPC npc;
    private NPC_Patrol npc_Patrol;
    private NPC_Talking npc_Talking;
    public Animator anim;
    public CircleCollider2D ecoCol;
    public CircleCollider2D dialogueCol;
    public CapsuleCollider2D trainCol;
    private scrChase chase;
    private scrEHealth eHealth;
    private scrEnemyKB eKB;
    private scrEnemyEco eEco;
    private scrAnimationControl animControl;

    // STATUS DO ECO ATUAL
    public float vidaAtual;
    public float vidaC;
    public float atkC;
    public float defC;
    public float veloC;
    public int levelC;
    public int quantEco;

    public RuntimeAnimatorController animControllerC;
    public RuntimeAnimatorController animControllerNPC;
    public bool usingEco = false;

    public void Start()
    {
        levelCheck = GetComponent<levelCheck>();
        npc = GetComponent<NPC>();
        npc_Patrol = GetComponent<NPC_Patrol>();
        npc_Talking = GetComponent<NPC_Talking>();
        chase = GetComponent<scrChase>();
        eHealth = GetComponent<scrEHealth>();
        eKB = GetComponent<scrEnemyKB>();
        eEco = GetComponent<scrEnemyEco>();
        animControl = GetComponent<scrAnimationControl>();


        quantEco = 0;

        for (int i = 0; i < ecoParty.Length; i++)
        {
            if (ecoParty[i] != null)
            {
                quantEco++;

                // Se o Eco começa com HP cheio
                vidaAtualParty[i] = CalcularVidaMaxima(i);
            }
        }

        // Equipa o primeiro Eco vivo
        for (int i = 0; i < ecoParty.Length; i++)
        {
            if (ecoParty[i] != null && vidaAtualParty[i] > 0)
            {
                EquipEco(i);
                break;
            }
        }
        usingEco=false;
    }

    void Update()
    {
        if (trainCol.enabled==false && (Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.DownArrow)||Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.RightArrow)))
        {
            dialogueCol.enabled=false;
        }

        if (chase.enabled==true)
        {
            npc_Patrol.enabled=false;
        }
    }

    public void StartBattle()
    {
        usingEco = true;
        gameObject.layer = LayerMask.NameToLayer("Enemy");

        //Ativar eco
        ecoCol.enabled=true;
        chase.enabled=true;
        eHealth.enabled=true;
        eKB.enabled=true;
        eEco.enabled=true;
        levelCheck.enabled=true;
        animControl.enabled=true;
        

        //Desativar NPC
        trainCol.enabled=false;
        npc.enabled=false;
        npc_Patrol.enabled=false;
        ChangeAnimator();

        eHealth.currentHP=eHealth.maxHP;
    }

    public void EndBattle()
    {
        usingEco = false;
        gameObject.layer = LayerMask.NameToLayer("Default");

        //Ativar eco
        ecoCol.enabled=false;
        chase.enabled=false;
        eHealth.enabled=false;
        eKB.enabled=false;
        eEco.enabled=false;
        levelCheck.enabled=false;
        animControl.enabled=false;
        

        //Desativar NPC
        trainCol.enabled=true;
        dialogueCol.enabled=true;
        npc.enabled=true;
        npc_Patrol.enabled=true;

        ChangeAnimator();

    }
    

    public void ChangeAnimator()
    {
        if (usingEco==true)
        {
            anim.runtimeAnimatorController = currentEco.animControllerEco;
        }
        else if(usingEco==false)
        {
            anim.runtimeAnimatorController = animControllerNPC;

        }
    }


    // =========================
    // EQUIPAR ECO
    // =========================

    public bool EquipEco(int index)
    {
        // Índice inválido
        if (index < 0 || index >= ecoParty.Length)
        {
            Debug.Log("Índice de Eco inválido.");
            return false;
        }

        // Eco morto
        if (vidaAtualParty[index] <= 0)
        {
            Debug.Log(
                "O Eco do slot " +
                (index + 1) +
                " está morto."
            );

            return false;
        }

        // Define o índice atual
        currentEcoIndex = index;

        // Define o Eco atual
        currentEco = ecoParty[index];

        // Calcula a vida máxima do Eco
        vidaC = levelCheck.hp;

        // Recupera o HP salvo daquele slot
        vidaAtual = vidaAtualParty[index];

        // Outros atributos
        atkC = levelCheck.atk;
        defC = levelCheck.def;

        // Velocidade não escala com levelC
        veloC = levelCheck.speed;

        // Animação
        animControllerC = currentEco.animControllerEco;

        return true;
    }


    // =========================
    // SALVAR HP DO ECO ATUAL
    // =========================

    public void SaveCurrentEcoHealth(float hp)
    {
        if (currentEco == null)
            {
                return;
            }

        // Salva o HP no slot do Eco atualmente equipado
        vidaAtualParty[currentEcoIndex] = hp;

        // Atualiza também o HP atual
        vidaAtual = hp;
    }

    private float CalcularVidaMaxima(int index)
    {
        if (ecoParty[index] == null)
            return 0;

        // Ajuste esta fórmula conforme seu sistema
        return (ecoParty[index].Vida + 2 * Mathf.Sqrt(levelCheck.level)) * 10;
    }


    // =========================
    // PEGAR HP DO ECO ATUAL
    // =========================

    public float GetCurrentEcoHealth()
    {
        if (currentEco == null)
        {
            return 0;
        }

        return vidaAtualParty[currentEcoIndex];
    }


    // =========================
    // ENCONTRAR PRÓXIMO ECO VIVO
    // =========================

    public int GetNextAliveEco()
    {
        // Primeiro procura nos slots seguintes
        for (int i = currentEcoIndex + 1; i < ecoParty.Length; i++)
        {
            if (ecoParty[i] != null && vidaAtualParty[i] > 0)
            {
                return i;
            }
        }

        // Depois procura nos slots anteriores
        for (int i = 0; i < currentEcoIndex; i++)
        {
            if (ecoParty[i] != null && vidaAtualParty[i] > 0)
            {
                return i;
            }
        }
    
        Debug.Log("Todos os Ecos morreram!");
        EndBattle();

        // Nenhum Eco vivo
        return -1;
    }

    /* void ApplyEcoStats(){

        ecoSpeed = globalStatus.veloC;

        speed = (ecoSpeed + 15 * Mathf.Sqrt( globalStatus.levelC)) / 10;

        life.MaxHealth = globalStatus.vidaC;

        life.CurrentHealth = globalStatus.vidaAtual;

        life.UpdateHPText();

        anim.runtimeAnimatorController = globalStatus.animControllerC;
    } */
}
