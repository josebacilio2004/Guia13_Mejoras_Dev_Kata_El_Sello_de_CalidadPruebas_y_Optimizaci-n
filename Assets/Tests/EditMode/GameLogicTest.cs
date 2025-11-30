using NUnit.Framework;
using UnityEngine;

public class GameLogicTests
{
    private GameLogic gameLogic;

    [SetUp]
    public void Setup()
    {
        // Este método se ejecuta antes de cada test
    }

    [TearDown]
    public void Teardown()
    {
        // Este método se ejecuta después de cada test
        gameLogic = null;
    }

    // PRUEBAS DE FUNCIONALIDAD NORMAL

    [Test]
    public void CompleteObjective_IncrementsCounter_WhenUnderWinCondition()
    {
        // Arrange
        gameLogic = new GameLogic(3);
        int initialCount = gameLogic.ObjectivesCompleted;

        // Act
        gameLogic.CompleteObjective();

        // Assert
        Assert.AreEqual(initialCount + 1, gameLogic.ObjectivesCompleted, 
            "El contador de objetivos debería incrementarse en 1");
        Assert.IsFalse(gameLogic.IsVictoryConditionMet, 
            "No debería cumplirse la condición de victoria todavía");
    }

    [Test]
    public void IsVictoryConditionMet_ReturnsTrue_WhenObjectivesCompletedEqualsObjectivesToWin()
    {
        // Arrange
        gameLogic = new GameLogic(3);
        gameLogic.SetObjectivesCompleted(3);

        // Act
        bool victoryConditionMet = gameLogic.IsVictoryConditionMet;

        // Assert
        Assert.IsTrue(victoryConditionMet, 
            "Debería cumplirse la condición de victoria cuando objetivos completados = objetivos requeridos");
    }

    [Test]
    public void CompleteObjective_DoesNotIncrement_WhenVictoryConditionAlreadyMet()
    {
        // Arrange
        gameLogic = new GameLogic(2);
        gameLogic.SetObjectivesCompleted(2); // Ya se cumplió la victoria
        int initialCount = gameLogic.ObjectivesCompleted;

        // Act
        gameLogic.CompleteObjective();

        // Assert
        Assert.AreEqual(initialCount, gameLogic.ObjectivesCompleted, 
            "El contador no debería incrementarse después de alcanzar la victoria");
        Assert.IsTrue(gameLogic.IsVictoryConditionMet, 
            "La condición de victoria debería mantenerse como verdadera");
    }

    // PRUEBAS DE CASOS EXTREMOS

    [Test]
    public void Constructor_WithZeroObjectivesToWin_SetsToOne()
    {
        // Arrange & Act - Caso extremo: 0 objetivos para ganar
        gameLogic = new GameLogic(0);

        // Assert
        Assert.AreEqual(1, gameLogic.ObjectivesToWin, 
            "Debería establecer 1 objetivo cuando se pasa 0 como parámetro");
        Assert.AreEqual(0, gameLogic.ObjectivesCompleted, 
            "El contador de objetivos completados debería empezar en 0");
    }

    [Test]
    public void Constructor_WithNegativeObjectivesToWin_SetsToOne()
    {
        // Arrange & Act - Caso extremo: número negativo de objetivos para ganar
        gameLogic = new GameLogic(-5);

        // Assert
        Assert.AreEqual(1, gameLogic.ObjectivesToWin, 
            "Debería establecer 1 objetivo cuando se pasa un número negativo como parámetro");
        Assert.AreEqual(0, gameLogic.ObjectivesCompleted, 
            "El contador de objetivos completados debería empezar en 0");
    }

    [Test]
    public void CompleteObjective_MultipleCallsAfterVictory_StopsIncrementing()
    {
        // Arrange
        gameLogic = new GameLogic(2);
        
        // Act - Completar exactamente los objetivos necesarios y luego intentar más
        gameLogic.CompleteObjective(); // 1 objetivo
        gameLogic.CompleteObjective(); // 2 objetivos - VICTORIA
        gameLogic.CompleteObjective(); // Intentar completar después de victoria
        gameLogic.CompleteObjective(); // Intentar completar después de victoria

        // Assert
        Assert.AreEqual(2, gameLogic.ObjectivesCompleted, 
            "El contador debería detenerse en el número de objetivos requeridos para ganar");
        Assert.IsTrue(gameLogic.IsVictoryConditionMet, 
            "La condición de victoria debería mantenerse como verdadera");
    }

    [Test]
    public void IsVictoryConditionMet_ReturnsTrue_WhenObjectivesCompletedExceedsObjectivesToWin()
    {
        // Arrange
        gameLogic = new GameLogic(2);
        gameLogic.SetObjectivesCompleted(5); // Más objetivos de los necesarios

        // Act
        bool victoryConditionMet = gameLogic.IsVictoryConditionMet;

        // Assert
        Assert.IsTrue(victoryConditionMet, 
            "Debería cumplirse la condición de victoria cuando objetivos completados > objetivos requeridos");
    }

    [Test]
    public void CompleteObjective_BoundaryCondition_AtExactWinCondition()
    {
        // Arrange
        gameLogic = new GameLogic(3);
        gameLogic.SetObjectivesCompleted(2); // Un objetivo antes de la victoria

        // Act
        gameLogic.CompleteObjective(); // Tercer objetivo - victoria exacta

        // Assert
        Assert.AreEqual(3, gameLogic.ObjectivesCompleted, 
            "Debería incrementar hasta el número exacto de objetivos requeridos");
        Assert.IsTrue(gameLogic.IsVictoryConditionMet, 
            "Debería cumplirse la condición de victoria en el límite exacto");
    }

    // PRUEBAS ADICIONALES

    [Test]
    public void ResetObjectives_SetsCompletedToZero()
    {
        // Arrange
        gameLogic = new GameLogic(5);
        gameLogic.SetObjectivesCompleted(3);

        // Act
        gameLogic.ResetObjectives();

        // Assert
        Assert.AreEqual(0, gameLogic.ObjectivesCompleted, 
            "El reset debería establecer los objetivos completados a 0");
        Assert.IsFalse(gameLogic.IsVictoryConditionMet, 
            "Después del reset, no debería cumplirse la condición de victoria");
    }

    [Test]
    public void SetObjectivesCompleted_ClampsNegativeValuesToZero()
    {
        // Arrange
        gameLogic = new GameLogic(3);

        // Act
        gameLogic.SetObjectivesCompleted(-5);

        // Assert
        Assert.AreEqual(0, gameLogic.ObjectivesCompleted, 
            "Debería clamp valores negativos a 0");
    }

    [Test]
    public void SetObjectivesCompleted_AllowsLargePositiveValues()
    {
        // Arrange
        gameLogic = new GameLogic(3);

        // Act
        gameLogic.SetObjectivesCompleted(1000);

        // Assert
        Assert.AreEqual(1000, gameLogic.ObjectivesCompleted, 
            "Debería permitir valores positivos grandes");
        Assert.IsTrue(gameLogic.IsVictoryConditionMet, 
            "Debería cumplirse la condición de victoria con muchos objetivos completados");
    }

    [Test]
    public void DefaultConstructor_SetsOneObjectiveToWin()
    {
        // Arrange & Act
        gameLogic = new GameLogic();

        // Assert
        Assert.AreEqual(1, gameLogic.ObjectivesToWin, 
            "El constructor por defecto debería establecer 1 objetivo para ganar");
        Assert.AreEqual(0, gameLogic.ObjectivesCompleted, 
            "El constructor por defecto debería empezar con 0 objetivos completados");
    }

    [Test]
    public void MultipleCompleteObjective_Calls_IncrementCorrectly()
    {
        // Arrange
        gameLogic = new GameLogic(5);

        // Act
        for (int i = 0; i < 3; i++)
        {
            gameLogic.CompleteObjective();
        }

        // Assert
        Assert.AreEqual(3, gameLogic.ObjectivesCompleted, 
            "Múltiples llamadas deberían incrementar el contador correctamente");
        Assert.IsFalse(gameLogic.IsVictoryConditionMet, 
            "No debería cumplirse la victoria antes de alcanzar todos los objetivos");
    }

    [Test]
    public void VictoryCondition_EdgeCase_SingleObjective()
    {
        // Arrange
        gameLogic = new GameLogic(1);

        // Act
        gameLogic.CompleteObjective();

        // Assert
        Assert.AreEqual(1, gameLogic.ObjectivesCompleted, 
            "Debería completar el único objetivo requerido");
        Assert.IsTrue(gameLogic.IsVictoryConditionMet, 
            "Debería cumplirse la victoria con un solo objetivo");
    }
}