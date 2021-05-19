using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;

public class DungeonGenerator : MonoBehaviour
{ 
    [SerializeField] private int _minRooms = 10;

    private Room[,] _board = new Room[11, 11];

    private RoomType[] _leafRooms = {RoomType.YNNN, RoomType.NYNN, RoomType.NNYN, RoomType.NNNY};

    private RoomType[] _internalRooms =
    {
        RoomType.YYNN, RoomType.YNYN, RoomType.YNNY, RoomType.NYYN, RoomType.NYNY, RoomType.NNYY, RoomType.YYYN,
        RoomType.YYNY, RoomType.YNYY, RoomType.NYYY, RoomType.YYYY
    };

    private List<Room> _validNeighbors = new List<Room>();
    private List<Room> _invalidNeighbors = new List<Room>();

    private Random _random = new Random();


    protected void Start()
    {
        GenerateLevel();
        PrintBoard();
    }
    
    private void GenerateLevel()
    {
        int i;
        Room currentRoom;
        (int, int) currentRoomPosition;
        
        //chose first room
        Room firstRoom = new Room(ChooseRandomRoom(_internalRooms), (6, 6));
        _invalidNeighbors.Add(firstRoom);
        _board[6, 6] = firstRoom;
        
        _minRooms -= 1;

        while (_invalidNeighbors.Any())
        {
            //choose a random unfinished room and add to it
            i = _random.Next(0, _invalidNeighbors.Count);
            currentRoom = _invalidNeighbors[i];
            currentRoomPosition = currentRoom.getPosition();
            if (_minRooms > 0)
            {
                //add more internal rooms
                //add north room
                if (currentRoom.HasNorthConnection() &&
                    _board[currentRoomPosition.Item1, currentRoomPosition.Item2 + 1] != null)
                {
                    AddRoom((currentRoomPosition.Item1, currentRoomPosition.Item2 + 1), true);
                }
                //add east room
                if (currentRoom.HasEastConnection() &&
                    _board[currentRoomPosition.Item1 + 1, currentRoomPosition.Item2] != null)
                {
                    AddRoom((currentRoomPosition.Item1 + 1, currentRoomPosition.Item2), true);
                }
                //add south room
                if (currentRoom.HasSouthConnection() &&
                    _board[currentRoomPosition.Item1, currentRoomPosition.Item2 - 1] != null)
                {
                    AddRoom((currentRoomPosition.Item1, currentRoomPosition.Item2 - 1), true);
                }
                //add west room
                if (currentRoom.HasWestConnection() &&
                    _board[currentRoomPosition.Item1 - 1, currentRoomPosition.Item2] != null)
                {
                    AddRoom((currentRoomPosition.Item1 - 1, currentRoomPosition.Item2), true);
                }

                if (ValidateNeighbors(currentRoom))
                {
                    _invalidNeighbors.Remove(currentRoom);
                    _validNeighbors.Add(currentRoom);
                }
                //else throw new Exception("Invalid room at " + currentRoom.getPosition() + " of type " + currentRoom.GetType());
            }
            else
            {
                //finish off invalid doorways with leaf rooms
                //add north room
                if (currentRoom.HasNorthConnection() &&
                    _board[currentRoomPosition.Item1, currentRoomPosition.Item2 + 1] != null)
                {
                    AddRoom((currentRoomPosition.Item1, currentRoomPosition.Item2 + 1), false);
                }
                //add east room
                if (currentRoom.HasEastConnection() &&
                    _board[currentRoomPosition.Item1 + 1, currentRoomPosition.Item2] != null)
                {
                    AddRoom((currentRoomPosition.Item1 + 1, currentRoomPosition.Item2), false);
                }
                //add south room
                if (currentRoom.HasSouthConnection() &&
                    _board[currentRoomPosition.Item1, currentRoomPosition.Item2 - 1] != null)
                {
                    AddRoom((currentRoomPosition.Item1, currentRoomPosition.Item2 - 1), false);
                }
                //add west room
                if (currentRoom.HasWestConnection() &&
                    _board[currentRoomPosition.Item1 - 1, currentRoomPosition.Item2] != null)
                {
                    AddRoom((currentRoomPosition.Item1 - 1, currentRoomPosition.Item2), false);
                }

                if (ValidateNeighbors(currentRoom))
                {
                    _invalidNeighbors.Remove(currentRoom);
                    _validNeighbors.Add(currentRoom);
                }
                //else throw new Exception("Invalid room at " + currentRoom.getPosition() + " of type " + currentRoom.GetType());
                
            }
        }
    }

    private void AddRoom((int, int) position, bool preferInternalRoom)
    {
        List<RoomType> leafSelection = _leafRooms.ToList();
        List<RoomType> internalSelection = _internalRooms.ToList();
        Room room;
        Room neighbor;

        //check neighbor in N E S W and eliminate options  accordingly

        //check north
        neighbor = _board[position.Item1, position.Item2 + 1];
        if (neighbor != null)
        {
            if (neighbor.HasSouthConnection())
            {
                leafSelection.Remove(RoomType.NYNN);
                leafSelection.Remove(RoomType.NNYN);
                leafSelection.Remove(RoomType.NNNY);

                internalSelection.Remove(RoomType.NYYN);
                internalSelection.Remove(RoomType.NYNY);
                internalSelection.Remove(RoomType.NNYY);
                internalSelection.Remove(RoomType.NYYY);
            }
            else
            {
                leafSelection.Remove(RoomType.YNNN);
                
                internalSelection.Remove(RoomType.YYNN);
                internalSelection.Remove(RoomType.YNYN);
                internalSelection.Remove(RoomType.YNNY);
                internalSelection.Remove(RoomType.YYYN);
                internalSelection.Remove(RoomType.YYNY);
                internalSelection.Remove(RoomType.YNYY);
                internalSelection.Remove(RoomType.YYYY);
            }
        }
        //check east
        neighbor = _board[position.Item1 + 1, position.Item2];
        if (neighbor != null)
        {
            if (neighbor.HasWestConnection())
            {
                leafSelection.Remove(RoomType.YNNN);
                leafSelection.Remove(RoomType.NNYN);
                leafSelection.Remove(RoomType.NNNY);

                internalSelection.Remove(RoomType.YNYN);
                internalSelection.Remove(RoomType.YNNY);
                internalSelection.Remove(RoomType.NNYY);
                internalSelection.Remove(RoomType.YNYY);
            }
            else
            {
                leafSelection.Remove(RoomType.NYNN);
                
                internalSelection.Remove(RoomType.YYNN);
                internalSelection.Remove(RoomType.NYYN);
                internalSelection.Remove(RoomType.NYNY);
                internalSelection.Remove(RoomType.YYYN);
                internalSelection.Remove(RoomType.YYNY);
                internalSelection.Remove(RoomType.NYYY);
                internalSelection.Remove(RoomType.YYYY);
            }
        }
        
        //check south
        neighbor = _board[position.Item1, position.Item2 - 1];
        if (neighbor != null)
        {
            if (neighbor.HasNorthConnection())
            {
                leafSelection.Remove(RoomType.YNNN);
                leafSelection.Remove(RoomType.NYNN);
                leafSelection.Remove(RoomType.NNNY);

                internalSelection.Remove(RoomType.YYNN);
                internalSelection.Remove(RoomType.YNNY);
                internalSelection.Remove(RoomType.NYNY);
                internalSelection.Remove(RoomType.YYNY);
            }
            else
            {
                leafSelection.Remove(RoomType.NNYN);
                
                internalSelection.Remove(RoomType.YNYN);
                internalSelection.Remove(RoomType.NYYN);
                internalSelection.Remove(RoomType.NNYY);
                internalSelection.Remove(RoomType.YYYN);
                internalSelection.Remove(RoomType.YNYY);
                internalSelection.Remove(RoomType.NYYY);
                internalSelection.Remove(RoomType.YYYY);
            }
        }
        
        //check west
        neighbor = _board[position.Item1 - 1, position.Item2];
        if (neighbor != null)
        {
            if (neighbor.HasEastConnection())
            {
                leafSelection.Remove(RoomType.YNNN);
                leafSelection.Remove(RoomType.NYNN);
                leafSelection.Remove(RoomType.NNYN);

                internalSelection.Remove(RoomType.YYNN);
                internalSelection.Remove(RoomType.YNYN);
                internalSelection.Remove(RoomType.NYYN);
                internalSelection.Remove(RoomType.YYYN);
            }
            else
            {
                leafSelection.Remove(RoomType.NNNY);
                
                internalSelection.Remove(RoomType.YNNY);
                internalSelection.Remove(RoomType.NYNY);
                internalSelection.Remove(RoomType.NNYY);
                internalSelection.Remove(RoomType.YYNY);
                internalSelection.Remove(RoomType.YNYY);
                internalSelection.Remove(RoomType.NYYY);
                internalSelection.Remove(RoomType.YYYY);
            }
        }

        if (preferInternalRoom && internalSelection.Any())
        {
            room = new Room(ChooseRandomRoom(internalSelection), position);
            _board[position.Item1, position.Item2] = room;
            if (ValidateNeighbors(room)) _validNeighbors.Add(room);
            else _invalidNeighbors.Add(room);
            
        }
        else
        {
            room = new Room(ChooseRandomRoom(leafSelection), position);
            _board[position.Item1, position.Item2] = room;
            _validNeighbors.Add(room);
        }
    }
    
    private RoomType ChooseRandomRoom(RoomType[] selection)
    {
        int i = _random.Next(0, selection.Length);
        return selection[i];
    }
    
    private RoomType ChooseRandomRoom(List<RoomType> selection)
    {
        int i = _random.Next(0, selection.Count);
        return selection[i];
    }
    
    private bool ValidateNeighbors(Room room)
    {
        if (room.HasNorthConnection())
        {
            if (_board[room.getPosition().Item1, room.getPosition().Item2 + 1] == null) return false;
        }
        if (room.HasEastConnection())
        {
            if (_board[room.getPosition().Item1 + 1, room.getPosition().Item2] == null) return false;
        }
        if (room.HasSouthConnection())
        {
            if (_board[room.getPosition().Item1, room.getPosition().Item2 - 1] == null) return false;
        }
        if (room.HasWestConnection())
        {
            if (_board[room.getPosition().Item1 - 1, room.getPosition().Item2] == null) return false;
        }
        return true;
    }

    private void PrintBoard()
    {
        int rowLength = _board.GetLength(0);
        int colLength = _board.GetLength(1);

        for (int i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < colLength; j++)
            {
                Debug.Log(string.Format("{0} ", _board[i, j]));
            }
            Debug.Log(Environment.NewLine + Environment.NewLine);
        }
    }
}
