namespace iTunesSmartParser.Data.Limits;

public record Limit(bool IsLimited, LimitUnits? Units, int? Amount, bool OnlyChecked, SelectionMethods SortBy,
    bool SortByDescending);