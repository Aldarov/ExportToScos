/**
 * Конвертирует строку в число,
 * если строка не является числом, то возвращается 0
 * @param value входное значение
 * @returns
 */
export const convertStringToNumber = (value: string | null | undefined): number => {
  if (!isNaN(Number(value))) {
    return Number(value);
  } else {
    return 0;
  }
}