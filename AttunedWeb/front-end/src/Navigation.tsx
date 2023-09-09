import { FunctionComponent } from "react";
import { useRouteQuery } from "./Queries/useRouteQuery.ts";
import { PlaylistDto } from "./dtos/Dtos.ts";

type SelectedTrackList =
  | { playListId: number; playListName: string; trackIds: number[] }
  | undefined;

interface NavigationProps {
  selectedTrackList: SelectedTrackList;
  setSelectedTrackList: (selectedTrackList: SelectedTrackList) => void;
}

const mediaPlaylistNames = [
  "Music",
  "Films",
  "TV Programmes",
  "Podcasts",
  "Audiobooks",
];

export const Navigation: FunctionComponent<NavigationProps> = (props) => {
  const { data, isFetching } = useRouteQuery<PlaylistDto[]>({
    url: "playlist",
    refetchOnWindowFocus: false,
  });

  const allPlaylists = data?.map((x) => ({
    id: x.Id,
    name: x.Name,
    trackList: x.Items,
  }));

  const mediaPlaylists = allPlaylists?.filter(
    (x) => mediaPlaylistNames.indexOf(x.name) !== -1,
  );

  const playlists = allPlaylists?.filter(
    (x) => mediaPlaylistNames.indexOf(x.name) === -1,
  );

  return (
    <nav className="p-4 border-r">
      <h2 className="text-2xl mb-2">Navigation</h2>
      <ul>
        <li>
          <h3 className="text-xl">
            <button onClick={() => props.setSelectedTrackList(undefined)}>
              All tracks
            </button>
          </h3>
        </li>
        <li>
          <h3 className="text-xl">Media categories</h3>
          <ul className="pl-2">
            {isFetching && <span>Loading...</span>}
            {mediaPlaylists &&
              mediaPlaylists.map((x) => (
                <li key={x.id}>
                  <button
                    className="truncate"
                    onClick={() =>
                      props.setSelectedTrackList({
                        playListId: x.id,
                        playListName: x.name,
                        trackIds: x.trackList,
                      })
                    }
                  >
                    {x.name}
                  </button>
                </li>
              ))}
          </ul>
        </li>
        <li>
          <h3 className="text-xl">Playlists</h3>
          <ul className="pl-2">
            {isFetching && <span>Loading...</span>}
            {playlists &&
              playlists.map((x) => (
                <li key={x.id}>
                  <button
                    className="truncate"
                    onClick={() =>
                      props.setSelectedTrackList({
                        playListId: x.id,
                        playListName: x.name,
                        trackIds: x.trackList,
                      })
                    }
                  >
                    {x.name}
                  </button>
                </li>
              ))}
          </ul>
        </li>
      </ul>
    </nav>
  );
};
