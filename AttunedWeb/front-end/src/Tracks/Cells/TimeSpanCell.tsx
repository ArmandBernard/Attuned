import { CellComponent } from "./CellComponent.ts";
import { TimeSpan } from "../../dtos/TimeSpan.ts";
import {timeSpanToTimeString} from "../../StringFormatters/timeSpanToTimeString.ts";

export const TimeSpanCell: CellComponent<TimeSpan | undefined> = (props) => {
  const { className, value, ...rest } = props;

  const classNames = ["px-2 truncate text-end", className].filter(Boolean).join(" ");

  return (
    <td className={classNames} {...rest}>
      {value !== undefined && timeSpanToTimeString(value)}
    </td>
  );
};