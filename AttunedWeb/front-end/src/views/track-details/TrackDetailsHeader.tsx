import { TrackDetailsDto, TrackDto } from "@dtos";
import { Dispatch, SetStateAction } from "react";
import { CoverArt } from "@components/CoverArt.tsx";
import RouteIconButton from "@components/RouteIconButton.tsx";
import ChevronLeftIcon from "@components/icons/ChevronLeftIcon.tsx";
import ChevronRightIcon from "@components/icons/ChevronRightIcon.tsx";
import IconButton from "@components/IconButton.tsx";
import EditIcon from "@components/icons/EditIcon.tsx";
import CloseIcon from "@components/icons/CloseIcon.tsx";

export function TrackDetailsHeader({
  track,
  previousTrack,
  nextTrack,
  onClose,
  editing,
  setEditing,
}: {
  track: TrackDto | TrackDetailsDto;
  previousTrack: TrackDto | undefined;
  nextTrack: TrackDto | undefined;
  onClose: () => void;
  editing: boolean;
  setEditing: Dispatch<SetStateAction<boolean>>;
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
        {!editing && (
          <>
            {previousTrack && (
              <RouteIconButton
                aria-label="Previous track"
                title="Previous track"
                to="."
                params={(prev) => ({
                  ...prev,
                  trackId: String(previousTrack.Id),
                })}
                search
                resetScroll={false}
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
                search
                resetScroll={false}
              >
                <ChevronRightIcon />
              </RouteIconButton>
            )}
            <IconButton
              aria-label="Edit"
              title="Edit"
              onClick={() => setEditing(true)}
            >
              <EditIcon />
            </IconButton>
          </>
        )}
        <IconButton aria-label="Close" title="Close" onClick={onClose}>
          <CloseIcon />
        </IconButton>
      </div>
    </div>
  );
}
