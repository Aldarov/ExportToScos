import { IPagination } from "./pagination";

/**
 * Информацией для реализации пагинации
 */
export interface IPaginationInfo extends IPagination {
  /**
   * Общее кол-во записей
   */
  rowCount?: number;
}