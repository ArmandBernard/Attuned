import { CellComponent } from "./CellComponent.ts";

export const RatingCell: CellComponent<number> = (props) => {
  const { className, value, ...rest } = props;

  const classNames = ["px-2 whitespace-nowrap text-primary text-lg", className]
    .filter(Boolean)
    .join(" ");

  return (
    <td className={classNames} {...rest}>
      {Array(value).fill("★")}
      {Array(5 - value).fill("☆")}
    </td>
  );
};
