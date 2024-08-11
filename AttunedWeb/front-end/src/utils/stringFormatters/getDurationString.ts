import { TimeSpan } from "@root/dtos/TimeSpan.ts";

export const getDurationString = (timeSpan: TimeSpan) => {
  const days = timeSpan / (24 * 60 * 60 * 1000);
  if (days > 1) {
    return `${days.toLocaleString(undefined, { maximumFractionDigits: 1 })} days`;
  }
  const hours = Math.trunc(timeSpan / (60 * 60 * 1000));
  const minutes = Math.trunc(timeSpan / (60 * 1000) - hours * 60);

  if (hours >= 1) {
    return `${hours} hours, ${minutes} minutes`;
  } else {
    return `${minutes} minutes`;
  }
};
