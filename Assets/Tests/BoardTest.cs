using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.TestTools;



public class BoardTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void BoardTestSimplePasses()
    {
        Board board = new Board(3,1);  




        // Use the Assert class to test conditions
        // Assert.Equals(board.GetDims(),  (3,1));
        // Assert.Equals(board.Get(1,1),  0);

        // board.
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator BoardTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
