interface AlbumItemProps {
  album: string;
  artist: string;
  coverArt: string | undefined;
  onClick: () => void;
}

export function AlbumItem({
  album,
  artist,
  coverArt,
  onClick,
}: AlbumItemProps) {
  return (
    <div className="flex flex-col w-48 cursor-pointer" onClick={onClick}>
      {coverArt ? (
        <div className="w-48 h-48 flex items-center justify-center">
          <img
            alt="album art"
            className="max-w-48 max-h-48 h-auto w-auto rounded"
            src={`data:image/png;base64,${coverArt}`}
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
