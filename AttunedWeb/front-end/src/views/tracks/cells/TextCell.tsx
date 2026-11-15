import { CellComponent } from "./CellComponent.ts";
import { ReactNode } from "react";

export const TextCell: CellComponent<string | ReactNode | undefined> = (
  props,
) => {
  const { className, value, ...rest } = props;

  const classNames = ["px-2 truncate", className].filter(Boolean).join(" ");

  return (
    <td className={classNames} {...rest}>
      {value}
    </td>
  );
};
