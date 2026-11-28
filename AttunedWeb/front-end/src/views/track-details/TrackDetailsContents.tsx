import { useContext, useState } from "react";
import { TrackDto, TrackPatchDto } from "@dtos";
import { EditingContext } from "@utils/EditingContext.ts";
import { useQueryClient } from "@tanstack/react-query";
import { useRouteMutation } from "@utils/useRouteMutation.ts";
import { RatingField } from "@components/fields/RatingField.tsx";
import { Rating } from "@root/dtos/Rating.ts";
import { getLoveLabel } from "@utils/stringFormatters/getLoveLabel.ts";
import { LoveStatus } from "@components/icons/LoveStatus.tsx";
import { ErrorAlert } from "@components/ErrorAlert.tsx";
import Button from "@components/Button.tsx";
import { FieldValuePair } from "@components/field-list/FieldValuePair.tsx";
import { XOfYFieldPair } from "@components/field-list/XOfYFieldPair.tsx";
import { TextFieldPair } from "@components/field-list/TextFieldPair.tsx";

export function TrackDetailsContents({ track }: { track: TrackDto }) {
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
      <dl className="grid grid-cols-[auto_1fr] gap-x-4 gap-y-1">
        <TextFieldPair item={modified} field="Name" patch={setPatchDto} />
        <TextFieldPair item={modified} field="Artist" patch={setPatchDto} />
        <TextFieldPair item={modified} field="Album" patch={setPatchDto} />
        <TextFieldPair item={modified} field="Composer" patch={setPatchDto} />
        <TextFieldPair item={modified} field="Genre" patch={setPatchDto} />
        <TextFieldPair item={modified} field="Year" patch={setPatchDto} />

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
          <span className="flex gap-4 items-center">
            <RatingField
              value={modified.Rating as Rating}
              onChange={(Rating) =>
                setPatchDto((previous) => ({ ...previous, Rating }))
              }
            />
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
