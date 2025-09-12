using System;
using System.Collections;
using System.Globalization;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.Message
{
    [Serializable]
    public class MessagePrototype
    {
        public Sprite sp;

        public string content;
        public string name;

        public SerializableDateTime sdt;
        public DateTime sendTime
        {
            get
            {
                return sdt.ToDateTime();
            }
        }

        public string timeStr
        {
            get { return sendTime.ToString("h:mm tt", CultureInfo.InvariantCulture); }
        }
    }
}


[Serializable]
public struct SerializableDateTime
{
    public int Year;
    public int Month;
    public int Day;
    public int Hour;
    public int Minute;
    public int Second;

    // Constructor from System.DateTime
    public SerializableDateTime(DateTime dt)
    {
        Year = dt.Year;
        Month = dt.Month;
        Day = dt.Day;
        Hour = dt.Hour;
        Minute = dt.Minute;
        Second = dt.Second;
    }

    // One-line conversion back to System.DateTime
    public DateTime ToDateTime() =>
        new DateTime(Year, Month, Day, Hour, Minute, Second);

    // Implicit helpers so you rarely have to think about it
    public static implicit operator SerializableDateTime(DateTime dt) => new SerializableDateTime(dt);
    public static implicit operator DateTime(SerializableDateTime sdt) => sdt.ToDateTime();
}