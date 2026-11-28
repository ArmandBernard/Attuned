import { ReactNode, useContext, useId } from "react";
import { Rating } from "@root/dtos/Rating.ts";
import { EditingContext } from "@utils/EditingContext.ts";
import { ReadonlyRating } from "@components/ReadonlyRating.tsx";
import StarFilledIcon from "@components/icons/StarFilledIcon.tsx";
import StarEmptyIcon from "@components/icons/StarEmptyIcon.tsx";
import CloseIcon from "@components/icons/CloseIcon.tsx";

/**
 * A group of native radio buttons, drawn as stars.
 * The radios do the accessibility work: one tab stop, arrow keys to change
 * the value, and "n of 6, checked" announcements.
 */
export function RatingField({
  value,
  onChange,
  label = "Rating",
}: {
  value: Rating;
  onChange: (value: Rating) => void;
  label?: string;
}) {
  const [editing] = useContext(EditingContext);

  if (!editing) {
    return <ReadonlyRating value={value} className="h-6.5" />;
  }

  return <RatingInput value={value} onChange={onChange} label={label} />;
}

const STARS: Rating[] = [1, 2, 3, 4, 5];

function RatingInput({
  value,
  onChange,
  label = "Rating",
}: {
  value: Rating;
  onChange: (value: Rating) => void;
  label?: string;
}) {
  const name = useId();

  return (
    <div
      role="radiogroup"
      aria-label={label}
      className="flex items-center h-6.5" // match height to text fields
    >
      <RatingOption
        name={name}
        rating={0}
        checked={value === 0}
        onChange={onChange}
        text="No rating"
      >
        <CloseIcon className="w-5 h-5 fill-current" aria-hidden />
      </RatingOption>
      {STARS.map((star) => {
        const Icon = star <= value ? StarFilledIcon : StarEmptyIcon;

        return (
          <RatingOption
            key={star}
            name={name}
            rating={star}
            checked={value === star}
            onChange={onChange}
            text={`${star} ${star === 1 ? "star" : "stars"}`}
          >
            <Icon className="w-5 h-5 fill-current text-primary" aria-hidden />
          </RatingOption>
        );
      })}
    </div>
  );
}

function RatingOption({
  name,
  rating,
  checked,
  onChange,
  text,
  children,
}: {
  name: string;
  rating: Rating;
  checked: boolean;
  onChange: (value: Rating) => void;
  text: string;
  children: ReactNode;
}) {
  return (
    <label className="cursor-pointer" title={text}>
      <input
        type="radio"
        name={name}
        className="peer sr-only"
        checked={checked}
        onChange={() => onChange(rating)}
      />
      <span className="flex rounded-full p-0 hover:bg-hover peer-focus-visible:outline-2 peer-focus-visible:outline-offset-1 peer-focus-visible:outline-primary">
        {children}
        <span className="sr-only">{text}</span>
      </span>
    </label>
  );
}
