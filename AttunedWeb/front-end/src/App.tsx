import { TracksView } from "./Tracks/TracksView.tsx";
import { ThemeLayer } from "./Theming/ThemeLayer.tsx";
import { QueryClient, QueryClientProvider } from "react-query";

export default function App() {
  const queryClient = new QueryClient();

  return (
    <QueryClientProvider client={queryClient}>
      <ThemeLayer>
        <h1>Tracks</h1>
        <TracksView />
      </ThemeLayer>
    </QueryClientProvider>
  );
}
