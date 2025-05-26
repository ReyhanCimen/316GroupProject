using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    [Header("Visuals")]
    public Sprite idleSprite;
    public RuntimeAnimatorController hitAnimatorController;

    [Header("Scale Animation")]
    public Vector3 hitScale = new Vector3(1.2f, 1.2f, 1f);
    public Vector3 idleScale = Vector3.one;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Enemy enemy;

    private bool isHit = false;
    private float hitAnimLength = 0f;
    private float hitTimer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemy = GetComponentInParent<Enemy>();

        if (spriteRenderer != null && idleSprite != null)
            spriteRenderer.sprite = idleSprite;

        if (animator != null && hitAnimatorController != null)
        {
            animator.runtimeAnimatorController = hitAnimatorController;
            animator.enabled = false;

            AnimationClip[] clips = hitAnimatorController.animationClips;
            if (clips.Length > 0)
                hitAnimLength = clips[0].length;
        }
    }

    void Update()
    {
        if (enemy != null && enemy.isEnemyHit && !isHit)
        {
            PlayHitAnimation();
        }

        if (isHit)
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= hitAnimLength)
            {
                ResetToIdle();
            }
        }
    }

    public void PlayHitAnimation()
    {
        if (animator == null || hitAnimatorController == null)
            return;

        isHit = true;
        hitTimer = 0f;

        if (enemy != null)
            enemy.isEnemyHit = false;

        animator.enabled = true;
        animator.Play("Hit", 0, 0f);

        transform.localScale = hitScale;
    }

    void ResetToIdle()
    {
        isHit = false;
        hitTimer = 0f;

        if (animator != null)
            animator.enabled = false;

        if (spriteRenderer != null && idleSprite != null)
            spriteRenderer.sprite = idleSprite;

        transform.localScale = idleScale;
    }
}
