import { HeartIcon } from "./HeartIcon.tsx";
import { StrikeThroughHeartIcon } from "./StrikeThroughHeartIcon.tsx";
import { FunctionComponent, SVGProps } from "react";

type LoveStatusProps = SVGProps<SVGSVGElement> & {
  loved: boolean | undefined;
};

export const LoveStatus: FunctionComponent<LoveStatusProps> = (props) => {
  const { loved, ...rest } = props;

  switch (loved) {
    case true:
      return <HeartIcon style={{ fill: "currentColor" }} {...rest} />;
    case false:
      return (
        <StrikeThroughHeartIcon style={{ fill: "currentColor" }} {...rest} />
      );
    default:
      return (
        <HeartIcon
          style={{
            fill: "none",
            stroke: "currentColor",
            strokeWidth: 32,
          }}
          {...props}
        />
      );
  }
};
