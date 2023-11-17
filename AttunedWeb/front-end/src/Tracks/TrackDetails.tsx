import { Dialog } from "../Dialog.tsx";
import { FunctionComponent, ReactNode } from "react";
import { TrackDto } from "../dtos/Dtos.ts";
import { getRatingString } from "../StringFormatters/getRatingString.ts";
import { Rating } from "../dtos/Rating.ts";
import { LoveStatus } from "../Icons/LoveStatus.tsx";
import { getLoveLabel } from "../StringFormatters/getLoveLabel.ts";

interface TrackDetailProps {
  show: boolean;
  onClose: () => void;
  track: TrackDto | undefined;
}

export const TrackDetails: FunctionComponent<TrackDetailProps> = ({
  show,
  onClose,
  track,
}) => {
  return (
    <Dialog
      className="max-sm:w-full sm:max-w-[700px] sm:w-4/5 bg-background text-text-color"
      show={show}
      onClose={onClose}
    >
      <div className="flex justify-between py-3 px-4 text-xl">
        <h1>{track === undefined ? "No track selected" : track.Name}</h1>
        <button onClick={onClose}>✕</button>
      </div>
      {track !== undefined && (
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
              <span
                aria-label={`${track.Rating} stars`}
                className="text-primary"
              >
                {getRatingString(track.Rating as Rating)}
              </span>
              <span
                className="text-love"
                aria-label={getLoveLabel(track.Loved)}
              >
                <LoveStatus loved={track.Loved} className="w-4 h-4 inline-block" />
              </span>
            </span>
          </FieldValuePair>
          <FieldValuePair fieldName="Play count">
            {track.PlayCount}
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
