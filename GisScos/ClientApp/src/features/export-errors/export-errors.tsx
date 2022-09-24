import React, { useEffect, useMemo, useState } from 'react';
import Typography from '@mui/material/Typography';
import { DataGrid, GridCellEditCommitParams, GridCellParams, GridColDef, GridSelectionModel } from "@mui/x-data-grid";

import { IPaginationInfo } from '@/types/common/pagination-info';
import gridDateGetter from '@/common/utils/grid-date-getter';
import { IHeader } from '@/types/common/header';
import { ExportError } from '@/types/export-error';
import { useSetErrorResolutionStatusMutation, useGetExportErrorsQuery } from './export-service';
import SearchInput from '@/common/components/search-input';
import { Box } from '@mui/material';
import { styles } from './export-errors-styles';
import { toPrettyJson } from '@/common/utils/to-pretty-json';
import useDataArrayQuery from '@/common/hooks/data-array-query-hook';
import { QueryField } from '@/types/common/query-field';


const columns: GridColDef[] = [
  { field: "resolved", headerName: "Исправлена", width: 110, sortable: false, align: 'center', type: 'boolean', editable: true },
  { field: "errorId", headerName: "Код ошибки", width: 200, sortable: false, align: 'center' },
  { field: "createDate", headerName: "Дата ошибки", width: 180, sortable: false, align: 'center', type: 'dateTime', valueGetter: gridDateGetter },
  { field: "entity", headerName: "Сущность", width: 170, sortable: false },
  { field: "externalId", headerName: "Код записи сущности", width: 150, sortable: false, align: 'center' },
  { field: "scosId", headerName: "Код СЦОС", width: 300, sortable: false, align: 'center' },
  { field: "exportQueueId", headerName: "Код пакета", width: 110, sortable: false, align: 'center' }
];


const ExportErrors: React.FC<IHeader> = ({ setHeader }) => {
  const { queryState, rows, setRowsState, setDataArray, setQueryField } = useDataArrayQuery<ExportError>();

  const { data } = useGetExportErrorsQuery({
    page: queryState.page,
    pageSize: queryState.pageSize,
    search: queryState.search
  });
  const [setErrorResolutionStatus] = useSetErrorResolutionStatusMutation();

  useEffect(() => {
    setHeader(
      <Typography variant="h6" color="inherit" component="div">
        Список ошибок при выгрузке данных в ГИС СЦОС
      </Typography>
    );

    setDataArray(data);
  }, [data, setHeader, setDataArray]);

  const handleCellEditCommit = async (params: GridCellEditCommitParams) => {
    const row = { ...(params as GridCellParams).row };
    row[params.field] = params.value;

    try {
      if (params.field === 'resolved') {
        await setErrorResolutionStatus(row).unwrap();
      }

      setRowsState(prev => prev.map(item => (item.id === params.id ? row : item)));
    } catch (error) {
      setRowsState(prev => [...prev]);
    }
  };

  const [selectedRow, setSelectedRowState ] = useState<ExportError>();
  const selectedJson = useMemo(() => toPrettyJson(selectedRow?.json), [selectedRow?.json]);
  const handleSelection = (selectionModel: GridSelectionModel) => {
    const row = rows.filter(item => selectionModel.includes(item.id))[0];
    setSelectedRowState(row);
  }

  return <Box sx={styles.root}>
    <SearchInput
      value={queryState.search}
      handleChange={setQueryField(QueryField.search)}
      handleClear={setQueryField(QueryField.search)}
      sx={styles.searchInput}
    />
    <Box sx={styles.mainPane}>
      <DataGrid
        sx={styles.dataGrid}
        columns={columns}
        rows={rows}
        disableColumnFilter

        pagination
        paginationMode='server'
        {...(queryState as IPaginationInfo)}

        onPageChange={setQueryField(QueryField.page)}
        onPageSizeChange={setQueryField(QueryField.pageSize)}
        onCellEditCommit={handleCellEditCommit}
        onSelectionModelChange={handleSelection}
      />
      <Box sx={styles.rightPane}>
        {
          selectedRow && <>
            <h3>Экспортируемые данные, код пакета - {selectedRow?.exportQueueId}</h3>
            <p><b>Ошибка: {selectedRow?.message}</b></p>
            <pre>{ selectedJson }</pre>
          </>
        }
      </Box>
    </Box>
  </Box>;
}

export default ExportErrors;