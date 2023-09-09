import { FunctionComponent, HTMLProps } from "react";

export const TextCell: FunctionComponent<
  Omit<HTMLProps<HTMLTableCellElement>, "children"> & { value: string }
> = (props) => {
  const { className, value, ...rest } = props;

  const classNames = ["px-2 truncate", className].filter(Boolean).join(" ");

  return (
    <td className={classNames} {...rest}>
      {value}
    </td>
  );
};
