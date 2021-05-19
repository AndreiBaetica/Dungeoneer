
using System;
using NUnit.Compatibility;

public class Room
{
    protected RoomType Type;
    
    //for debug purposes
    protected (int, int) Position;

    protected bool ValidNeighbors = false;

    public Room(RoomType type, (int, int) position)
    {
        Type = type;
        Position = position;
    }

    public bool HasNorthConnection()
    {
        if (Type.Equals(RoomType.YNNN)
        || Type.Equals(RoomType.YYNN)
        || Type.Equals(RoomType.YNYN)
        || Type.Equals(RoomType.YNNY)
        || Type.Equals(RoomType.YYYN)
        || Type.Equals(RoomType.YYNY)
        || Type.Equals(RoomType.YNYY)
        || Type.Equals(RoomType.YYYY)) return true;
        return false;
    }
    
    public bool HasEastConnection()
    {
        if (Type.Equals(RoomType.NYNN)
            || Type.Equals(RoomType.YYNN)
            || Type.Equals(RoomType.NYYN)
            || Type.Equals(RoomType.NYNY)
            || Type.Equals(RoomType.YYYN)
            || Type.Equals(RoomType.YYNY)
            || Type.Equals(RoomType.NYYY)
            || Type.Equals(RoomType.YYYY)) return true;
        return false;
    }
    
    public bool HasSouthConnection()
    {
        if (Type.Equals(RoomType.NNYN)
            || Type.Equals(RoomType.YNYN)
            || Type.Equals(RoomType.NYYN)
            || Type.Equals(RoomType.NNYY)
            || Type.Equals(RoomType.YYYN)
            || Type.Equals(RoomType.YNYY)
            || Type.Equals(RoomType.NYYY)
            || Type.Equals(RoomType.YYYY)) return true;
        return false;
    }
    
    public bool HasWestConnection()
    {
        if (Type.Equals(RoomType.NNNY)
            || Type.Equals(RoomType.YNNY)
            || Type.Equals(RoomType.NYNY)
            || Type.Equals(RoomType.NNYY)
            || Type.Equals(RoomType.YYNY)
            || Type.Equals(RoomType.YNYY)
            || Type.Equals(RoomType.NYYY)
            || Type.Equals(RoomType.YYYY)) return true;
        return false;
    }

    public (int, int) getPosition()
    {
        return Position;
    }

    public RoomType GetRoomType()
    {
        return Type;
    }

    public new String ToString()
    {
        return Type.ToString();
    }
}
