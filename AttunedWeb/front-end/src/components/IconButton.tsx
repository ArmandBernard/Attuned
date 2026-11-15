import { ButtonHTMLAttributes } from "react";

export default function IconButton(
  props: ButtonHTMLAttributes<HTMLButtonElement>,
) {
  return (
    <button
      {...props}
      className={
        "cursor-pointer hover:bg-hover h-8 w-8 rounded-full flex justify-center items-center " +
        props.className
      }
    >
      {props.children}
    </button>
  );
}
