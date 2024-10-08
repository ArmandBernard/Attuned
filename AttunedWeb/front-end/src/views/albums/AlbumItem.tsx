import { TrackDetailsDto, TrackDto } from "@dtos";
import { useRouteQuery } from "@utils/useRouteQuery.ts";

interface AlbumItemProps {
  album: string;
  artist: string;
  items: TrackDto[];
  onClick: () => void;
}

export function AlbumItem({ album, artist, items, onClick }: AlbumItemProps) {
  const firstTrack = items.length > 0 ? items[0] : undefined;

  const { data: trackDetails } = useRouteQuery<
    TrackDetailsDto,
    unknown,
    { id: number }
  >({
    enabled: firstTrack !== undefined,
    url: "track/{id}",
    parameters: {
      id: firstTrack?.Id as number,
    },
  });

  return (
    <div className="flex flex-col w-48 cursor-pointer" onClick={onClick}>
      {trackDetails?.CoverArt ? (
        <div className="w-48 h-48 flex items-center justify-center">
          <img
            alt="album art"
            className="max-w-48 max-h-48 h-auto w-auto rounded"
            src={`data:image/png;base64,${trackDetails.CoverArt}`}
          />
        </div>
      ) : (
        <div className="w-48 h-48 rounded bg-paper-2"></div>
      )}
      <button className="truncate text-left">{album}</button>
      <div className="text-sm truncate">{artist}</div>
    </div>
  );
}
