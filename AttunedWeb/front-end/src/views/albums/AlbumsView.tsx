import { useRouteQuery } from "@utils/useRouteQuery.ts";
import { TrackDto } from "@dtos";
import { useMemo } from "react";
import { useNavigate, useParams, useSearch } from "@tanstack/react-router";
import { AlbumItem } from "@views/albums/AlbumItem.tsx";
import { FullScreenLoading } from "@components/FullScreenLoading.tsx";
import { AlbumDetails } from "@views/albums/AlbumDetails.tsx";

export function AlbumsView() {
  const navigate = useNavigate();
  const search = useSearch({ from: "/albums" });

  const openAlbum =
    search.album === undefined
      ? undefined
      : { album: search.album, albumArtist: search.albumArtist ?? "" };

  const { data: unsortedTracks, isLoading: isLoadingTracks } = useRouteQuery<
    TrackDto[]
  >({
    url: "track",
  });

  const sorted = useMemo(
    () => unsortedTracks?.toSorted(sortComparer),
    [unsortedTracks],
  );

  const grouped = useMemo(
    () =>
      Object.groupBy(sorted ?? [], (track) => {
        return albumKey(track.Album, track.AlbumArtist);
      }),
    [sorted],
  );

  const openAlbumTracks =
    openAlbum && grouped[albumKey(openAlbum.album, openAlbum.albumArtist)];

  const trackParams = useParams({
    from: "/albums/$trackId",
    shouldThrow: false,
  });
  const openedTrackId = trackParams ? parseInt(trackParams.trackId) : undefined;

  const closeAlbum = () => {
    void navigate({ to: "/albums", resetScroll: false });
  };

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

            // tracks with no album are not an album, so they are left out of the list
            if (!album) return null;

            return (
              <AlbumItem
                key={group}
                album={album}
                albumArtist={albumArtist}
                items={tracks}
              />
            );
          })}
        </div>
      )}
      <AlbumDetails
        show={openAlbum !== undefined}
        onClose={closeAlbum}
        album={openAlbum?.album}
        albumArtist={openAlbum?.albumArtist}
        tracks={openAlbumTracks}
        openedTrackId={openedTrackId}
      />
    </main>
  );
}

const sortComparer = (trackA: TrackDto, trackB: TrackDto): number => {
  const albumOrder = (trackA.Album ?? "").localeCompare(trackB.Album ?? "");

  if (albumOrder !== 0) {
    return albumOrder;
  }

  return (trackA.Artist ?? "").localeCompare(trackB.Artist ?? "");
};

const albumKey = (
  album: string | undefined,
  albumArtist: string | undefined,
): string => JSON.stringify([album ?? "", albumArtist ?? ""]);
