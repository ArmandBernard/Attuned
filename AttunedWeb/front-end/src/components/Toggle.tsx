import { ButtonHTMLAttributes } from "react";

type ToggleProps = Omit<
  ButtonHTMLAttributes<HTMLButtonElement>,
  "onChange" | "role" | "type"
> & {
  /**
   * Whether the toggle is on
   */
  checked: boolean;
  /**
   * Called with the new value when the user flips the toggle
   */
  onChange: (checked: boolean) => void;
};

/**
 * A basic on/off switch. Provide an accessible name with `aria-label` or
 * `aria-labelledby`.
 */
export default function Toggle({
  checked,
  onChange,
  className,
  ...rest
}: ToggleProps) {
  return (
    <button
      {...rest}
      type="button"
      role="switch"
      aria-checked={checked}
      onClick={() => onChange(!checked)}
      className={
        "cursor-pointer relative h-4 w-7 shrink-0 rounded-full border border-primary transition-colors " +
        (checked ? "bg-primary " : "bg-background ") +
        (className ?? "")
      }
    >
      <span
        className={
          "absolute top-0.5 left-0.5 h-2.5 w-2.5 rounded-full transition-transform " +
          (checked ? "translate-x-3 bg-background" : "translate-x-0 bg-primary")
        }
      />
    </button>
  );
}
