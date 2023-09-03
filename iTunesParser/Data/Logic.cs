namespace iTunesSmartParser.Data;

public enum LogicSign
{
    IntPositive = 0x00,
    StringPositive = 0x01,
    IntNegative = 0x02,
    StringNegative = 0x03
}

public enum LogicRule
{
    Other = 0x00,
    Is = 0x01,
    Contains = 0x02,
    Starts = 0x04,
    Ends = 0x08,
    Greater = 0x10,
    Less = 0x40
}
