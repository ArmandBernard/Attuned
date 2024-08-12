import { useRouteQuery } from "@utils/useRouteQuery.ts";
import { TrackDto } from "@dtos";
import { useMemo, useState } from "react";
import { AlbumItem } from "@views/albums/AlbumItem.tsx";
import { FullScreenLoading } from "@components/FullScreenLoading.tsx";
import { AlbumDetails } from "@views/albums/AlbumDetails.tsx";

export function AlbumsView() {
  const [openAlbum, setOpenAlbum] = useState<
    { album: string; artist: string } | undefined
  >(undefined);
  const { data: tracks, isLoading: isLoadingTracks } = useRouteQuery<
    TrackDto[]
  >({
    url: "track",
  });

  const sorted = useMemo(() => tracks?.sort(sortComparer), [tracks]);

  const grouped = useMemo(() => {
    const map = new Map<string, TrackDto[]>();
    sorted?.forEach((track) => {
      const key = `${track.Album};${track.Artist}`;
      const collection = map.get(key);
      if (!collection) {
        map.set(key, [track]);
      } else {
        collection.push(track);
      }
    });

    return map;
  }, [sorted]);

  const openAlbumTracks =
    openAlbum && grouped.get(`${openAlbum.album};${openAlbum.artist}`);

  return (
    <main className="flex flex-1 flex-col items-center">
      {isLoadingTracks ? (
        <FullScreenLoading />
      ) : (
        <div className="p-4 justify-items-center grid xs:grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-8">
          {[...grouped].map((x) => {
            const album = x[1][0].Album;
            const artist = x[1][0].Artist;

            return (
              <AlbumItem
                album={album}
                artist={artist}
                items={x[1]}
                onClick={() => setOpenAlbum({ album, artist })}
              />
            );
          })}
        </div>
      )}
      <AlbumDetails
        show={openAlbum !== undefined}
        onClose={() => setOpenAlbum(undefined)}
        album={openAlbum?.album}
        artist={openAlbum?.artist}
        tracks={openAlbumTracks}
      />
    </main>
  );
}

const sortComparer = (trackA: TrackDto, trackB: TrackDto): number => {
  const albumOrder = trackA.Album?.localeCompare(trackB.Album);

  if (albumOrder !== 0) {
    return albumOrder;
  }

  const artistOrder = trackA.Artist?.localeCompare(trackB.Artist);

  if (artistOrder !== 0) {
    return artistOrder;
  }

  const trackANumber = trackA.TrackNumber ?? 0;
  const trackBNumber = trackB.TrackNumber ?? 0;

  return trackANumber > trackBNumber ? 1 : trackANumber < trackBNumber ? -1 : 0;
};
