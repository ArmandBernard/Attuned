import { Rating } from "@root/dtos/Rating.ts";
import StarFilledIcon from "@components/icons/StarFilledIcon.tsx";
import StarEmptyIcon from "@components/icons/StarEmptyIcon.tsx";
import { HTMLAttributes } from "react";

export function ReadonlyRating({
  value,
  className,
  ...rest
}: { value: Rating } & HTMLAttributes<HTMLElement>) {
  return (
    <div
      aria-label={`${value} stars`}
      title={`${value} stars`}
      className={`inline-flex items-center align-middle ${className}`}
      {...rest}
    >
      {[1, 2, 3, 4, 5].map((star) => {
        const Icon = star <= value ? StarFilledIcon : StarEmptyIcon;

        return (
          <span key={star} className="flex rounded-full p-0">
            <Icon className="w-5 h-5 fill-current" aria-hidden />
          </span>
        );
      })}
    </div>
  );
}
