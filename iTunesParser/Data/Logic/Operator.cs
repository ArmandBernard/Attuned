namespace iTunesSmartParser.Data.Logic;

public enum Operator
{
    Other = 0x00,
    Is = 0x01,
    Contains = 0x02,
    Starts = 0x04,
    Ends = 0x08,
    Greater = 0x10,
    Less = 0x40
}
