using System;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using Managers;
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
        
         
         //test rooms
         [Test]
         public void TestRoomAllDoors()
         {
             var room = new Room(RoomType.YYYY, (1,1));
             
             Assert.AreEqual(RoomType.YYYY, room.GetRoomType());
             Assert.AreEqual((1,1), room.getPosition());
             
             Assert.True(room.HasNorthConnection());
             Assert.True(room.HasEastConnection());
             Assert.True(room.HasSouthConnection());
             Assert.True(room.HasWestConnection());
         }
         
         [Test]
         public void TestRoomNorthDoor()
         {
             var room = new Room(RoomType.YNNN, (1,1));
             
             Assert.AreEqual(RoomType.YNNN, room.GetRoomType());
             Assert.AreEqual((1,1), room.getPosition());
             
             Assert.True(room.HasNorthConnection());
             Assert.False(room.HasEastConnection());
             Assert.False(room.HasSouthConnection());
             Assert.False(room.HasWestConnection());
         }
         
         [Test]
         public void TestRoomEastDoor()
         {
             var room = new Room(RoomType.NYNN, (1,1));
             
             Assert.AreEqual(RoomType.NYNN, room.GetRoomType());
             Assert.AreEqual((1,1), room.getPosition());
             
             Assert.False(room.HasNorthConnection());
             Assert.True(room.HasEastConnection());
             Assert.False(room.HasSouthConnection());
             Assert.False(room.HasWestConnection());
         }
         
         [Test]
         public void TestRoomSouthDoor()
         {
             var room = new Room(RoomType.NNYN, (1,1));
             
             Assert.AreEqual(RoomType.NNYN, room.GetRoomType());
             Assert.AreEqual((1,1), room.getPosition());
             
             Assert.False(room.HasNorthConnection());
             Assert.False(room.HasEastConnection());
             Assert.True(room.HasSouthConnection());
             Assert.False(room.HasWestConnection());
         }
         
         
         [Test]
         public void TestRoomWestDoor()
         {
             var room = new Room(RoomType.NNNY, (1,1));
             
             Assert.AreEqual(RoomType.NNNY, room.GetRoomType());
             Assert.AreEqual((1,1), room.getPosition());
             
             Assert.False(room.HasNorthConnection());
             Assert.False(room.HasEastConnection());
             Assert.False(room.HasSouthConnection());
             Assert.True(room.HasWestConnection());
         }
         
         //test grid system
         [Test]
         public void TestGridSystemCreation()
         {

             GridSystem<int> gridSys = new GridSystem<int>(10, 20, 1, Vector3.zero, delegate(GridSystem<int> system, int i, int arg3)
             {
                 return 0;
             });
             
             Assert.AreEqual(10, gridSys.GetWidth());
             Assert.AreEqual(20, gridSys.GetHeight());
             Assert.AreEqual(1, gridSys.GetCellSize());
         }
         
         [Test]
         public void TestGridSystemWorldPosition()
         {

             GridSystem<int> gridSys = new GridSystem<int>(10, 20, 1, Vector3.zero, delegate(GridSystem<int> system, int i, int arg3)
             {
                 return 0;
             });
             
             Assert.AreEqual(new Vector3(2, 0, 2), gridSys.GetWorldPosition(2,2));
             Assert.AreEqual(new Vector3(4, 0, 6), gridSys.GetWorldPosition(4, 6));
             Assert.AreEqual(new Vector3(10, 0, 20), gridSys.GetWorldPosition(10, 20));
         }
         
         [Test]
         public void TestGridSystemWorldPositionNegative()
         {

             GridSystem<int> gridSys = new GridSystem<int>(10, 20, 1, Vector3.zero, delegate(GridSystem<int> system, int i, int arg3)
             {
                 return 0;
             });
             
             Assert.AreEqual(new Vector3(-2, 0, -2), gridSys.GetWorldPosition(-2,-2));
             Assert.AreEqual(new Vector3(4, 0, -6), gridSys.GetWorldPosition(4,-6));
             Assert.AreEqual(new Vector3(-2, 0, 2), gridSys.GetWorldPosition(-2,2));
         }
    
         //test bag 
         [Test]
         public void TestInitializeBag()
         {

             var bag = ScriptableObject.CreateInstance<Bag>();
             
             bag.Initialize(25);
             
             Assert.AreEqual(25, bag.Slots);
             
         }
         
         //test bag 
         [Test]
         public void TestInitializeBagNegativeWillDefaultTo0()
         {

             var bag = ScriptableObject.CreateInstance<Bag>();
             
             bag.Initialize(-25);
             
             Assert.AreEqual(0, bag.Slots);
             
         }
         
        [Test]
        public void EditTestSuiteSimplePasses()
        {
            
        }
        
        [UnityTest]
        public IEnumerator EditTestSuiteWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
