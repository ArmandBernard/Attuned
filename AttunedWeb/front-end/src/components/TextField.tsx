import { InputHTMLAttributes, useContext } from "react";
import { EditingContext } from "@utils/EditingContext.ts";

export function TextField(props: InputHTMLAttributes<HTMLInputElement>) {
  const [editing] = useContext(EditingContext);

  if (!editing) {
    return <span className={props.className}>{props.value}</span>;
  }

  return <input className="border" {...props} />;
}
