export const getLoveString = (love: boolean | undefined) => {
  switch (love) {
    case true:
      return "♥";
    case false:
      return "💔";
    default:
      return "♡";
  }
};
