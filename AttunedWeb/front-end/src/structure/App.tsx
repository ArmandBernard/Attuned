import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ThemeLayer } from "@root/structure/ThemeLayer.tsx";
import { RouterLayer } from "@root/structure/RouterLayer.tsx";

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
