import { Dialog } from "@components/Dialog.tsx";
import { FunctionComponent, useState } from "react";
import { TrackDetailsDto, TrackDto } from "@dtos";
import { useRouteQuery } from "@utils/useRouteQuery.ts";
import { useNavigate } from "@tanstack/react-router";
import { EditingContext } from "@utils/EditingContext.ts";
import { TrackDetailsHeader } from "@views/track-details/TrackDetailsHeader.tsx";
import { TrackDetailsContents } from "@views/track-details/TrackDetailsContents.tsx";

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
  const [editing, setEditing] = useState(false);
  function handleClose() {
    setEditing(false);

    if (onCloseOverride) {
      onCloseOverride();
    } else {
      // keep the search params, as the album that is open is in them
      void navigate({ to: "..", search: true });
    }
  }

  const { data: trackDetails } = useRouteQuery<TrackDetailsDto, { id: number }>(
    {
      enabled: track !== undefined,
      url: "track/{id}",
      parameters: {
        id: track?.Id as number,
      },
    },
  );

  const availableDetails: TrackDetailsDto | TrackDto | undefined =
    trackDetails ?? track;

  if (availableDetails === undefined) {
    return <></>;
  }

  return (
    <Dialog
      className="max-sm:w-full sm:max-w-[700px] sm:w-4/5 bg-background text-text-color m-auto"
      show
      onClose={handleClose}
      closeOnBackgroundClick
    >
      <TrackDetailsHeader
        track={availableDetails}
        previousTrack={previousTrack}
        nextTrack={nextTrack}
        onClose={handleClose}
        editing={editing}
        setEditing={setEditing}
      />
      <EditingContext.Provider value={[editing, setEditing]}>
        <TrackDetailsContents track={availableDetails} />
      </EditingContext.Provider>
    </Dialog>
  );
};
