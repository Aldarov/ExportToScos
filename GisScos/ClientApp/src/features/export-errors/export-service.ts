import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

import { IPaginationResult } from '@/types/common/pagination-result';
import { ExportError } from '@/types/export-error';
import { IQuery } from '@/types/common/query';

export const exportService = createApi({
  baseQuery: fetchBaseQuery({ baseUrl: 'api/export' }),
  refetchOnMountOrArgChange: true,
  tagTypes: ['ExportErrors'],
  endpoints: (builder) => ({
    getExportErrors: builder.query<IPaginationResult<ExportError>, IQuery>({
      query: (query) => {
        let url = `get-errors?page=${query.page}&pageSize=${query.pageSize}`;
        if (query.search) {
          url = url + `&search=${query.search}`
        }
        return url;
      },
      providesTags: ['ExportErrors'],
    }),
    setErrorResolutionStatus: builder.mutation<void, ExportError>({
      query: (error) => ({
        url: `set-error-resolution-status?errorId=${error.errorId}&resolved=${error.resolved}`,
        method: 'GET'
      }),
      invalidatesTags: ['ExportErrors'],
    }),

  }),
});

export const { useGetExportErrorsQuery, useSetErrorResolutionStatusMutation } = exportService;