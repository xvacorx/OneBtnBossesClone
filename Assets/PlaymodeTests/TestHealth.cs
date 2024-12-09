using NUnit.Framework;
using UnityEngine;

public class PlayerHealthTests
{
    [Test]
    public void PlayerTakesDamageCorrectly()
    {
        // Arrange: Crear el objeto PlayerHealth y establecer la salud máxima
        var playerGameObject = new GameObject();
        var playerHealth = playerGameObject.AddComponent<PlayerHealth>();
        playerHealth.maxHealth = 3;

        // Act: Hacer que el jugador reciba daño
        playerHealth.TakeDamage(1);

        // Assert: Verificar que la salud actual es la esperada
        Assert.AreEqual(2, playerHealth.GetCurrentHealth(), "El jugador no recibió daño correctamente.");
    }
}
