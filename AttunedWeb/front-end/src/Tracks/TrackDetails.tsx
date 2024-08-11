import { Dialog } from "../Dialog.tsx";
import { FunctionComponent, ReactNode } from "react";
import { TrackDetailsDto, TrackDto } from "../dtos/Dtos.ts";
import { getRatingString } from "../StringFormatters/getRatingString.ts";
import { Rating } from "../dtos/Rating.ts";
import { LoveStatus } from "../Icons/LoveStatus.tsx";
import { getLoveLabel } from "../StringFormatters/getLoveLabel.ts";
import { useRouteQuery } from "../Queries/useRouteQuery.ts";

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

  return (
    <Dialog
      className="max-sm:w-full sm:max-w-[700px] sm:w-4/5 bg-background text-text-color"
      show={show}
      onClose={onClose}
    >
      <div className="flex justify-between py-3 px-4 text-xl">
        <h1>
          {availableDetails === undefined
            ? "No track selected"
            : availableDetails.Name}
        </h1>
        <div className="flex gap-4">
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
      {availableDetails !== undefined && (
        <div className="grid grid-cols-[auto_1fr] p-4 gap-x-2">
          <FieldValuePair fieldName="Name">
            {availableDetails.Name}
          </FieldValuePair>
          <FieldValuePair fieldName="Artist">
            {availableDetails.Artist}
          </FieldValuePair>
          <FieldValuePair fieldName="Album">
            {availableDetails.Album}
          </FieldValuePair>
          <FieldValuePair fieldName="Composer">
            {availableDetails.Composer}
          </FieldValuePair>
          <FieldValuePair fieldName="Genre">
            {availableDetails.Genre}
          </FieldValuePair>
          <FieldValuePair fieldName="Year">
            {availableDetails.Year}
          </FieldValuePair>
          <FieldValuePair fieldName="Track">
            {availableDetails.TrackNumber ?? "?"} of{" "}
            {availableDetails.TrackCount ?? "?"}
          </FieldValuePair>
          <FieldValuePair fieldName="Disc">
            {availableDetails.DiscNumber ?? "?"} of{" "}
            {availableDetails.DiscCount ?? "?"}
          </FieldValuePair>
          <FieldValuePair fieldName="Rating">
            <span className="flex gap-4">
              <span
                aria-label={`${availableDetails.Rating} stars`}
                className="text-primary"
              >
                {getRatingString(availableDetails.Rating as Rating)}
              </span>
              <span
                className="text-love"
                aria-label={getLoveLabel(availableDetails.Loved)}
              >
                <LoveStatus
                  loved={availableDetails.Loved}
                  className="w-4 h-4 inline-block"
                />
              </span>
            </span>
          </FieldValuePair>
          <FieldValuePair fieldName="Play count">
            {availableDetails.PlayCount}
          </FieldValuePair>
        </div>
      )}
    </Dialog>
  );
};

const FieldValuePair = ({
  fieldName,
  children,
}: {
  fieldName: string;
  children?: ReactNode;
}) => (
  <>
    <span className="font-bold">{fieldName}</span>
    <span>{children}</span>
  </>
);
