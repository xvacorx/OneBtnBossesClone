using NUnit.Framework;
using UnityEngine;

public class TestEnemyDamage
{
    private GameObject enemyObject;
    private Enemy enemy;
    private GameObject projectile;

    private class GameControllerStub : MonoBehaviour
    {
        public static GameControllerStub Instance { get; set; }
        public void Victory() { }
    }

    [SetUp]
    public void SetUp()
    {
        GameControllerStub.Instance = new GameObject().AddComponent<GameControllerStub>();

        enemyObject = new GameObject();
        enemy = enemyObject.AddComponent<EnemyShooting>();

        enemyObject.GetComponent<EnemyShooting>().life = 3;

        projectile = new GameObject();
        projectile.AddComponent<BoxCollider2D>();
        projectile.tag = "PlayerProjectile";
    }

    [Test]
    public void TestTakeDamage()
    {
        Assert.AreEqual(3, enemyObject.GetComponent<Enemy>().life, "El enemigo debe comenzar con 3 de vida.");
        enemy.TakeDamage(1);

        Assert.AreEqual(2, enemyObject.GetComponent<Enemy>().life, "La vida del enemigo debe reducirse a 2.");

        enemy.TakeDamage(1);

        Assert.AreEqual(1, enemyObject.GetComponent<Enemy>().life, "La vida del enemigo debe reducirse a 1.");
        enemy.TakeDamage(1);

        Assert.AreEqual(0, enemyObject.GetComponent<Enemy>().life, "La vida del enemigo debe reducirse a 0.");
    }

    [Test]
    public void TestEnemyDeath()
    {
        enemyObject.GetComponent<Enemy>().life = 1;

        enemy.TakeDamage(1);

        Assert.AreEqual(0, enemyObject.GetComponent<Enemy>().life, "La vida del enemigo debe ser 0 al morir.");

    }
    [Test]
    public void TestOnTriggerEnter2D()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;

        var enemyCollider = enemyObject.AddComponent<BoxCollider2D>();
        var projectileCollider = projectile.AddComponent<BoxCollider2D>();

        var projectileRigidbody = projectile.AddComponent<Rigidbody2D>();
        projectileRigidbody.isKinematic = true;

        projectile.transform.position = enemyObject.transform.position;

        Physics2D.Simulate(Time.fixedDeltaTime);

        Assert.IsFalse(projectile.activeSelf, "El proyectil debe desactivarse después de la colisión.");

        Assert.AreEqual(2, enemyObject.GetComponent<Enemy>().life, "La vida del enemigo debe reducirse en 1 al recibir daño del proyectil.");

        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
    }




    [TearDown]
    public void TearDown()
    {
        if (enemyObject != null)
        {
            Object.DestroyImmediate(enemyObject);
        }

        if (projectile != null)
        {
            Object.DestroyImmediate(projectile);
        }

        if (GameControllerStub.Instance != null)
        {
            Object.DestroyImmediate(GameControllerStub.Instance.gameObject);
        }
    }

}
