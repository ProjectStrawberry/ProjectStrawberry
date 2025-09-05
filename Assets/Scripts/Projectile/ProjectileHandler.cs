using UnityEngine;
using UnityEngine.InputSystem.XR;
using Random = UnityEngine.Random;

public class ProjectileHandler : MonoBehaviour
{

    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;

    [SerializeField] private int bulletIndex;
    public int BulletIndex { get { return bulletIndex; } }

    [SerializeField] private float bulletSize = 1;
    public float BulletSize { get { return bulletSize; } }

    [SerializeField] private int power = 1;
    public int Power { get => power; set => power = value; }

    [SerializeField] private float speed = 1f;
    public float Speed { get => speed; set => speed = value; }

    [SerializeField] private float duration;
    public float Duration { get { return duration; } }

    [SerializeField] private float spread;
    public float Spread { get { return spread; } }

    [SerializeField] private int numberofProjectilesPerShot;
    public int NumberofProjectilesPerShot { get { return numberofProjectilesPerShot; } }

    [SerializeField] private float multipleProjectilesAngle;
    public float MultipleProjectilesAngel { get { return multipleProjectilesAngle; } }

    [SerializeField] private Color projectileColor;
    public Color ProjectileColor { get { return projectileColor; } }

    public AudioClip attackSoundClip;

    private ProjectileManager projectileManager;

    private StatHandler statHandler;

    public Transform target;

    protected void Start()
    {
        projectileManager = ProjectileManager.Instance;
        statHandler = GetComponentInParent<StatHandler>();
    }

    public void Attack()
    {
        float projectileAngleSpace = multipleProjectilesAngle;
        int numberOfProjectilePerShot = numberofProjectilesPerShot;

        float minAlge = -(numberOfProjectilePerShot / 2f) * projectileAngleSpace;

        for (int i = 0; i < numberOfProjectilePerShot; i++)
        {
            float angle = minAlge + projectileAngleSpace * i;
            float randomSpread = Random.Range(-spread, spread);
            angle += randomSpread;
            CreateProjectile(DirectionToTarget(target), angle);
        }
    }
    public void Attack(GameObject targetObj)
    {
         Transform target = targetObj.transform;
        float projectileAngleSpace = multipleProjectilesAngle;
        int numberOfProjectilePerShot = numberofProjectilesPerShot;

        float minAlge = -(numberOfProjectilePerShot / 2f) * projectileAngleSpace;

        for (int i = 0; i < numberOfProjectilePerShot; i++)
        {
            float angle = minAlge + projectileAngleSpace * i;
            float randomSpread = Random.Range(-spread, spread);
            angle += randomSpread;
            CreateProjectile(DirectionToTarget(target), angle);
        }
    }

    private void CreateProjectile(Vector2 _lookDirection, float angle)
    {
        projectileManager.ShootBullet(
            this,
            projectileSpawnPosition.position,
            RotateVector2(_lookDirection, angle));
    }

    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }

    protected Vector2 DirectionToTarget(Transform target)
    {
        return (target.position - transform.position).normalized;
    }
}
