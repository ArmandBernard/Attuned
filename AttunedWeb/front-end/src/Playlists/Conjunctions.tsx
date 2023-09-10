import { FunctionComponent } from "react";
import { ConjunctionDto } from "../dtos/Dtos.ts";
import { Rule } from "./Rule.tsx";

export const Conjunctions: FunctionComponent<{
  conjunction: ConjunctionDto;
}> = ({ conjunction }) => {
  // remove media filters
  const filteredRules = conjunction.Rules.filter(
    (x) => !(x.RuleType === "Dictionary" && x.Field === "MediaKind"),
  );

  return (
    <>
      {filteredRules.length > 0 && conjunction.Type}
      <ul>
        {filteredRules.map((x) => (
          <li className="ml-4 list-disc">
            <Rule rule={x} />
          </li>
        ))}
      </ul>
      <ul>
        {conjunction.SubConjunctions.map((x) => (
          <li className="ml-4 list-disc">
            <Conjunctions conjunction={x} />
          </li>
        ))}
      </ul>
    </>
  );
};
