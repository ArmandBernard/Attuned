import { Dialog } from "@components/Dialog.tsx";
import { FunctionComponent, ReactNode } from "react";
import { TrackDetailsDto, TrackDto } from "@dtos";
import { getRatingString } from "@utils/stringFormatters/getRatingString.ts";
import { Rating } from "@root/dtos/Rating.ts";
import { LoveStatus } from "@components/icons/LoveStatus.tsx";
import { getLoveLabel } from "@utils/stringFormatters/getLoveLabel.ts";
import { useRouteQuery } from "@utils/useRouteQuery.ts";

interface TrackDetailProps {
  show: boolean;
  onPreviousTrack: (() => void) | undefined;
  onNextTrack: (() => void) | undefined;
  onClose: () => void;
  track: TrackDto | undefined;
}

export const TrackDetails: FunctionComponent<TrackDetailProps> = ({
  show,
  onPreviousTrack,
  onNextTrack,
  onClose,
  track,
}) => {
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
      className="max-sm:w-full sm:max-w-[700px] sm:w-4/5 bg-background text-text-color"
      show={show}
      onClose={onClose}
    >
      <Header
        track={availableDetails}
        onPreviousTrack={onPreviousTrack}
        onNextTrack={onNextTrack}
        onClose={onClose}
      />
      <Details track={availableDetails} />
    </Dialog>
  );
};

function Header({
  track,
  onPreviousTrack,
  onNextTrack,
  onClose,
}: {
  track: TrackDto | TrackDetailsDto;
  onPreviousTrack: (() => void) | undefined;
  onNextTrack: (() => void) | undefined;
  onClose: () => void;
}) {
  return (
    <div className="flex justify-between py-3 px-4 text-xl bg-paper-1">
      <div className="flex items-center gap-4">
        {"CoverArt" in track ? (
          <div className="w-32 h-32 flex items-center justify-center">
            <img
              alt="album art"
              className="max-w-32 max-h-32 h-auto w-auto rounded"
              src={`data:image/png;base64,${track.CoverArt}`}
            />
          </div>
        ) : (
          <div className="w-32 h-32 rounded bg-paper-2"></div>
        )}
        <div>
          <h1 className="text-xl">{track.Name}</h1>
          <div className="text-base">{track.Artist}</div>
          <div className="text-base">{track.Album}</div>
        </div>
      </div>
      <div className="flex items-start gap-4">
        {onPreviousTrack && (
          <button
            aria-label="Previous track"
            title="Previous track"
            onClick={onPreviousTrack}
          >
            ⮜
          </button>
        )}
        {onNextTrack && (
          <button
            aria-label="Next track"
            title="Next track"
            onClick={onNextTrack}
          >
            ⮞
          </button>
        )}
        <button aria-label="Close" title="Close" onClick={onClose}>
          ✕
        </button>
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
