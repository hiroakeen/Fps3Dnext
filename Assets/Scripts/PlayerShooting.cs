using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject defaultArrow;
    [SerializeField] private Transform arrowPosition;
    [SerializeField] private float shootPower = 10f;
    [SerializeField] private float shootCooldown = 1.0f;
    [SerializeField] private Transform cameraTransform;

    private bool canShoot = true;
    private GameObject newBullet;

    private PlayerInputHandler inputHandler;
    private PlayerAnimation anim;
    private PlayerAudio audioPlayer;

    void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        anim = GetComponent<PlayerAnimation>();
        audioPlayer = GetComponent<PlayerAudio>();
    }

    void Update()
    {
        if (inputHandler.IsRightClickHeld)
        {
            if (inputHandler.IsRightClickDown)
            {
                audioPlayer.PlayStringSound();
            }

            anim.SetAttackState(true);

            if (inputHandler.IsLeftClickDown && canShoot)
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

        newBullet = Instantiate(arrowPrefab, arrowPosition.position, transform.rotation);
        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        rb.AddForce(cameraTransform.forward * shootPower, ForceMode.Impulse);

        audioPlayer.PlayBowShot();

        Destroy(newBullet, 10);
        StartCoroutine(ArrowCooldown());
    }

    IEnumerator ArrowCooldown()
    {
        canShoot = false;
        defaultArrow.SetActive(false);
        yield return new WaitForSeconds(shootCooldown);
        defaultArrow.SetActive(true);
        canShoot = true;
    }
}
