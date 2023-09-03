namespace iTunesSmartParser.Data;

public record Limit(bool IsLimited, LimitUnits? Units, int? Amount, bool OnlyChecked, SelectionMethods SortBy,
    bool SortByDescending);