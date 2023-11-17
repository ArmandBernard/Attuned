import {CellComponent} from "./CellComponent.ts";
import {getLoveString} from "../../StringFormatters/getLoveString.ts";

export const LoveCell: CellComponent<boolean | undefined> = (props) => {
  const { className, value, ...rest } = props;

  const classNames = ["px-2 whitespace-nowrap text-primary text-lg", className]
    .filter(Boolean)
    .join(" ");

  return (
    <td aria-label={`${value} stars`} className={classNames} {...rest}>
      {getLoveString(value)}
    </td>
  );
};