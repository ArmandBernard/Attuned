import { FunctionComponent, HTMLProps } from "react";

export type CellComponent<T> = FunctionComponent<
  Omit<HTMLProps<HTMLTableCellElement>, "children" | "value"> & {
    value: T;
  }
>;
