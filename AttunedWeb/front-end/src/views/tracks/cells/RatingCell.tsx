import { CellComponent } from "./CellComponent.ts";
import { Rating } from "@root/dtos/Rating.ts";
import { ReadonlyRating } from "@components/ReadonlyRating.tsx";

export const RatingCell: CellComponent<Rating> = (props) => {
  const { className, value, ...rest } = props;

  const classNames = ["px-2 truncate", className].filter(Boolean).join(" ");

  return (
    <td className={classNames} {...rest}>
      <ReadonlyRating value={value} />
    </td>
  );
};
