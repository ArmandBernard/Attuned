export const getLoveLabel = (love: boolean | undefined) => {
  switch (love) {
    case true:
      return "loved";
    case false:
      return "disliked";
    default:
      return "not loved";
  }
};
