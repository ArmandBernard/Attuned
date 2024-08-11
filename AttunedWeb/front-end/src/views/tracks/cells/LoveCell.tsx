import { CellComponent } from "./CellComponent.ts";
import { LoveStatus } from "@components/icons/LoveStatus.tsx";

export const LoveCell: CellComponent<boolean | undefined> = (props) => {
  const { className, value, ...rest } = props;

  const classNames = ["px-2 text-love text-center", className]
    .filter(Boolean)
    .join(" ");

  return (
    <td aria-label={`${value} stars`} className={classNames} {...rest}>
      <LoveStatus loved={value} className="w-4 h-4 inline-block" />
    </td>
  );
};
