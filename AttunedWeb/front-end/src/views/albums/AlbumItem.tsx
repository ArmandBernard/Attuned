import { TrackDto } from "@dtos";
import { CoverArt } from "@components/CoverArt.tsx";

interface AlbumItemProps {
  album: string;
  albumArtist: string;
  items: TrackDto[];
  onClick: () => void;
}

export function AlbumItem({
  album,
  albumArtist,
  items,
  onClick,
}: AlbumItemProps) {
  const firstTrack = items.length > 0 ? items[0] : undefined;

  return (
    <div className="flex flex-col w-48 cursor-pointer" onClick={onClick}>
      <CoverArt
        trackId={firstTrack?.Id}
        sizeClassName="w-48 h-48"
        maxSizeClassName="max-w-48 max-h-48"
      />
      <button className="truncate text-left cursor-pointer">{album}</button>
      <div className="text-sm truncate">{albumArtist}</div>
    </div>
  );
}
