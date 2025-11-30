using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LootChestInteractionTests
{
    private GameObject chestGameObject;
    private LootChestController lootChest;

    [SetUp]
    public void SetUp()
    {
        // Crear un nuevo cofre para cada test
        chestGameObject = new GameObject("TestLootChest");
        lootChest = chestGameObject.AddComponent<LootChestController>();
        lootChest.ResetForTesting();
    }

    [TearDown]
    public void TearDown()
    {
        // Limpiar después de cada test
        if (chestGameObject != null)
        {
            GameObject.DestroyImmediate(chestGameObject);
        }
    }

    // ✅ TEST ORIGINAL - Debe seguir pasando
    [UnityTest]
    public IEnumerator LootChest_Interact_OpensChestAndBecomesNonInteractable()
    {
        // ARRANGE
        Assert.IsFalse(lootChest.IsOpened, "El cofre debería empezar cerrado");
        Assert.IsFalse(lootChest.WasCostlyFunctionCalled(), "Función costosa no llamada al inicio");

        // ACT: Primera interacción
        lootChest.Interact();
        yield return null;

        // ASSERT
        Assert.IsTrue(lootChest.IsOpened, "El cofre debería estar abierto después de la primera interacción.");
        Assert.IsFalse(lootChest.WasCostlyFunctionCalled(), "Función costosa no debería llamarse en apertura normal");
    }

    // ✅ TEST ORIGINAL 
    [Test]
    public void LootChest_InitialState_IsClosed()
    {
        Assert.IsFalse(lootChest.IsOpened, "ERROR: Debería empezar CERRADO");
        Assert.IsFalse(lootChest.WasCostlyFunctionCalled(), "Función costosa no llamada en estado inicial");
    }

    // 🔥 NUEVO TEST CRÍTICO: Detección de Regresión
    [UnityTest]
    public IEnumerator LootChest_SecondInteraction_ShouldNotCallCostlyFunction()
    {
        // ARRANGE - Primera interacción normal
        lootChest.Interact();
        yield return null;
        
        bool costlyAfterFirst = lootChest.WasCostlyFunctionCalled();
        Assert.IsFalse(costlyAfterFirst, "Función costosa no debería llamarse en primera interacción");

        // ACT - Segunda interacción (NO debería ejecutar función costosa)
        lootChest.Interact();
        yield return null;

        // ASSERT - Detectar la regresión
        Assert.IsFalse(lootChest.WasCostlyFunctionCalled(),
            "❌ REGRESIÓN DETECTADA: La función costosa fue llamada en interacción redundante. " +
            "Esto impacta el rendimiento!");
    }

    // 🔥 TEST DE OPTIMIZACIÓN PREVENTIVA
    [Test]
    public void LootChest_MultipleInteractions_OnlyFirstOneMatters()
    {
        // ARRANGE
        int interactionCount = 0;
        System.Action onOpened = () => interactionCount++;
        
        // Simular evento (en una implementación real usarías el evento real)
        lootChest.Interact(); // Primera - debería contar como "abrir"
        bool firstState = lootChest.IsOpened;
        bool firstCostly = lootChest.WasCostlyFunctionCalled();

        lootChest.Interact(); // Segunda - NO debería hacer nada
        bool secondState = lootChest.IsOpened;
        bool secondCostly = lootChest.WasCostlyFunctionCalled();

        // ASSERT
        Assert.IsTrue(firstState, "Debe abrirse en primera interacción");
        Assert.IsTrue(secondState, "Debe permanecer abierto");
        
        // 🔥 VERIFICACIÓN DE OPTIMIZACIÓN CRÍTICA
        Assert.IsFalse(firstCostly, "Función costosa no llamada en interacción válida");
        Assert.IsFalse(secondCostly, "Función costosa no llamada en interacción redundante");
        
        Debug.Log("✅ Optimización verificada: Funciones costosas no se ejecutan innecesariamente");
    }

    // ✅ TEST ORIGINAL MANTENIDO  
    [Test] 
    public void LootChest_StaysOpen_OnMultipleInteractions()
    {
        // Arrange
        lootChest.Interact();
        bool firstState = lootChest.IsOpened;
        bool firstCostly = lootChest.WasCostlyFunctionCalled();
        
        lootChest.Interact();
        bool secondState = lootChest.IsOpened;
        bool secondCostly = lootChest.WasCostlyFunctionCalled();
        
        // Assert
        Assert.IsTrue(firstState, "Debería abrirse en primera interacción");
        Assert.IsTrue(secondState, "Debería permanecer abierto");
        Assert.AreEqual(firstState, secondState, "El estado no debería cambiar");
        
        // Nueva verificación de optimización
        Assert.IsFalse(firstCostly, "Función costosa no llamada en apertura normal");
        Assert.IsFalse(secondCostly, "Función costosa no llamada en interacción redundante");
    }
}