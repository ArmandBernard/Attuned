import { Dialog } from "@components/Dialog.tsx";
import { FunctionComponent, ReactNode } from "react";
import { TrackDetailsDto, TrackDto } from "@dtos";
import { getRatingString } from "@utils/stringFormatters/getRatingString.ts";
import { Rating } from "@root/dtos/Rating.ts";
import { LoveStatus } from "@components/icons/LoveStatus.tsx";
import { getLoveLabel } from "@utils/stringFormatters/getLoveLabel.ts";
import { useRouteQuery } from "@utils/useRouteQuery.ts";
import CloseIcon from "@components/icons/CloseIcon.tsx";
import IconButton from "@components/IconButton.tsx";
import ChevronRightIcon from "@components/icons/ChevronRightIcon.tsx";
import ChevronLeftIcon from "@components/icons/ChevronLeftIcon.tsx";
import { CoverArt } from "@components/CoverArt.tsx";
import { useNavigate } from "@tanstack/react-router";
import RouteIconButton from "@components/RouteIconButton.tsx";

interface TrackDetailProps {
  previousTrack: TrackDto | undefined;
  nextTrack: TrackDto | undefined;
  track: TrackDto | undefined;
  // Overrides the default close behaviour of navigating to the parent route,
  // for routes whose parent is not a sensible place to go back to.
  onClose?: () => void;
}

// Only shows when `track` is set, which happens on a route ending in $trackId
// (albums or playlists). Navigation is therefore relative, so this doesn't need
// to know which route it is on: "." is the current route with a different
// trackId, and ".." is the parent route with no track open.
export const TrackDetails: FunctionComponent<TrackDetailProps> = ({
  previousTrack,
  nextTrack,
  track,
  onClose: onCloseOverride,
}) => {
  const navigate = useNavigate();
  const onClose = onCloseOverride ?? (() => void navigate({ to: ".." }));

  const { data: trackDetails } = useRouteQuery<
    TrackDetailsDto,
    unknown,
    { id: number }
  >({
    enabled: track !== undefined,
    url: "track/{id}",
    parameters: {
      id: track?.Id as number,
    },
  });

  const availableDetails: TrackDetailsDto | TrackDto | undefined =
    trackDetails ?? track;

  if (availableDetails === undefined) {
    return <></>;
  }

  return (
    <Dialog
      className="max-sm:w-full sm:max-w-[700px] sm:w-4/5 bg-background text-text-color m-auto"
      show
      onClose={onClose}
      closeOnBackgroundClick
    >
      <Header
        track={availableDetails}
        previousTrack={previousTrack}
        nextTrack={nextTrack}
        onClose={onClose}
      />
      <Details track={availableDetails} />
    </Dialog>
  );
};

function Header({
  track,
  previousTrack,
  nextTrack,
  onClose,
}: {
  track: TrackDto | TrackDetailsDto;
  previousTrack: TrackDto | undefined;
  nextTrack: TrackDto | undefined;
  onClose: () => void;
}) {
  return (
    <div className="flex justify-between py-3 px-4 text-xl bg-paper-1">
      <div className="flex items-center gap-4">
        <CoverArt
          trackId={track.Id}
          sizeClassName="w-32 h-32"
          maxSizeClassName="max-w-32 max-h-32"
        />
        <div>
          <h1 className="text-xl">{track.Name}</h1>
          <div className="text-base">{track.Artist}</div>
          <div className="text-base">{track.Album}</div>
        </div>
      </div>
      <div className="flex items-start gap-2">
        {previousTrack && (
          <RouteIconButton
            aria-label="Previous track"
            title="Previous track"
            to="."
            params={(prev) => ({ ...prev, trackId: String(previousTrack.Id) })}
          >
            <ChevronLeftIcon />
          </RouteIconButton>
        )}
        {nextTrack && (
          <RouteIconButton
            aria-label="Next track"
            title="Next track"
            to="."
            params={(prev) => ({ ...prev, trackId: String(nextTrack.Id) })}
          >
            <ChevronRightIcon />
          </RouteIconButton>
        )}
        <IconButton aria-label="Close" title="Close" onClick={onClose}>
          <CloseIcon />
        </IconButton>
      </div>
    </div>
  );
}

function Details({ track }: { track: TrackDto }) {
  return (
    <div className="grid grid-cols-[auto_1fr] p-4 gap-x-2">
      <FieldValuePair fieldName="Name">{track.Name}</FieldValuePair>
      <FieldValuePair fieldName="Artist">{track.Artist}</FieldValuePair>
      <FieldValuePair fieldName="Album">{track.Album}</FieldValuePair>
      <FieldValuePair fieldName="Composer">{track.Composer}</FieldValuePair>
      <FieldValuePair fieldName="Genre">{track.Genre}</FieldValuePair>
      <FieldValuePair fieldName="Year">{track.Year}</FieldValuePair>
      <FieldValuePair fieldName="Track">
        {track.TrackNumber ?? "?"} of {track.TrackCount ?? "?"}
      </FieldValuePair>
      <FieldValuePair fieldName="Disc">
        {track.DiscNumber ?? "?"} of {track.DiscCount ?? "?"}
      </FieldValuePair>
      <FieldValuePair fieldName="Rating">
        <span className="flex gap-4">
          <span aria-label={`${track.Rating} stars`} className="text-primary">
            {getRatingString(track.Rating as Rating)}
          </span>
          <span className="text-love" aria-label={getLoveLabel(track.Loved)}>
            <LoveStatus loved={track.Loved} className="w-4 h-4 inline-block" />
          </span>
        </span>
      </FieldValuePair>
      <FieldValuePair fieldName="Play count">{track.PlayCount}</FieldValuePair>
    </div>
  );
}

function FieldValuePair({
  fieldName,
  children,
}: {
  fieldName: string;
  children?: ReactNode;
}) {
  return (
    <>
      <span className="font-bold">{fieldName}</span>
      <span>{children}</span>
    </>
  );
}
