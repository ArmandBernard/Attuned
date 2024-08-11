import { FunctionComponent } from "react";
import { PlaylistDetailsDto } from "../dtos/Dtos.ts";

export const Limits: FunctionComponent<{ playlist: PlaylistDetailsDto }> = ({
  playlist,
}) => {
  return (
    <>
      <label>
        <input type="checkbox" disabled checked={playlist.Limit?.IsLimited} />{" "}
        Limit to {playlist.Limit?.Amount} {playlist.Limit?.Units} selected by{" "}
        {playlist.Limit?.SortField}{" "}
        {playlist.Limit?.SortDirection.toLowerCase()}
      </label>
      <label>
        <input type="checkbox" disabled checked={playlist.Limit?.OnlyChecked} />{" "}
        Match only ticked items
      </label>
      <label>
        <input type="checkbox" disabled checked={playlist.LiveUpdating} /> Live
        updating
      </label>
    </>
  );
};
