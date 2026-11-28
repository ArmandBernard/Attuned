import { ButtonHTMLAttributes } from "react";

export default function Button({
  variant = "primary",
  className,
  ...rest
}: ButtonHTMLAttributes<HTMLButtonElement> & {
  variant?: "primary" | "secondary";
}) {
  return (
    <button
      {...rest}
      className={
        (variant === "primary"
          ? "bg-primary text-on-primary"
          : "border border-primary") +
        " cursor-pointer hover:bg-hover font-semibold px-2 py-1 rounded-md flex justify-center items-center " +
        className
      }
    >
      {rest.children}
    </button>
  );
}
