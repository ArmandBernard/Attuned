import { ThemeLayer } from "./Theming/ThemeLayer.tsx";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { RouterLayer } from "./Routing/RouterLayer.tsx";

export default function App() {
  const queryClient = new QueryClient();

  return (
    <QueryClientProvider client={queryClient}>
      <ThemeLayer>
        <RouterLayer />
      </ThemeLayer>
    </QueryClientProvider>
  );
}
