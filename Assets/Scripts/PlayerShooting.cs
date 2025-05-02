using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float shootPower = 50f;
    [SerializeField] private float shootCooldown = 1.0f;
    [SerializeField] private Transform arrowPosition; // 弓の先
    [SerializeField] private Image reticleUI; // レティクルのRectTransform
    [SerializeField] private Camera mainCamera; // レンダリング用のカメラ

    private bool canShoot = true;
    private PlayerInputHandler inputHandler;
    private PlayerAnimation anim;
    private PlayerAudio audioPlayer;

    public bool IsDrawing { get; private set; }
    void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        anim = GetComponent<PlayerAnimation>();
        audioPlayer = GetComponent<PlayerAudio>();

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        IsDrawing = inputHandler.IsRightClickHeld;

        if (inputHandler.IsRightClickHeld)
        {
            if (inputHandler.IsRightClickDown)
            {
                audioPlayer.PlayStringSound();
            }

            anim.SetAttackState(true);

            if (inputHandler.IsLeftClickDown && canShoot && GameManager.Instance.IsGameStarted)
            {
                Shoot();
            }
        }
        else
        {
            anim.SetAttackState(false);
        }
    }

    void Shoot()
    {
        anim.TriggerArrowAttack();
        audioPlayer.PlayBowShot();

        // ✅ RectTransform からスクリーン座標を取得
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, reticleUI.rectTransform.position);

        // ✅ 画面上のその位置からRayを作成
        Ray ray = mainCamera.ScreenPointToRay(screenPoint);
        Vector3 shootDirection = ray.direction;

        Vector3 spawnPos = arrowPosition.position;
        Quaternion rotation = Quaternion.LookRotation(shootDirection);

        GameObject arrow = Instantiate(arrowPrefab, spawnPos, rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.linearVelocity = shootDirection * shootPower;

        Destroy(arrow, 10f);
        StartCoroutine(ArrowCooldown());

        // デバッグ表示
        Debug.DrawRay(ray.origin, shootDirection * 5f, Color.red, 2f);
        Debug.DrawRay(spawnPos, shootDirection * 5f, Color.cyan, 2f);
    }

    IEnumerator ArrowCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
