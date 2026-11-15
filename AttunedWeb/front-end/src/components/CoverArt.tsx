import { useState } from "react";
import { apiBase } from "@utils/useRouteQuery.ts";
import { MusicNotesIcon } from "@components/icons/MusicNotesIcon.tsx";

interface CoverArtProps {
  trackId: number | undefined;
  /** Tailwind size classes for the square container, e.g. "w-48 h-48" */
  sizeClassName: string;
  /** Matching tailwind max-size classes for the image, e.g. "max-w-48 max-h-48" */
  maxSizeClassName: string;
}

/**
 * Album art loaded natively by the browser from the cover endpoint.
 * The placeholder is shown until the image loads, or if the track has no art.
 */
export function CoverArt({
  trackId,
  sizeClassName,
  maxSizeClassName,
}: CoverArtProps) {
  // remember which track failed, so a different track gets a fresh attempt
  const [failedId, setFailedId] = useState<number | undefined>(undefined);

  const failed = trackId === undefined || failedId === trackId;

  return (
    <div
      className={`${sizeClassName} rounded-sm bg-paper-2 flex items-center justify-center`}
    >
      {!failed ? (
        <img
          alt="album art"
          loading="lazy"
          decoding="async"
          className={`${maxSizeClassName} h-auto w-auto rounded-sm`}
          src={new URL(`cover/${trackId}`, apiBase).toString()}
          onError={() => setFailedId(trackId)}
        />
      ) : (
        <div className="p-[10%]">
          <MusicNotesIcon className="fill-paper-1 h-full w-full" />
        </div>
      )}
    </div>
  );
}
