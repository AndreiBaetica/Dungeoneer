using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EditTestSuite
    {
        
        [Test]
        public void GameObject_CreatedWithGiven_WillHaveTheName()
        {
            var go = new GameObject("MyGameObject");
            Assert.AreEqual("MyGameObject", go.name);
        }
        
        // [Test]
        // public void TestHPDrainAndIncrease() 
        // {
        //     var playerController = new GameObject().AddComponent<PlayerController>();
        //     playerController.MaxHealth = 100;
        //     playerController.CurrentHealth = 100;
        //     playerController.enabled;
        //     
        // }
        
        
        // A Test behaves as an ordinary method
        [Test]
        public void EditTestSuiteSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator EditTestSuiteWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
