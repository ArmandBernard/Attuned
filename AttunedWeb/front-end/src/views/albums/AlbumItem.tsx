import { useRouteQuery } from "@utils/useRouteQuery.ts";
import { TrackDto } from "@dtos";

interface AlbumItemProps {
  album: string;
  artist: string;
  onClick: () => void;
  tracks: TrackDto[];
}

export function AlbumItem({ album, artist, onClick, tracks }: AlbumItemProps) {
  const { data: art } = useRouteQuery<string, unknown, { id: number }>({
    url: "image/{id}",
    parameters: { id: tracks[0].Id },
  });

  return (
    <div className="flex flex-col w-48 cursor-pointer" onClick={onClick}>
      {art ? (
        <div className="w-48 h-48 flex items-center justify-center">
          <img
            alt="album art"
            className="max-w-48 max-h-48 h-auto w-auto rounded"
            src={`data:image/png;base64,${art}`}
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
