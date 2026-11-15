import { Dialog } from "@components/Dialog.tsx";
import { FunctionComponent, useMemo, useState } from "react";
import { TrackDto } from "@dtos";
import { CoverArt } from "@components/CoverArt.tsx";
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
  albumArtist: string | undefined;
  tracks: TrackDto[] | undefined;
}

export const AlbumDetails: FunctionComponent<TrackDetailProps> = ({
  show,
  onClose,
  album,
  albumArtist,
  tracks,
}) => {
  const firstTrack = tracks && tracks.length > 0 ? tracks[0] : undefined;

  const distinctArtists = new Set([
    ...(albumArtist ? [albumArtist] : []),
    ...(tracks?.map((t) => t.Artist) ?? []),
  ]);
  const showArtist = distinctArtists.size > 1;

  return (
    <Dialog
      className="max-sm:w-full sm:max-w-[700px] sm:w-4/5 bg-background text-text-color m-auto"
      show={show}
      onClose={onClose}
      closeOnBackgroundClick
    >
      <Header
        album={album}
        albumArtist={albumArtist ?? firstTrack?.Artist}
        trackId={firstTrack?.Id}
        onClose={onClose}
      />
      <Details tracks={tracks} showArtist={showArtist} />
    </Dialog>
  );
};

function Header({
  album,
  albumArtist,
  trackId,
  onClose,
}: {
  album: string | undefined;
  albumArtist: string | undefined;
  trackId: number | undefined;
  onClose: () => void;
}) {
  return (
    <div className="p-4 text-xl bg-paper-1 sticky top-0">
      <div className="flex items-center gap-4">
        <CoverArt
          trackId={trackId}
          sizeClassName="w-32 h-32"
          maxSizeClassName="max-w-32 max-h-32"
        />
        <div>
          <h1 className="text-xl">{album}</h1>
          <div className="text-base">{albumArtist}</div>
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

function Details({
  tracks,
  showArtist,
}: {
  tracks: TrackDto[] | undefined;
  showArtist: boolean;
}) {
  const [openedTrack, setOpenedTrack] = useState<TrackDto | undefined>(
    undefined,
  );

  const sorted = useMemo(() => tracks?.toSorted(sortComparer), [tracks]);

  const index = openedTrack && sorted?.indexOf(openedTrack);

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
      <ul className="grid grid-cols-[auto_1fr_auto_auto] items-center p-4 gap-x-4 gap-y-2">
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
                  <div>
                    <LinkButton
                      className="block"
                      onClick={() => setOpenedTrack(track)}
                    >
                      {track.Name}
                    </LinkButton>
                    {showArtist && (
                      <div className="text-sm">{track.Artist}</div>
                    )}
                  </div>
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
