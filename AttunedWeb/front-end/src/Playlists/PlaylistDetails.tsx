import { FunctionComponent } from "react";
import { PlaylistDto } from "../dtos/Dtos.ts";
import { Dialog } from "../Dialog.tsx";
import {Limits} from "./Limits.tsx";

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
      className="w-1/2 bg-background text-text-color"
      show={show}
      onClose={onClose}
    >
      <div className="flex justify-between py-3 px-4 text-xl">
        <h1 className="">{playlist.Name}</h1>
        <button onClick={onClose}>✕</button>
      </div>
      <div className="p-4">
        <label>
          <input
            disabled
            type="checkbox"
            checked={playlist.RuleConjunction !== undefined}
          />{" "}
          Match music for the following rules:
        </label>
        <Limits playlist={playlist} />
      </div>
    </Dialog>
  );
};
