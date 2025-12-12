using UnityEngine;

public class WalkingScenario : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    
    // Kita butuh referensi langsung ke objek fisik karena script ada di Parent
    [SerializeField] public Collider2D targetZone; // Masukkan object 'Square' ke sini
    [SerializeField] public Collider2D playerCollider; // Masukkan object 'Player' ke sini

    [Header("Settings")]
    [SerializeField] private float requiredStayTime = 3f;

    [Header("Debug Info")] // Supaya bisa dilihat di inspector
    public float stayTimer = 0f;
    private bool completed = false;

    void Start()
    {
        if (gameManager == null)
        {
            gameManager = Object.FindFirstObjectByType<GameManager>();
        }

        // Validasi agar tidak error
        if (targetZone == null || playerCollider == null)
        {
            Debug.LogError("Target Zone (Square) atau Player belum dimasukkan ke inspector WalkingScenario!");
            enabled = false;
        }
    }

    void Update()
    {
        if (completed) return;

        // Cek apakah Player bersentuhan dengan Target Zone (Square)
        // IsTouching memerlukan Rigidbody2D pada salah satu objek (biasanya Player)
        if (targetZone.IsTouching(playerCollider))
        {
            HandleStay();
        }
        else
        {
            HandleExit();
        }
    }

    private void HandleStay()
    {
        stayTimer += Time.deltaTime;
        // Debug.Log("Timer berjalan: " + stayTimer); // Uncomment jika ingin debug

        if (stayTimer >= requiredStayTime)
        {
            CompleteScenario();
        }
    }

    private void HandleExit()
    {
        if (stayTimer > 0)
        {
            stayTimer = 0f;
            Debug.Log("Keluar zona, timer reset");
        }
    }

    private void CompleteScenario()
    {
        completed = true;
        Debug.Log("Scenario Selesai!");
        this.gameObject.SetActive(false);
        if (gameManager != null)
        {
            gameManager.OnMiniGameComplete("Player reach target");
        }
    }
}