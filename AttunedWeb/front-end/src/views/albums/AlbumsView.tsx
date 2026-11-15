import { useRouteQuery } from "@utils/useRouteQuery.ts";
import { TrackDto } from "@dtos";
import { useMemo, useState } from "react";
import { AlbumItem } from "@views/albums/AlbumItem.tsx";
import { FullScreenLoading } from "@components/FullScreenLoading.tsx";
import { AlbumDetails } from "@views/albums/AlbumDetails.tsx";

export function AlbumsView() {
  const [openAlbum, setOpenAlbum] = useState<
    { album: string; albumArtist: string } | undefined
  >(undefined);
  const { data: tracks, isLoading: isLoadingTracks } = useRouteQuery<
    TrackDto[]
  >({
    url: "track",
  });

  const sorted = useMemo(() => tracks?.sort(sortComparer), [tracks]);

  const grouped = useMemo(
    () =>
      Object.groupBy(
        sorted ?? [],
        (track) => `${track.Album};${track.AlbumArtist}`,
      ),
    [sorted],
  );

  const openAlbumTracks =
    openAlbum && grouped[`${openAlbum.album};${openAlbum.albumArtist}`];

  return (
    <main className="flex flex-1 flex-col items-center">
      {isLoadingTracks ? (
        <FullScreenLoading />
      ) : (
        <div className="p-4 justify-items-center grid xs:grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-8">
          {Object.entries(grouped).map(([group, tracks]) => {
            if (!tracks) return null;

            const album = tracks[0].Album;
            const albumArtist = tracks[0].AlbumArtist;

            return (
              <AlbumItem
                key={group}
                album={album}
                albumArtist={albumArtist}
                items={tracks}
                onClick={() => setOpenAlbum({ album, albumArtist })}
              />
            );
          })}
        </div>
      )}
      <AlbumDetails
        show={openAlbum !== undefined}
        onClose={() => setOpenAlbum(undefined)}
        album={openAlbum?.album}
        albumArtist={openAlbum?.albumArtist}
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

  return trackA.Artist?.localeCompare(trackB.Artist);
};
