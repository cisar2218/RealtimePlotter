using System;

[Serializable]
public class Message
{
    public string type;
    
    // zeros are default
    public float lon = 0; 
    public float lat = 0;
    public float alt = 0;
    public float pitch = 0;
    public float roll = 0;
    public float yaw = 0;
    public float value = 0;

    override public string ToString() {
        return string.Format("{0} {1} {2} {3} {4}", type, lon, lat, alt, value);
    }
}

public enum MessageType {
    Value,
    Pos,
    Att,
    Clear,
    None
}

public static class MessageTypeExtensions
{
    public static MessageType ToMessageType(this string messageTypeString)
    {
        if (Enum.TryParse<MessageType>(messageTypeString, true, out MessageType messageType))
        {
            return messageType;
        }
        else
        {
            return MessageType.None;
        }
    }
}