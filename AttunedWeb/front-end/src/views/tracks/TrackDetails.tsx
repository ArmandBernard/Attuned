import { Dialog } from "@components/Dialog.tsx";
import {
  Dispatch,
  FunctionComponent,
  ReactNode,
  SetStateAction,
  useContext,
  useState,
} from "react";
import { TrackDetailsDto, TrackDto, TrackPatchDto } from "@dtos";
import { getRatingString } from "@utils/stringFormatters/getRatingString.ts";
import { Rating } from "@root/dtos/Rating.ts";
import { LoveStatus } from "@components/icons/LoveStatus.tsx";
import { getLoveLabel } from "@utils/stringFormatters/getLoveLabel.ts";
import { useRouteQuery } from "@utils/useRouteQuery.ts";
import CloseIcon from "@components/icons/CloseIcon.tsx";
import IconButton from "@components/IconButton.tsx";
import ChevronRightIcon from "@components/icons/ChevronRightIcon.tsx";
import ChevronLeftIcon from "@components/icons/ChevronLeftIcon.tsx";
import { CoverArt } from "@components/CoverArt.tsx";
import { useNavigate } from "@tanstack/react-router";
import RouteIconButton from "@components/RouteIconButton.tsx";
import EditIcon from "@components/icons/EditIcon.tsx";
import { EditingContext } from "@utils/EditingContext.ts";
import { TextField } from "@components/TextField.tsx";
import Button from "@components/Button.tsx";
import { useRouteMutation } from "@utils/useRouteMutation.ts";
import { ErrorAlert } from "@components/ErrorAlert.tsx";
import { useQueryClient } from "@tanstack/react-query";

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
      void navigate({ to: ".." });
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
      <Header
        track={availableDetails}
        previousTrack={previousTrack}
        nextTrack={nextTrack}
        onClose={handleClose}
        editing={editing}
        setEditing={setEditing}
      />
      <EditingContext.Provider value={[editing, setEditing]}>
        <Details track={availableDetails} />
      </EditingContext.Provider>
    </Dialog>
  );
};

function Header({
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

function Details({ track }: { track: TrackDto }) {
  const [patchDto, setPatchDto] = useState<TrackPatchDto>({});
  const [editing, setEditing] = useContext(EditingContext);

  const modified = { ...track, ...patchDto };

  const queryClient = useQueryClient();

  const {
    mutateAsync: patchTrack,
    isPending: patchTrackIsPending,
    error: patchTrackError,
    reset: resetPatch,
  } = useRouteMutation<TrackDto, { id: number }, TrackPatchDto>({
    url: "track/{id}",
    method: "PATCH",
  });

  async function handleSave() {
    await patchTrack({
      body: patchDto,
      parameters: {
        id: track.Id,
      },
    });
    setPatchDto({});
    setEditing(false);
    await queryClient.invalidateQueries({ queryKey: ["track/{id}"] });
    await queryClient.invalidateQueries({ queryKey: ["track"] });
  }

  function handleCancel() {
    setEditing(false);
    setPatchDto({});
    resetPatch();
  }

  return (
    <div className="flex flex-col p-4 gap-4">
      <dl className="grid grid-cols-[auto_1fr] gap-x-4">
        <TextFieldPair item={modified} field="Name" patch={setPatchDto} />
        <TextFieldPair item={modified} field="Artist" patch={setPatchDto} />
        <TextFieldPair item={modified} field="Album" patch={setPatchDto} />
        <TextFieldPair item={modified} field="Composer" patch={setPatchDto} />
        <TextFieldPair item={modified} field="Genre" patch={setPatchDto} />
        <FieldValuePair fieldName="Year">{modified.Year}</FieldValuePair>

        <XOfYFieldPair
          item={modified}
          fieldName="Track"
          field1="TrackNumber"
          field2="TrackCount"
          patch={setPatchDto}
        />
        <XOfYFieldPair
          item={modified}
          fieldName="Disc"
          field1="DiscNumber"
          field2="DiscCount"
          patch={setPatchDto}
        />
        <FieldValuePair fieldName="Rating">
          <span className="flex gap-4">
            <span
              aria-label={`${modified.Rating} stars`}
              className="text-primary"
            >
              {getRatingString(modified.Rating as Rating)}
            </span>
            <span className="text-love" aria-label={getLoveLabel(track.Loved)}>
              <LoveStatus
                loved={track.Loved}
                className="w-4 h-4 inline-block"
              />
            </span>
          </span>
        </FieldValuePair>
        <FieldValuePair fieldName="Play count">
          {modified.PlayCount}
        </FieldValuePair>
      </dl>

      <ErrorAlert error={patchTrackError} />

      {editing && (
        <div className="self-end flex gap-2">
          <Button
            variant="secondary"
            disabled={patchTrackIsPending}
            onClick={handleCancel}
          >
            Cancel
          </Button>
          <Button
            variant="primary"
            disabled={patchTrackIsPending}
            onClick={handleSave}
          >
            Save
          </Button>
        </div>
      )}
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
      <dt className="font-bold">{fieldName}</dt>
      <dd>{children}</dd>
    </>
  );
}

function TextFieldPair<T extends object>({
  item,
  field,
  fieldName,
  patch,
}: {
  item: T;
  field: keyof T;
  fieldName?: string;
  patch: Dispatch<SetStateAction<TrackPatchDto>>;
}) {
  return (
    <FieldValuePair fieldName={fieldName ?? (field as string)}>
      <TextField
        className="w-full"
        value={(item[field] as string) ?? ""}
        onChange={(e) =>
          patch((modifications) => ({
            ...modifications,
            [field]: e.target.value,
          }))
        }
      />
    </FieldValuePair>
  );
}

function XOfYFieldPair<T extends object>({
  item,
  field1,
  field2,
  fieldName,
  patch,
}: {
  item: T;
  field1: keyof T;
  field2: keyof T;
  fieldName?: string;
  patch: Dispatch<SetStateAction<TrackPatchDto>>;
}) {
  const [editing] = useContext(EditingContext);

  return (
    <FieldValuePair fieldName={fieldName ?? (field1 as string)}>
      <TextField
        className="w-16 inline"
        inputMode="numeric"
        value={(item[field1] as string) ?? (editing ? "" : "?")}
        onChange={(e) =>
          patch((modifications) => ({
            ...modifications,
            [field1]: e.target.value,
          }))
        }
      />
      <span> of </span>
      <TextField
        className="w-16 inline"
        inputMode="numeric"
        value={(item[field2] as string) ?? (editing ? "" : "?")}
        onChange={(e) =>
          patch((modifications) => ({
            ...modifications,
            [field2]: e.target.value,
          }))
        }
      />
    </FieldValuePair>
  );
}
