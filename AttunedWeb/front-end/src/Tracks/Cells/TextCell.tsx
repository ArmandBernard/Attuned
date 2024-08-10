import { CellComponent } from "./CellComponent.ts";

export const TextCell: CellComponent<string | undefined> = (props) => {
  const { className, value, ...rest } = props;

  const classNames = ["px-2 truncate", className].filter(Boolean).join(" ");

  return (
    <td className={classNames} style={{ contain: "strict" }} {...rest}>
      {value}
    </td>
  );
};
