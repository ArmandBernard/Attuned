import { FunctionComponent, HTMLProps } from "react";

export const TextCell: FunctionComponent<HTMLProps<HTMLTableCellElement>> = (
  props,
) => {
  const { className, ...rest } = props;

  const classNames = ["px-2 truncate",className].filter(Boolean).join(" ");

  return <td className={classNames} {...rest}></td>;
};
