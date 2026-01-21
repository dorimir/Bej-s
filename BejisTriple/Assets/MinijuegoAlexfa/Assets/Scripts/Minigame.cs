using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

public enum Sfx
{
    Trash,
    Attach
}

public enum States
{
    Tutorial,
    Playing,
    End
}

public class Minigame : MonoBehaviour, PlayerInputActions.ICursorActions
{
    //SONIDO
    [Header("Audio")]
    [SerializeField] private AudioClip attachSfx;
    [SerializeField] private AudioClip trashSfx;
    [SerializeField] private AudioClip lagrimaMusic;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource musicAudioSource;

    //TUTORIAL
    [Header("Tutorial")]
    [SerializeField] private GameObject tutorialScreen;
    bool endTutorial = false;

    //TIMER
    [Header("Timers")]
    [SerializeField] private float maxTime;
    private float gameTimer;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float maxEndTime;
    private float endTimer;


    //RING OBJECT AND DATA
    [Header("Rings prefabs and data")]
    [SerializeField] private RingObject goodRingScriptableObject;
    [SerializeField] private RingObject badRingScriptableObject;
    [SerializeField] private GameObject ringPrefab;

    //INPUT ACTIONS (CONTROLS)
    private PlayerInputActions inputActions;
    private Vector2 mousePos;
    private bool isClicking;

    //CURSOR
    [Header("Cursor")]
    [SerializeField] private GameObject cursorGameObject;
    [SerializeField] private RectTransform hotspotUI;
    [SerializeField] private Camera worldCamera;
    private float worldZ = 0f;

    //ANILLAS
    [Header("Ring Data")]
    [SerializeField] private RingObject ringData;
    private GameObject currentRing;
    [SerializeField] private float newRingDistance;

    //STATES
    private States currentState;

    //RAYCAST (anilla) a caja de anillas/basura
    [Header("Grabbed Data")]
    [SerializeField] private LayerMask pickableLayer;

    //INITIAL POSITIONS
    [SerializeField] private Transform startedArmorPos;

    //MATRIZ ANILLAS - grid
    [Header("Grid options")]
    [SerializeField] private GameObject ringContainer;
    [SerializeField] private int gridMaxRow;
    [SerializeField] private int gridMaxCol;
    private Rings[,] ringGrid;
    private bool armorStarted;

    //END
    [Header("Final Screen")]
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject loseScreen;
    private bool loseGame;
    private bool restart;

    //SCORE
    [Header("Score")]
    [SerializeField] private TMP_Text ringScoreText;
    private int ringScore;

    private void OnEnable()
    {
        inputActions.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isClicking = true;
        }
    }

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Cursor.SetCallbacks(this);
        Physics2D.queriesHitTriggers = true;
    }

    void Start()
    {
        currentState = States.Tutorial;
        ringGrid = new Rings[gridMaxRow, gridMaxCol];
        ringScore = gridMaxRow * gridMaxCol;
        gameTimer = maxTime;
        UpdateTimerText();
        UpdateScoreText();
        loseGame = false;
        endTimer = maxTime;
        restart = false;
        isClicking = false;
        armorStarted = false;
        musicAudioSource.ignoreListenerPause = true;
        musicAudioSource.clip = lagrimaMusic;
        musicAudioSource.Play();
    }

    void Update()
    {
        MinigameStatesFSM();
        MinigameHandlesFSM();
        //Debug.Log("MOUSE X, MOUSE Y: (" + mousePos.x + " | " + mousePos.y + ")");

        if (currentState == States.Playing)
        {
            if (isClicking)
            {
                isClicking = false;

                if (currentRing == null) DetectRingBoxHit();
                else DetectDropHit();
            }
        }
        else if (currentState == States.Tutorial)
        {
            if (isClicking)
            {
                isClicking = false;
                endTutorial = true;
            }
        }
    }

    void LateUpdate()
    {
        
    } 

    void MinigameStatesFSM()
    {
        switch(currentState)
        {
            case States.Tutorial:
                break;
            case States.Playing:
                Playing();
                break;
            case States.End:
                End();
                break;
        }
    }

    void MinigameHandlesFSM()
    {
        switch(currentState)
        {
            case States.Tutorial:
                TutorialHandle();
                break;
            case States.Playing:
                PlayingHandle();
                break;
            case States.End:
                EndHandle();
                break;
        }
    }

    void Playing()
    {
        FakeCursorMovement();
        if (currentRing != null) RingFollowMouse();
        GameTimer();
    }

    void PlayingHandle()
    {
        if (loseGame || ringScore <= 0) currentState = States.End;
    }

    void TutorialHandle()
    {
        if (endTutorial)
        {
            tutorialScreen.SetActive(false);
            //timerText.gameObject.SetActive(true);
            currentState = States.Playing;
        }
    }

    void End()
    {
        EndTimer();

        if (loseGame)
        {
            loseScreen.SetActive(true);
        }
        else
        {
            victoryScreen.SetActive(true);
            GameManager.Instance.minijuegoCompletado(3);
            Cursor.visible = true;
            SceneManager.LoadScene("Herreria", LoadSceneMode.Single);
        }
    }

    void EndHandle()
    {
        if (restart)
        {
            if (loseGame) RestartScene();
            else
            {
                GameManager.Instance.minijuegoCompletado(3);  //DESCOMENTAR
                SceneManager.LoadScene("Herreria", LoadSceneMode.Single);
            }
        }
    }

    void FakeCursorMovement()
    {
        cursorGameObject.transform.position = new Vector3 (mousePos.x, mousePos.y, cursorGameObject.transform.position.z);
    }

    void ConnectRing(Rings newRing, Collider2D hitHandle)
    {
        if (newRing.GetQuality() == RingQuality.Perfect) //conectar anilla, determinar donde
        {
            int r = 0;
            int c = 0;
            currentRing = null;
            newRing.transform.SetParent(ringContainer.transform);

            if (ringGrid[0, 0] == null) //primera anilla de la cota
            {
                ringGrid[0, 0] = newRing;
                newRing.SetColRowInGrid(r, c, gridMaxRow, gridMaxCol);
                Debug.Log("[ " + r + ", " + c + " ]");
                newRing.transform.position = startedArmorPos.position;
                armorStarted = true;
            }
            else //resto de anillas
            {
                Rings hittedRing = hitHandle.gameObject.GetComponentInParent<Rings>();

                string handleDirection = hittedRing.GetHandleHitDirection(hitHandle); //tomar la direcci�n del enganche

                List<int> hittedRingRowCol = hittedRing.GetColRow(); //tomar fila y columna del enganche
                int row = hittedRingRowCol[0];
                int col = hittedRingRowCol[1];

                List<int> newRingRowCol = GridPositionByAttachment(row, col, handleDirection); //calcular la posicion del nuevo anillo en el grid
                r = newRingRowCol[0];
                c = newRingRowCol[1];

                //evita errores de indexaci�n debido a malos disable de los handles
                if (r < 0 || r >= gridMaxRow || c < 0 || c >= gridMaxCol)
                {
                    return;
                }
                if (ringGrid[r, c] != null) return;

                ringGrid[r, c] = newRing;
                Debug.Log("[ " + r + ", " + c + " ]");

                newRing.SetColRowInGrid(r, c, gridMaxRow, gridMaxCol); //establecer la posicion de la nueva anilla en el grid

                hittedRing.DisableHandleByDirection(handleDirection); //desactivar ese enganche
                //newRing.DisableHandleByAttachment(handleDirection); //desactivar el enganche de la nueva anilla que se une al anterior
                List<bool> toBlockDirections = CheckNeighbours(r, c);
                newRing.DisableHandleByAttachment(toBlockDirections); //desactivar enganches de la nueva anilla en relacion a los vecinos

                //cambiar el order in layer de la anilla que entra
                ChangeRingOrder(newRing, hittedRingRowCol, r, c, hittedRing.GetOrderInLayer());

                newRing.transform.position = SetNewRingPos(handleDirection, hittedRing);
            }

            PlaySfx(Sfx.Attach); //sonido poner anilla
            ringScore--; //puntuacion
            UpdateScoreText();
        }
        else //intentando conectar una anilla mala
        {
            //Debug.Log("No se puede conectar una anilla rota");
        }
    }

    void ChangeRingOrder(Rings newRing, List<int> hittedRingColRow, int newRingRow, int newRingCol, int hittedRingOrder)
    {
        int hittedRow = hittedRingColRow[0];
        int baseOrder;
        int delta = (newRingRow > hittedRow) ? 1 : -1;

        if (newRingCol % 2 != 0)
        {
            baseOrder = 50;
            newRing.ChangeSpriteColor();
        }
        else
        {
            baseOrder = 100;
        }

        newRing.SetOrderInLayer(baseOrder + delta);
    }

    List<bool> CheckNeighbours(int newRingRow, int newRingCol)
    {
        //arriba abajo izquierda derecha
        List<bool> blockDirections = new List<bool>{ false, false, false, false };

        //arriba
        if (newRingRow - 1 >= 0 && ringGrid[newRingRow - 1, newRingCol] != null)
        {
            blockDirections[0] = true;
            ringGrid[newRingRow - 1, newRingCol].DisableHandleByDirection("down");
        }

        //abajo
        if (newRingRow + 1 < gridMaxRow && ringGrid[newRingRow + 1, newRingCol] != null)
        {
            blockDirections[1] = true;
            ringGrid[newRingRow + 1, newRingCol].DisableHandleByDirection("up");
        }

        //izquierda
        if (newRingCol - 1 >= 0 && ringGrid[newRingRow, newRingCol - 1] != null)
        {
            blockDirections[2] = true;
            ringGrid[newRingRow, newRingCol - 1].DisableHandleByDirection("right");
        }

        //derecha
        if (newRingCol + 1 < gridMaxCol && ringGrid[newRingRow, newRingCol + 1] != null)
        {
            blockDirections[3] = true;
            ringGrid[newRingRow, newRingCol + 1].DisableHandleByDirection("left");
        }

        return blockDirections;
    }

    Vector3 SetNewRingPos(string handleDirection, Rings hittedRing)
    {
        float x = 0;
        float y = 0;

        switch (handleDirection)
        {
            case "up":
                x = hittedRing.gameObject.transform.position.x;
                y = hittedRing.gameObject.transform.position.y + newRingDistance;
                break;
            case "down":
                x = hittedRing.gameObject.transform.position.x;
                y = hittedRing.gameObject.transform.position.y - newRingDistance;
                break;
            case "left":
                x = hittedRing.gameObject.transform.position.x - newRingDistance;
                y = hittedRing.gameObject.transform.position.y;
                break;
            case "right":
                x = hittedRing.gameObject.transform.position.x + newRingDistance;
                y = hittedRing.gameObject.transform.position.y;
                break;
        }

        return new Vector3(x, y, hittedRing.gameObject.transform.position.z);
    }

    List<int> GridPositionByAttachment(int hittedRow, int hittedCol, string handleDirection)
    {
        int row = 0;
        int col = 0;

        switch(handleDirection)
        {
            case "up":
                row = hittedRow - 1;
                col = hittedCol;
                break;
            case "down":
                row = hittedRow + 1;
                col = hittedCol;
                break;
            case "left":
                row = hittedRow;
                col = hittedCol - 1;
                break;
            case "right":
                row = hittedRow;
                col = hittedCol + 1;
                break;
        }

        return new List<int> { row, col };
    }

    void DetectRingBoxHit()
    {
        Vector3 position = HotspotScreenToWorld();
        Collider2D hit = Physics2D.OverlapPoint(position, pickableLayer);
        
        if (hit != null && hit.transform.CompareTag("Box"))
        {
            //Debug.Log("Coger anilla");
            RingGenerator();
        }
    }

    void DetectDropHit()
    {
        Vector2 position = HotspotScreenToWorld();

        if (!armorStarted)
        {
            Collider2D hit = Physics2D.OverlapPoint(position, pickableLayer);
            if (hit != null && hit.CompareTag("Board"))
            {
                ConnectRing(currentRing.GetComponent<Rings>(), hit);
            }
            else if (hit != null && hit.CompareTag("Trash"))
            {
                DropRing();
            }
            return;
        }

        float r = 0.12f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, r, pickableLayer);
        Collider2D best = null;
        //por si se detectan varios elementos con el mismo click, priorizar trash frente a attach
        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D h = hits[i];
            if (!h) continue;
            if (h.CompareTag("Trash")) { best = h; break; }
            if (best == null && h.CompareTag("Attachment")) best = h;
        }

        if (best == null) return;

        if (best.CompareTag("Trash")) DropRing();
        else if (best.CompareTag("Attachment")) ConnectRing(currentRing.GetComponent<Rings>(), best);
    }

    void DropRing()
    {
        Destroy(currentRing);
        currentRing = null;
        PlaySfx(Sfx.Trash);
    }

    void RingGenerator()
    {
        Vector3 position = HotspotScreenToWorld();
        currentRing = Instantiate(ringPrefab, position, Quaternion.identity);
        int r = Random.Range(1, 5);

        if (r == 4) //anilla rota
        {
            currentRing.GetComponent<Rings>().InitializationRing(badRingScriptableObject);
        }
        else
        {
            currentRing.GetComponent<Rings>().InitializationRing(goodRingScriptableObject);
        }

        currentRing.SetActive(true);
    }

    void RingFollowMouse()
    {
        Vector3 position = HotspotScreenToWorld();
        currentRing.transform.position = position;
    }

    //conversi�n coordenadas de mundo a pantalla (raton)
    Vector2 HotspotScreenToWorld()
    {
        if (!worldCamera) worldCamera = Camera.main;

        // hotspot a pantalla
        Vector2 screen = RectTransformUtility.WorldToScreenPoint(null, hotspotUI.position);

        // pantalla a mundo
        float zDist = worldZ - worldCamera.transform.position.z;
        Vector3 w = worldCamera.ScreenToWorldPoint(new Vector3(screen.x, screen.y, zDist));
        w.z = worldZ;

        return (Vector2)w;
    }

    void GameTimer()
    {
        if (gameTimer <= 0f)
            return;

        gameTimer -= Time.deltaTime;
        gameTimer = Mathf.Max(gameTimer, 0f);

        UpdateTimerText();

        if (gameTimer <= 0f) loseGame = true;    
    }

    void EndTimer()
    {
        if (endTimer <= 0f)
            return;

        endTimer -= Time.deltaTime;
        endTimer = Mathf.Max(endTimer, 0f);

        if (endTimer <= 0f) restart = true;
    }

    void UpdateTimerText()
    {
        timerText.text = Mathf.CeilToInt(gameTimer).ToString();
    }

    void UpdateScoreText()
    {
        ringScoreText.text = Mathf.CeilToInt(ringScore).ToString();
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void PlaySfx(Sfx sfx)
    {
        switch (sfx)
        {
            case Sfx.Trash: sfxAudioSource.PlayOneShot(trashSfx); break;
            case Sfx.Attach: sfxAudioSource.PlayOneShot(attachSfx); break;
        }
    }
}
