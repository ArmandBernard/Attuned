import { FunctionComponent } from "react";
import { PlaylistDto } from "../dtos/Dtos.ts";

export const Limits: FunctionComponent<{ playlist: PlaylistDto }> = ({
  playlist,
}) => {
  return (
    <>
      <div>
        <label>
          <input type="checkbox" disabled checked={playlist.Limit?.IsLimited} />{" "}
          Limit to {playlist.Limit?.Amount} {playlist.Limit?.Units} selected by{" "}
          {playlist.Limit?.SortField}{" "}
          {playlist.Limit?.SortDirection.toLowerCase()}
        </label>
      </div>
      <div>
        <label>
          <input
            type="checkbox"
            disabled
            checked={playlist.Limit?.OnlyChecked}
          />{" "}
          Match only ticked items
        </label>
      </div>
      <div>
        <label>
          <input type="checkbox" disabled checked={playlist.LiveUpdating} />{" "}
          Live updating
        </label>
      </div>
    </>
  );
};
