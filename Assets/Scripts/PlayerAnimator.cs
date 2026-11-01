using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("References (auto-found if empty)")]
    public PlayerController controller;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Transform visual;
    public Animator idleAnimator;

    [Header("Sprite")]
    public bool faceRightByDefault = true;

    [Header("Idle")]
    public float breatheSpeed = 2f;
    public float breatheAmount = 0f;
    public float maxIdleSpeed = 2f;

    [Header("Walk")]
    public float bobSpeed = 14f;
    public float bobHeight = 0.08f;
    public float squashAmount = 0.12f;
    public float maxTilt = 8f;
    public float smooth = 12f;

    [Header("Attack Punch")]
    public float punchAmount = 0.2f;
    public float punchRecover = 10f;

    [Header("Dust")]
    public Material dustMaterial;
    public Vector2 feetOffset = new Vector2(0f, -0.45f);
    public Color dustColor = new Color(0.55f, 0.65f, 0.8f, 0.6f);
    public float dustPerUnit = 6f;
    public float dustSize = 0.14f;
    public int turnBurst = 6;
    public float minTurnSpeed = 2f;

    [Header("Trail")]
    public bool useTrail = true;
    public Material trailMaterial;
    public Vector2 trailOffset = Vector2.zero;
    public Color trailColor = new Color(0.23f, 0.44f, 0.84f, 0.5f);
    public float trailTime = 0.25f;
    public float trailWidth = 0.5f;
    public float trailMinSpeed = 3.5f;

    private Vector3 baseScale;
    private Vector3 basePos;
    private float punch;
    private float lastSign;
    private ParticleSystem dust;
    private TrailRenderer trail;

    private void Awake()
    {
        if (controller == null) controller = GetComponent<PlayerController>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
        if (visual == null) visual = sr.transform;

        baseScale = visual.localScale;
        basePos = visual.localPosition;

        BuildDust();

        if (useTrail)
        {
            BuildTrail();
        }
    }

    private void Update()
    {
        HandleFlip();
        HandleSquashAndTilt();
        HandleDustAndTrail();
    }

    private void HandleFlip()
    {
        if (controller == null)
        {
            return;
        }

        if (controller.MoveX > 0f)
        {
            sr.flipX = !faceRightByDefault;
        }
        else if (controller.MoveX < 0f)
        {
            sr.flipX = faceRightByDefault;
        }
    }

    private void HandleSquashAndTilt()
    {
        float velX = rb.linearVelocity.x;
        float moving = Mathf.Clamp01(Mathf.Abs(velX) / 3f);
        float dir = Mathf.Sign(velX);
        float phase = Time.time * bobSpeed;

        if (idleAnimator != null)
        {
            idleAnimator.speed = Mathf.Lerp(1f, maxIdleSpeed, moving);
        }

        float bob = Mathf.Abs(Mathf.Sin(phase)) * bobHeight * moving;
        float walkSquash = Mathf.Cos(phase * 2f) * squashAmount * moving;
        float breathe = Mathf.Sin(Time.time * breatheSpeed) * breatheAmount * (1f - moving);

        punch = Mathf.Lerp(punch, 0f, punchRecover * Time.deltaTime);

        float s = walkSquash + breathe + punch;
        Vector3 targetScale = new Vector3(baseScale.x * (1f + s), baseScale.y * (1f - s), baseScale.z);
        Vector3 targetPos = basePos + Vector3.up * bob;
        float targetTilt = -dir * maxTilt * moving;

        float t = smooth * Time.deltaTime;
        visual.localScale = Vector3.Lerp(visual.localScale, targetScale, t);
        visual.localPosition = Vector3.Lerp(visual.localPosition, targetPos, t);

        float angle = Mathf.LerpAngle(visual.localEulerAngles.z, targetTilt, t);
        visual.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void HandleDustAndTrail()
    {
        float vx = rb.linearVelocity.x;
        float speed = Mathf.Abs(vx);
        float sign = speed > minTurnSpeed ? Mathf.Sign(vx) : 0f;

        if (sign != 0f && lastSign != 0f && sign != lastSign)
        {
            dust.Emit(turnBurst);
        }

        if (sign != 0f)
        {
            lastSign = sign;
        }

        if (trail != null)
        {
            trail.emitting = speed > trailMinSpeed;
        }
    }

    public void Punch()
    {
        punch = punchAmount;
    }

    private void BuildDust()
    {
        GameObject go = new GameObject("Dust");
        go.transform.SetParent(transform, false);
        go.transform.localPosition = feetOffset;

        dust = go.AddComponent<ParticleSystem>();
        dust.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = dust.main;
        main.loop = true;
        main.playOnAwake = false;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.3f, 0.6f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.2f, 0.8f);
        main.startSize = new ParticleSystem.MinMaxCurve(dustSize * 0.6f, dustSize);
        main.startColor = dustColor;
        main.gravityModifier = -0.1f;
        main.maxParticles = 150;

        var emission = dust.emission;
        emission.rateOverTime = 0f;
        emission.rateOverDistance = dustPerUnit;

        var shape = dust.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.08f;

        var col = dust.colorOverLifetime;
        col.enabled = true;
        Gradient fade = new Gradient();
        fade.SetKeys(
            new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) });
        col.color = fade;

        var size = dust.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0f));

        ParticleSystemRenderer pr = go.GetComponent<ParticleSystemRenderer>();

        if (dustMaterial != null)
        {
            pr.material = dustMaterial;
        }
        else
        {
            Material mat = new Material(sr.sharedMaterial);
            mat.mainTexture = MakeSoftCircle(32);
            pr.material = mat;
        }

        pr.sortingLayerID = sr.sortingLayerID;
        pr.sortingOrder = sr.sortingOrder - 1;

        dust.Play();
    }

    private void BuildTrail()
    {
        GameObject go = new GameObject("Trail");
        go.transform.SetParent(transform, false);
        go.transform.localPosition = trailOffset;

        trail = go.AddComponent<TrailRenderer>();
        trail.time = trailTime;
        trail.minVertexDistance = 0.05f;
        trail.emitting = false;
        trail.widthCurve = new AnimationCurve(new Keyframe(0f, trailWidth), new Keyframe(1f, 0f));

        Gradient g = new Gradient();
        g.SetKeys(
            new[] { new GradientColorKey(trailColor, 0f), new GradientColorKey(trailColor, 1f) },
            new[] { new GradientAlphaKey(trailColor.a, 0f), new GradientAlphaKey(0f, 1f) });
        trail.colorGradient = g;

        trail.material = trailMaterial != null ? trailMaterial : new Material(sr.sharedMaterial);
        trail.sortingLayerID = sr.sortingLayerID;
        trail.sortingOrder = sr.sortingOrder - 2;
    }

    private static Texture2D MakeSoftCircle(int size)
    {
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;
        float r = size / 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float d = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(r, r)) / r;
                float a = Mathf.SmoothStep(1f, 0f, d);
                tex.SetPixel(x, y, new Color(1f, 1f, 1f, a));
            }
        }

        tex.Apply();
        return tex;
    }
}