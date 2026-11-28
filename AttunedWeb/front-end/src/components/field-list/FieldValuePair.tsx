import { ReactNode } from "react";

export function FieldValuePair({
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
