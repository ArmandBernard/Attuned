import { createContext, Dispatch, SetStateAction } from "react";

type EditingContextValue = [boolean, Dispatch<SetStateAction<boolean>>];

export const EditingContext = createContext<EditingContextValue>([
  false,
  () => {},
]);
