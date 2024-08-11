import { CellComponent } from "./CellComponent.ts";
import { getRatingString } from "@utils/stringFormatters/getRatingString.ts";
import { Rating } from "@root/dtos/Rating.ts";

export const RatingCell: CellComponent<Rating> = (props) => {
  const { className, value, ...rest } = props;

  const classNames = ["px-2 whitespace-nowrap text-primary text-lg", className]
    .filter(Boolean)
    .join(" ");

  return (
    <td aria-label={`${value} stars`} className={classNames} {...rest}>
      {getRatingString(value)}
    </td>
  );
};
