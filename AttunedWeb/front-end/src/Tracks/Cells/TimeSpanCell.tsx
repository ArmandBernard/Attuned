import { CellComponent } from "./CellComponent.ts";
import { TimeSpan } from "../../dtos/TimeSpan.ts";

export const TimeSpanCell: CellComponent<TimeSpan | undefined> = (props) => {
  const { className, value, ...rest } = props;

  const classNames = ["px-2 truncate flex justify-end", className].filter(Boolean).join(" ");

  return (
    <td className={classNames} {...rest}>
      {value !== undefined && timeSpanToTimeString(value)}
    </td>
  );
};

function timeSpanToTimeString(timeSpan: TimeSpan) {
  const ISOTime = new Date(timeSpan).toISOString().slice(11, -5);
  
  // remove leading 00: if present and any leading minutes zero
  return ISOTime.replace(/(00:)?0?/, "");
}
