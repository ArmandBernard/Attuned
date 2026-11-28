import { ButtonHTMLAttributes } from "react";

export default function Button({
  variant = "primary",
  className,
  disabled,
  ...rest
}: ButtonHTMLAttributes<HTMLButtonElement> & {
  variant?: "primary" | "secondary";
}) {
  const classNames = [
    "px-2 py-1 rounded-md cursor-pointer aria-disabled:cursor-default font-semibold",
    variant === "primary"
      ? "bg-primary hover:opacity-80 aria-disabled:opacity-50 text-on-primary"
      : "border border-primary hover:bg-hover aria-disabled:opacity-50 aria-disabled:bg-transparent",
    className,
  ];

  return (
    <button
      {...rest}
      aria-disabled={disabled}
      className={classNames.join(" ")}
      onClick={(e) => {
        if (!disabled && rest.onClick) {
          rest.onClick(e);
        }
      }}
      onKeyDown={(e) => {
        if (!disabled && rest.onKeyDown) {
          rest.onKeyDown(e);
        }
      }}
    >
      {rest.children}
    </button>
  );
}
