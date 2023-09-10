import { TimeSpan } from "./dtos/TimeSpan.ts";

export const timeSpanToTimeString = (timeSpan: TimeSpan) => {
  const ISOTime = new Date(timeSpan).toISOString().slice(11, -5);

  // remove leading 00: if present and any leading minutes zero
  return ISOTime.replace(/(00:)?0?/, "");
};
