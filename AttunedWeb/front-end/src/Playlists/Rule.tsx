import { FunctionComponent, ReactElement } from "react";
import {
  BooleanRuleDto,
  DateRuleDto,
  DictionaryRuleDto,
  IntRuleDto,
  OperatorDto,
  PlaylistRuleDto,
  RatingRuleDto,
  SignDto,
  StringRuleDto,
  TimeSpanRuleDto,
} from "../dtos/Dtos.ts";
import { timeSpanToTimeString } from "../timeSpanToTimeString.ts";
import { getRatingString } from "../getRatingString.ts";

export const Rule: FunctionComponent<{
  rule:
    | BooleanRuleDto
    | DateRuleDto
    | DictionaryRuleDto
    | IntRuleDto
    | RatingRuleDto
    | PlaylistRuleDto
    | StringRuleDto
    | TimeSpanRuleDto;
}> = ({ rule }) => {
  return (
    <>
      {FieldText(rule)}{" "}
      {rule.RuleType === "Boolean"
        ? BooleanRuleText(rule)
        : ValueRuleText(rule)}
    </>
  );
};

const FieldText = (
  rule:
    | BooleanRuleDto
    | DateRuleDto
    | DictionaryRuleDto
    | IntRuleDto
    | RatingRuleDto
    | PlaylistRuleDto
    | StringRuleDto
    | TimeSpanRuleDto,
) => (rule.RuleType === "Playlist" ? "Playlist" : rule.Field);

const BooleanRuleText = (rule: BooleanRuleDto) =>
  rule.Sign === "Positive" ? "is true" : "is false";

const ValueRuleText = (
  rule:
    | DateRuleDto
    | DictionaryRuleDto
    | IntRuleDto
    | RatingRuleDto
    | PlaylistRuleDto
    | StringRuleDto
    | TimeSpanRuleDto,
) => (
  <>
    {GetValueRuleOperatorText(rule.Operator, rule.Sign)}{" "}
    {valueFormatter(rule, "A")}
    {valueFormatter(rule, "B") && <> and {valueFormatter(rule, "B")}</>}
  </>
);

const GetValueRuleOperatorText = (operator: OperatorDto, sign: SignDto) => {
  switch (operator) {
    case OperatorDto.Is:
      return sign === SignDto.Positive ? "is" : "is not";
    case OperatorDto.Contains:
      return sign === SignDto.Positive ? "contains" : "does not contain";
    case OperatorDto.Starts:
      return sign === SignDto.Positive ? "starts with" : "does not start with";
    case OperatorDto.Ends:
      return sign === SignDto.Positive ? "ends with" : "does not end with";
    case OperatorDto.Greater:
      return "is greater than";
    case OperatorDto.Less:
      return "is less than";
    case OperatorDto.Between:
      return "is in the range";
    default:
      operator satisfies never;
  }
};

const valueFormatter = (
  rule:
    | DateRuleDto
    | DictionaryRuleDto
    | IntRuleDto
    | RatingRuleDto
    | PlaylistRuleDto
    | StringRuleDto
    | TimeSpanRuleDto,
  property: "A" | "B",
): ReactElement | string | undefined => {
  switch (rule.RuleType) {
    case "Date": {
      const value = property === "A" ? rule.ValueA : rule.ValueB;
      return value && new Date(value).toLocaleDateString();
    }
    case "TimeSpan":
      return property === "A" ? timeSpanToTimeString(rule.ValueA) : undefined;
    case "Rating": {
      const value = property === "A" ? rule.ValueA : rule.ValueB;
      return value === undefined ? undefined : (
        <span aria-label={`${value} stars`}>{getRatingString(value)}</span>
      );
    }
    case "Int":
      return (property === "A" ? rule.ValueA : rule.ValueB)?.toFixed(0);
    case "Playlist":
    case "Dictionary":
    case "String":
      return property === "A" ? `"${rule.ValueA}"` : undefined;
    default:
      rule satisfies never;
  }
};
