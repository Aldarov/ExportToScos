import { ISearch } from "./search";
import { IPaginationInfo } from './pagination-info';

/**
 * Общий интерфейс для реализации запросов с пагинацией, поиском, фильтрации и др.
 */
export interface IQuery extends IPaginationInfo, ISearch, Record<string,any> {
}