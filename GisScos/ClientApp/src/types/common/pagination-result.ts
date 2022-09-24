import { IPaginationInfo } from "./pagination-info";

/**
 * Интерфейс возвращающий массив данных с информацией для реализации пагинации
 */
export interface IPaginationResult<T> {
  /**
   * Массив с данными
   */
  rows: Array<T>;

  /**
   * Информация о пагинации
   */
  info: IPaginationInfo
}