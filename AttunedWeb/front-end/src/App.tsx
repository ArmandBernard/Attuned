import { TracksView } from "./Tracks/TracksView.tsx";
import { ThemeLayer } from "./Theming/ThemeLayer.tsx";

export default function App() {
  return (
    <>
      <ThemeLayer>
        <h1>Tracks</h1>
        <TracksView />
      </ThemeLayer>
    </>
  );
}
