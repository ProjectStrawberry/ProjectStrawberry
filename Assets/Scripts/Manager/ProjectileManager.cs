using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private static ProjectileManager instance;
    public static ProjectileManager Instance { get { return instance; } }

    [SerializeField] private GameObject[] projectilePrefabs;


    ObjectPoolManager objectPoolManager;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        objectPoolManager = ObjectPoolManager.Instance;
    }

    public void ShootBullet(ProjectileHandler projectileHandler, Vector2 startPosition, Vector2 direction)
    {
        GameObject obj = objectPoolManager.GetObject(projectileHandler.BulletIndex, startPosition, Quaternion.identity);

        Projectile projectile = obj.GetComponent<Projectile>();
        projectile.Init(direction, projectileHandler, this);
    }
}