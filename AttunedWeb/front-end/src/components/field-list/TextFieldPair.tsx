import { Dispatch, SetStateAction } from "react";
import { TrackPatchDto } from "@dtos";
import { TextField } from "@components/fields/TextField.tsx";
import { FieldValuePair } from "@components/field-list/FieldValuePair.tsx";

export function TextFieldPair<T extends object>({
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
            [field]: e.target.value === "" ? null : e.target.value,
          }))
        }
      />
    </FieldValuePair>
  );
}
