import { Dialog } from "@components/Dialog.tsx";
import { FunctionComponent } from "react";
import { TrackDetailsDto, TrackDto } from "@dtos";
import { useRouteQuery } from "@utils/useRouteQuery.ts";
import { getRatingString } from "@utils/stringFormatters/getRatingString.ts";
import { Rating } from "@root/dtos/Rating.ts";
import { timeSpanToTimeString } from "@utils/stringFormatters/timeSpanToTimeString.ts";

interface TrackDetailProps {
  show: boolean;
  onClose: () => void;
  album: string | undefined;
  artist: string | undefined;
  tracks: TrackDto[] | undefined;
}

export const AlbumDetails: FunctionComponent<TrackDetailProps> = ({
  show,
  onClose,
  album,
  artist,
  tracks,
}) => {
  const firstTrack = tracks && tracks.length > 0 ? tracks[0] : undefined;

  const { data: trackDetails } = useRouteQuery<
    TrackDetailsDto,
    unknown,
    { id: number }
  >({
    enabled: firstTrack !== undefined,
    url: "track/{id}",
    parameters: {
      id: firstTrack?.Id as number,
    },
  });

  return (
    <Dialog
      className="max-sm:w-full sm:max-w-[700px] sm:w-4/5 bg-background text-text-color"
      show={show}
      onClose={onClose}
    >
      <Header
        album={album}
        artist={artist}
        albumArt={trackDetails?.CoverArt}
        onClose={onClose}
      />
      <Details tracks={tracks} />
    </Dialog>
  );
};

function Header({
  album,
  artist,
  albumArt,
  onClose,
}: {
  album: string | undefined;
  artist: string | undefined;
  albumArt: string | undefined;
  onClose: () => void;
}) {
  return (
    <div className="flex justify-between py-3 px-4 text-xl bg-paper-1">
      <div className="flex items-center gap-4">
        {albumArt ? (
          <div className="w-32 h-32 flex items-center justify-center">
            <img
              alt="album art"
              className="max-w-32 max-h-32 h-auto w-auto rounded"
              src={`data:image/png;base64,${albumArt}`}
            />
          </div>
        ) : (
          <div className="w-32 h-32 rounded bg-paper-2"></div>
        )}
        <div>
          <h1 className="text-xl">{album}</h1>
          <div className="text-base">{artist}</div>
        </div>
      </div>
      <div className="flex items-start gap-4">
        <button aria-label="Close" title="Close" onClick={onClose}>
          ✕
        </button>
      </div>
    </div>
  );
}

function Details({ tracks }: { tracks: TrackDto[] | undefined }) {
  return (
    <ul className="grid grid-cols-[auto_1fr_auto_auto] p-4 gap-x-4 gap-y-2">
      {tracks?.map((track) => (
        <>
          <span>{track.TrackNumber}</span>
          <span>{track.Name}</span>
          <span>{getRatingString(track.Rating as Rating)}</span>
          <span>
            {track.TotalTime && timeSpanToTimeString(track.TotalTime)}
          </span>
        </>
      ))}
    </ul>
  );
}
