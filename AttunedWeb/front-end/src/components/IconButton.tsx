import { ButtonHTMLAttributes } from "react";

export default function IconButton(
  props: ButtonHTMLAttributes<HTMLButtonElement>,
) {
  return (
    <button className="cursor-pointer hover:bg-hover h-8 w-8 rounded-full flex justify-center items-center">
      {props.children}
    </button>
  );
}
