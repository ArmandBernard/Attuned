using AttunedWebApi.CodeGen;
using iTunesSmartParser.Data.Limits;

namespace AttunedWebApi.Dtos.Limits;

[TypescriptDto]
public record LimitDto
{
    public required bool IsLimited { get; init; }
    public bool OnlyChecked { get; init; }
    public SortFieldDto SortField { get; init; }
    public SortDirection SortDirection { get; init; }
    public LimitUnitsDto Units { get; init; }
    public int Amount { get; init; }

    public static LimitDto FromLimit(Limit limit)
    {
        var sortField = limit.SortBy switch
        {
            SelectionMethods.Random => SortFieldDto.Random,
            SelectionMethods.Name => SortFieldDto.Name,
            SelectionMethods.Album => SortFieldDto.Album,
            SelectionMethods.Artist => SortFieldDto.Artist,
            SelectionMethods.Genre => SortFieldDto.Genre,
            SelectionMethods.HighestRating => SortFieldDto.Rating,
            SelectionMethods.LowestRating => SortFieldDto.Rating,
            SelectionMethods.RecentlyPlayed => SortFieldDto.LastPlayed,
            SelectionMethods.OftenPlayed => SortFieldDto.PlayCount,
            SelectionMethods.RecentlyAdded => SortFieldDto.DateAdded,
            _ => throw new ArgumentOutOfRangeException(nameof(limit), "SortBy is out of range")
        };

        var direction = limit.SortBy switch
        {
            SelectionMethods.HighestRating => SortDirection.Descending,
            SelectionMethods.LowestRating => SortDirection.Ascending,
            _ => limit.SortByDescending ? SortDirection.Descending : SortDirection.Ascending
        };

        var units = limit.Units switch
        {
            LimitUnits.Minutes => LimitUnitsDto.Minutes,
            LimitUnits.MB => LimitUnitsDto.MB,
            LimitUnits.Items => LimitUnitsDto.Items,
            LimitUnits.Hours => LimitUnitsDto.Hours,
            LimitUnits.GB => LimitUnitsDto.GB,
            _ => throw new ArgumentOutOfRangeException(nameof(limit), "Units is out of range")
        };

        return new LimitDto()
        {
            IsLimited = limit.IsLimited,
            OnlyChecked = limit.OnlyChecked,
            SortField = sortField,
            SortDirection = direction,
            Units = units,
            Amount = limit.Amount
        };
    }
}