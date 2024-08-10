import { TracksView } from "./Tracks/TracksView.tsx";
import { ThemeLayer } from "./Theming/ThemeLayer.tsx";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Navigation } from "./Navigation.tsx";
import { useState } from "react";
import { PlaylistDto } from "./dtos/Dtos.ts";

export default function App() {
  const queryClient = new QueryClient();
  const [selectedPlaylist, setSelectedPlaylist] = useState<
    PlaylistDto | undefined
  >(undefined);

  return (
    <QueryClientProvider client={queryClient}>
      <ThemeLayer>
        <div className="flex h-full">
          <Navigation
            selectedPlaylist={selectedPlaylist}
            setSelectedPlaylist={(playlist) => setSelectedPlaylist(playlist)}
          />
          <main className="h-full min-w-0 flex-1">
            <TracksView
              title={selectedPlaylist?.Name ?? "All tracks"}
              playlist={selectedPlaylist}
            />
          </main>
        </div>
      </ThemeLayer>
    </QueryClientProvider>
  );
}
