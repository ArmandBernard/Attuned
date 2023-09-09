import { TracksView } from "./Tracks/TracksView.tsx";
import { ThemeLayer } from "./Theming/ThemeLayer.tsx";
import { QueryClient, QueryClientProvider } from "react-query";
import { Navigation } from "./Navigation.tsx";
import { useState } from "react";

type SelectedTrackList =
  | { playListId: number; playListName: string; trackIds: number[] }
  | undefined;

export default function App() {
  const queryClient = new QueryClient();
  const [selectedTrackList, setSelectedTrackList] =
    useState<SelectedTrackList>(undefined);

  return (
    <QueryClientProvider client={queryClient}>
      <ThemeLayer>
        <div className="flex">
          <Navigation
            selectedTrackList={selectedTrackList}
            setSelectedTrackList={(trackList) =>
              setSelectedTrackList(trackList)
            }
          />
          <main className="max-sm:h-full">
            <TracksView
              title={selectedTrackList?.playListName ?? "All tracks"}
              idFilter={selectedTrackList?.trackIds}
            />
          </main>
        </div>
      </ThemeLayer>
    </QueryClientProvider>
  );
}
