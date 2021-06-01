
using System;
using NUnit.Compatibility;

public class Room
{
    private RoomType type;
    
    //for debug purposes
    private (int, int) position;
    
    public Room(RoomType type, (int, int) position)
    {
        this.type = type;
        this.position = position;
    }

    public bool HasNorthConnection()
    {
        if (type.Equals(RoomType.YNNN)
        || type.Equals(RoomType.YYNN)
        || type.Equals(RoomType.YNYN)
        || type.Equals(RoomType.YNNY)
        || type.Equals(RoomType.YYYN)
        || type.Equals(RoomType.YYNY)
        || type.Equals(RoomType.YNYY)
        || type.Equals(RoomType.YYYY)) return true;
        return false;
    }
    
    public bool HasEastConnection()
    {
        if (type.Equals(RoomType.NYNN)
            || type.Equals(RoomType.YYNN)
            || type.Equals(RoomType.NYYN)
            || type.Equals(RoomType.NYNY)
            || type.Equals(RoomType.YYYN)
            || type.Equals(RoomType.YYNY)
            || type.Equals(RoomType.NYYY)
            || type.Equals(RoomType.YYYY)) return true;
        return false;
    }
    
    public bool HasSouthConnection()
    {
        if (type.Equals(RoomType.NNYN)
            || type.Equals(RoomType.YNYN)
            || type.Equals(RoomType.NYYN)
            || type.Equals(RoomType.NNYY)
            || type.Equals(RoomType.YYYN)
            || type.Equals(RoomType.YNYY)
            || type.Equals(RoomType.NYYY)
            || type.Equals(RoomType.YYYY)) return true;
        return false;
    }
    
    public bool HasWestConnection()
    {
        if (type.Equals(RoomType.NNNY)
            || type.Equals(RoomType.YNNY)
            || type.Equals(RoomType.NYNY)
            || type.Equals(RoomType.NNYY)
            || type.Equals(RoomType.YYNY)
            || type.Equals(RoomType.YNYY)
            || type.Equals(RoomType.NYYY)
            || type.Equals(RoomType.YYYY)) return true;
        return false;
    }

    public (int, int) getPosition()
    {
        return position;
    }

    public RoomType GetRoomType()
    {
        return type;
    }

    public new String ToString()
    {
        return type.ToString();
    }
}
