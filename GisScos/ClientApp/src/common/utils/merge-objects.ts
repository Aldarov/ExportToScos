export const mergeObjects = (obj1?: object, obj2?: object): object | null | undefined => {
  return {...obj1, ...obj2}
};