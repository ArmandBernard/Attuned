import { Dialog } from "@components/Dialog.tsx";
import { FunctionComponent, useMemo, useState } from "react";
import { TrackDetailsDto, TrackDto } from "@dtos";
import { useRouteQuery } from "@utils/useRouteQuery.ts";
import { getRatingString } from "@utils/stringFormatters/getRatingString.ts";
import { Rating } from "@root/dtos/Rating.ts";
import { timeSpanToTimeString } from "@utils/stringFormatters/timeSpanToTimeString.ts";
import IconButton from "@components/IconButton.tsx";
import CloseIcon from "@components/icons/CloseIcon.tsx";
import { TrackDetails } from "@views/tracks/TrackDetails.tsx";
import LinkButton from "@components/LinkButton.tsx";

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
      className="max-sm:w-full sm:max-w-[700px] sm:w-4/5 bg-background text-text-color m-auto"
      show={show}
      onClose={onClose}
      closeOnBackgroundClick
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
    <div className="p-4 text-xl bg-paper-1 sticky top-0">
      <div className="flex items-center gap-4">
        {albumArt ? (
          <div className="w-32 h-32 flex items-center justify-center">
            <img
              alt="album art"
              className="max-w-32 max-h-32 h-auto w-auto rounded-sm"
              src={`data:image/png;base64,${albumArt}`}
            />
          </div>
        ) : (
          <div className="w-32 h-32 rounded-sm bg-paper-2"></div>
        )}
        <div>
          <h1 className="text-xl">{album}</h1>
          <div className="text-base">{artist}</div>
        </div>
      </div>
      <IconButton
        aria-label="Close"
        title="Close"
        className="absolute top-2 right-2"
        onClick={onClose}
      >
        <CloseIcon />
      </IconButton>
    </div>
  );
}

function Details({ tracks }: { tracks: TrackDto[] | undefined }) {
  const [openedTrack, setOpenedTrack] = useState<TrackDto | undefined>(
    undefined,
  );

  const index = openedTrack && tracks?.indexOf(openedTrack);

  const sorted = useMemo(() => tracks?.sort(sortComparer), [tracks]);

  const previousTrack =
    index !== undefined && index > 0 ? sorted?.at(index - 1) : undefined;

  const nextTrack =
    sorted && index !== undefined && index < sorted.length
      ? sorted.at(index + 1)
      : undefined;

  const groupedByDisc = sorted
    ? Object.groupBy(sorted, (item) => item.DiscNumber ?? "none")
    : [];

  return (
    <>
      <ul className="grid grid-cols-[auto_1fr_auto_auto] p-4 gap-x-4 gap-y-2">
        {Object.entries(groupedByDisc).map(([disc, tracks]) => {
          return (
            <>
              {disc !== "none" && (
                <>
                  <span></span>
                  <span className="col-span-3 font-bold">Disc {disc}</span>
                </>
              )}
              {tracks?.map((track) => (
                <>
                  <span>{track.TrackNumber}</span>
                  <LinkButton
                    className="text-left"
                    onClick={() => setOpenedTrack(track)}
                  >
                    {track.Name}
                  </LinkButton>
                  <span>{getRatingString(track.Rating as Rating)}</span>
                  <span>
                    {track.TotalTime && timeSpanToTimeString(track.TotalTime)}
                  </span>
                </>
              ))}
            </>
          );
        })}
      </ul>
      <TrackDetails
        show={openedTrack !== undefined}
        onPreviousTrack={previousTrack && (() => setOpenedTrack(previousTrack))}
        onNextTrack={nextTrack && (() => setOpenedTrack(nextTrack))}
        onClose={() => setOpenedTrack(undefined)}
        track={openedTrack}
      />
    </>
  );
}

const sortComparer = (trackA: TrackDto, trackB: TrackDto): number => {
  const discNumberA = trackA.DiscNumber ?? 0;
  const discNumberB = trackB.DiscNumber ?? 0;

  const discSort =
    discNumberA > discNumberB ? 1 : discNumberA < discNumberB ? -1 : 0;

  if (discSort !== 0) {
    return discSort;
  }

  const trackNumberA = trackA.TrackNumber ?? 0;
  const trackNumberB = trackB.TrackNumber ?? 0;

  return trackNumberA > trackNumberB ? 1 : trackNumberA < trackNumberB ? -1 : 0;
};
