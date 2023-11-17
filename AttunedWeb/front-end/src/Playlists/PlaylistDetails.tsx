import { FunctionComponent } from "react";
import { PlaylistDto } from "../dtos/Dtos.ts";
import { Dialog } from "../Dialog.tsx";
import { Limits } from "./Limits.tsx";
import { Conjunctions } from "./Conjunctions.tsx";

interface PlaylistDetailsProps {
  playlist: PlaylistDto;
  show: boolean;
  onClose: () => void;
}

export const PlaylistDetails: FunctionComponent<PlaylistDetailsProps> = ({
  playlist,
  show,
  onClose,
}) => {
  return (
    <Dialog
      className="max-sm:w-full sm:max-w-[1000px] sm:w-4/5 bg-background text-text-color"
      show={show}
      onClose={onClose}
    >
      <div className="flex justify-between py-3 px-4 text-xl">
        <h1>{playlist.Name}</h1>
        <button onClick={onClose}>✕</button>
      </div>
      <div className="flex flex-col gap-2 px-4 pb-4">
        <label>
          <input
            disabled
            type="checkbox"
            checked={playlist.RuleConjunction !== undefined}
          />{" "}
          Match music for the following rules:
        </label>
        {playlist.RuleConjunction && (
          <div className="border p-2">
            <Conjunctions conjunction={playlist.RuleConjunction} />
          </div>
        )}
        <Limits playlist={playlist} />
      </div>
    </Dialog>
  );
};
