using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    [Header("Visuals")]
    public Sprite idleSprite;
    public RuntimeAnimatorController hitAnimatorController;
    public Sprite deadSprite;

    [Header("Attack Animation")]
    public RuntimeAnimatorController attackAnimatorController;
    public float attackAnimLength = 0.5f; // Set this to your attack animation length

    [Header("Scale Animation")]
    public Vector3 hitScale = new Vector3(1.2f, 1.2f, 1f);
    public Vector3 idleScale = Vector3.one;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Enemy enemy;

    private bool isHit = false;
    private float hitAnimLength = 0f;
    private float hitTimer = 0f;

    private bool isAttacking = false;
    private float attackTimer = 0f;

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

        // Get attack animation length if controller is set
        if (attackAnimatorController != null)
        {
            AnimationClip[] attackClips = attackAnimatorController.animationClips;
            if (attackClips.Length > 0)
                attackAnimLength = attackClips[0].length;
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

        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackAnimLength)
            {
                ResetToIdle();
            }
        }

        // Can 0 veya altına düştüyse ölüm animasyonunu otomatik oynat
        if (enemy != null && enemy.currentHealth <= 0 && spriteRenderer.sprite != deadSprite)
        {
            PlayDeadAnimation();
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

    public void PlayAttackAnimation()
    {
        if (animator == null || attackAnimatorController == null)
            return;

        isAttacking = true;
        attackTimer = 0f;

        animator.runtimeAnimatorController = attackAnimatorController;
        animator.enabled = true;
        animator.Play("Attack", 0, 0f); // animasyonu sıfırdan başlat

        // İsterseniz scale efektini de ekleyebilirsiniz, ör: transform.localScale = hitScale;
        // Ancak hit ile aynı davranış isteniyorsa:
        transform.localScale = hitScale;
    }

    public void PlayDeadAnimation()
    {
        // Ölüm animasyonu: sprite'ı deadSprite yap, scale sıfırla, animator'ı kapat
        if (animator != null)
            animator.enabled = false;

        if (spriteRenderer != null && deadSprite != null)
            spriteRenderer.sprite = deadSprite;

        transform.localScale = idleScale;
        isHit = false;
        hitTimer = 0f;
    }

    void ResetToIdle()
    {
        // Eğer düşman öldüyse idle'a dönme
        if (enemy != null && enemy.currentHealth <= 0)
            return;

        isHit = false;
        isAttacking = false;
        hitTimer = 0f;
        attackTimer = 0f;

        if (animator != null)
            animator.enabled = false;

        if (spriteRenderer != null && idleSprite != null)
            spriteRenderer.sprite = idleSprite;

        transform.localScale = idleScale;
    }
}
