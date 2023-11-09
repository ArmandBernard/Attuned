import { Dialog } from "../Dialog.tsx";
import { FunctionComponent, ReactNode } from "react";
import { TrackDto } from "../dtos/Dtos.ts";

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
      className="max-sm:w-full sm:max-w-[1000px] sm:w-4/5 bg-background text-text-color"
      show={show}
      onClose={onClose}
    >
      <div className="flex justify-between py-3 px-4 text-xl">
        <h1 className="">{track === undefined ? "No track selected" : track.Name }</h1>
        <button onClick={onClose}>✕</button>
      </div>      
      {track !== undefined && (
        <div className="grid grid-cols-[auto_1fr] p-4 gap-x-2">
          <FieldValuePair fieldName="Name">{track.Name}</FieldValuePair>
        </div>
      )}
    </Dialog>
  );
};

const FieldValuePair = ({
  fieldName,
  children
}: {
  fieldName: string;
  children?: ReactNode;
}) => (
  <>
    <span className="font-bold">{fieldName}</span>
    {children}
  </>
);
