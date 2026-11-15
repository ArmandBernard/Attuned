import { ButtonHTMLAttributes } from "react";

export default function LinkButton(
  props: ButtonHTMLAttributes<HTMLButtonElement>,
) {
  return (
    <button
      {...props}
      className={
        "text-primary underline decoration-current/50 hover:decoration-current cursor-pointer " +
        props.className
      }
    />
  );
}
