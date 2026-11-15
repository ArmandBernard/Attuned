import { PlaylistsView } from "@views/playlists/PlaylistsView.tsx";

export function AllTracksRouteView() {
  return (
    <main className="h-full min-w-0 flex-1">
      <PlaylistsView title="All tracks" playlist={undefined} />
    </main>
  );
}
