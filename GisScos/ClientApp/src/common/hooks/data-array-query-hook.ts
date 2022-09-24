import { Dispatch, SetStateAction, useCallback, useState } from "react";
import { useSearchParams } from "react-router-dom";

import { convertStringToNumber } from "@/common/utils/convert-string-to-number";
import { IQuery } from '@/types/common/query';
import { QueryField } from "@/types/common/query-field";
import { IPaginationResult } from '@/types/common/pagination-result';

/**
 * Набор свойств и методов для реализации пагинации, поиска
 * T - тип определяющий строку массива данных
 */
export type DataArrayQueryType<T> = {
  /**
   * Набор свойств, необходимый для создания запроса на выборку данных с пагинацией, поиском
   */
  queryState: IQuery;

  /**
   * Переменная для хранения, в локальном хранилище, результирующего массива данных
   */
  rows: T[];

  /**
   * Метод изменяет массив данных в локальном хранилище
   */
  setRowsState: Dispatch<SetStateAction<T[]>>;

  /**
   * Метод инициализации данных в в локальном хранилище
   */
  setDataArray: (data: IPaginationResult<T> | undefined) => void;

  /**
   * Метод изменяет свойства для создания запроса на выборку данных с пагинацией, поиском
   */
  setQueryField: (field: QueryField) => (value: any) => void;
};

/**
 * Хук для реализации формы списка данных с пагинацией, поиском.
 * Изменяет url-параметры страницы в зависимости от выбранных значений для запроса данных.
 * @returns Набор свойств и методов для реализации пагинации, поиска.
 * T - тип определяющий строку массива данных.
 */
const useDataArrayQuery = <T>(): DataArrayQueryType<T> => {
  const [searchParams, setSearchParams] = useSearchParams();
  const page = convertStringToNumber(searchParams.get(QueryField.page));
  let pageSize = convertStringToNumber(searchParams.get(QueryField.pageSize));
  pageSize = pageSize > 0 ? pageSize : 25;
  let search = searchParams.get(QueryField.search) ?? '';

  const [queryState, setQueryState] = useState<IQuery>({
    page,
    pageSize,
    rowCount: 0,
    search,
  });

  const [rows, setRowsState ] = useState<T[]>([]);

  const setDataArray = useCallback((data: IPaginationResult<T> | undefined = { rows: [], info: {page, pageSize, rowCount: 0} }) => {
    setQueryState(prev => ({ ...prev, rowCount: data.info.rowCount }));
    setRowsState(() => data.rows);
  }, [page, pageSize]);

  const setQueryField = useCallback((field: QueryField) => (value: any) => {
    if (value) {
      searchParams.set(field, value);
    } else {
      searchParams.delete(field);
    }

    setSearchParams(searchParams);
    setQueryState(prev => {
      const query: IQuery = {...prev};
      query[String(field)] = value;
      return query;
    });
  }, [setSearchParams, searchParams])

  const res: DataArrayQueryType<T> = {
    queryState,
    rows,
    setRowsState,
    setDataArray,
    setQueryField
  };

  return res;
};

export default useDataArrayQuery;