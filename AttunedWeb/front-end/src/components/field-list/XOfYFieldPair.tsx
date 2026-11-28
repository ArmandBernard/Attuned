import { Dispatch, SetStateAction, useContext } from "react";
import { TrackPatchDto } from "@dtos";
import { EditingContext } from "@utils/EditingContext.ts";
import { TextField } from "@components/TextField.tsx";
import { FieldValuePair } from "@components/field-list/FieldValuePair.tsx";

export function XOfYFieldPair<T extends object>({
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
            [field1]: e.target.value === "" ? null : e.target.value,
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
            [field2]: e.target.value === "" ? null : e.target.value,
          }))
        }
      />
    </FieldValuePair>
  );
}
