import { CellComponent } from "./CellComponent.ts";

export const NumberCell: CellComponent<number | undefined> = (props) => {
  const { className, value, ...rest } = props;

  const classNames = ["px-2 truncate flex justify-end", className].filter(Boolean).join(" ");

  return (
    <td className={classNames} {...rest}>
      {value}
    </td>
  );
};
