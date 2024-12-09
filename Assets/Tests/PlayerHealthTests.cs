using NUnit.Framework; 
using UnityEngine;

public class PlayerHealthTests
{
    [Test]
    public void PlayerTakesDamageCorrectly()
    {
        var playerGameObject = new GameObject();
        var playerHealth = playerGameObject.AddComponent<PlayerHealth>();
        playerHealth.ResetHealth();

        playerHealth.TakeDamage(1);

        Assert.AreEqual(2, playerHealth.GetCurrentHealth());
    }
}
