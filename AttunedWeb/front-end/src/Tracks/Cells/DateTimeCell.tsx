import { CellComponent } from "./CellComponent.ts";

export const DateTimeCell: CellComponent<number | undefined> = (props) => {
  const { className, value, ...rest } = props;

  const classNames = ["px-2 whitespace-nowrap flex justify-end", className].filter(Boolean).join(" ");

  return (
    <td className={classNames} {...rest}>
      {value && new Date(value).toLocaleString()}
    </td>
  );
};
