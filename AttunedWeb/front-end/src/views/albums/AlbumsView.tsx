import { useRouteQuery } from "@utils/useRouteQuery.ts";
import { TrackDto } from "@dtos";
import { useMemo } from "react";
import { AlbumItem } from "@views/albums/AlbumItem.tsx";
import { FullScreenLoading } from "@components/FullScreenLoading.tsx";

export function AlbumsView() {
  const { data: tracks, isLoading: isLoadingTracks } = useRouteQuery<
    TrackDto[]
  >({
    url: "track",
  });

  const grouped = useMemo(() => {
    const map = new Map<string, TrackDto[]>();
    tracks?.forEach((track) => {
      const key = `${track.Album};${track.Artist}`;
      const collection = map.get(key);
      if (!collection) {
        map.set(key, [track]);
      } else {
        collection.push(track);
      }
    });

    return map;
  }, [tracks]);

  return (
    <main className="flex flex-1 flex-col items-center">
      {isLoadingTracks ? (
        <FullScreenLoading />
      ) : (
        <div className="p-4 justify-items-center grid xs:grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-8">
          {[...grouped].map((x) => (
            <AlbumItem
              album={x[1][0].Album}
              artist={x[1][0].Artist}
              items={x[1]}
            />
          ))}
        </div>
      )}
    </main>
  );
}
