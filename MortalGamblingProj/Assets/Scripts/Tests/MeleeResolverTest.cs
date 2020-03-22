using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


namespace Tests
{
    public class MeleeResolverTest
    {

        // private TurnManager manager; WHY DOESNT THIS WORK
        [SetUp]
        public void Setup()
        {
             
        }
        // A Test behaves as an ordinary method
        [Test]
        public void MeleeResolverTestSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator MeleeResolverTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
