/**
 * Форматирует json, добавляя отступы
 * @param json json-строка
 * @returns
 */
export const toPrettyJson = (json: string | undefined): string => {
  if (json === undefined)
    return '';

  let obj: any;
  try {
    obj = JSON.parse(json)
  } catch (error) {
    throw Error("Неверный формат json");
  }

  return JSON.stringify(obj, null, 2);
}