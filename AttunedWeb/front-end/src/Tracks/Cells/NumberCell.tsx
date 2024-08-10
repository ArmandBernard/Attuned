import { CellComponent } from "./CellComponent.ts";

export const NumberCell: CellComponent<number | undefined> = (props) => {
  const { className, value, ...rest } = props;

  const classNames = ["px-2 truncate text-end", className]
    .filter(Boolean)
    .join(" ");

  return (
    <td className={classNames} style={{ contain: "strict" }} {...rest}>
      {value}
    </td>
  );
};
