import { InputHTMLAttributes, useContext } from "react";
import { EditingContext } from "@utils/EditingContext.ts";

export function TextField(props: InputHTMLAttributes<HTMLInputElement>) {
  const [editing] = useContext(EditingContext);

  if (!editing) {
    return <div className={"py-px " + props.className}>{props.value}</div>;
  }

  return <input {...props} className={"border " + props.className} />;
}
