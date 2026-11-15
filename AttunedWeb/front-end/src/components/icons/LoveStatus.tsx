import { HeartIcon } from "./HeartIcon.tsx";
import { FunctionComponent, SVGProps } from "react";
import HeartIconFilled from "@components/icons/HeartIconFilled.tsx";

type LoveStatusProps = SVGProps<SVGSVGElement> & {
  loved: boolean | undefined;
};

export const LoveStatus: FunctionComponent<LoveStatusProps> = (props) => {
  const { loved, ...rest } = props;

  switch (loved) {
    case true:
      return <HeartIconFilled style={{ fill: "currentColor" }} {...rest} />;
    case false:
    default:
      return (
        <HeartIcon
          style={{
            fill: "currentColor",
          }}
          {...props}
        />
      );
  }
};
