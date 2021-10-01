using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

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
        
        
        //test
        [Test]
         public void TestHpDrainAndIncrease()
         {
             var pc = new GameObject();
             pc.AddComponent<PlayerController>();

             pc.GetComponent<PlayerController>().healthBar = pc.AddComponent<HealthBar>();
             pc.GetComponent<PlayerController>().MaxHealth = 100;
             pc.GetComponent<PlayerController>().CurrentHealth = 50;

             
             pc.GetComponent<HealthBar>().slider = pc.AddComponent<Slider>();
             pc.GetComponent<HealthBar>().fill = pc.AddComponent<Image>();
             pc.GetComponent<HealthBar>().gradient = new Gradient();
             
             pc.GetComponent<HealthBar>().SetMaxHealth(100);
             pc.GetComponent<HealthBar>().SetHealth(50);
             
             
             pc.GetComponent<PlayerController>().Heal(10);
             
             Assert.AreEqual(60, pc.GetComponent<PlayerController>().CurrentHealth);
         }
        
        
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
