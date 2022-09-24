export const styles = {
  root: {
    flex: 1,
    display: 'flex',
    flexDirection: 'column'
  },
  searchInput: {
    mb: 1
  },
  mainPane: {
    flex: 1,
    display: 'flex',
    overflowX: 'auto'
  },
  dataGrid: {
    flex: 1,
    minWidth: '350px',
    '& .MuiDataGrid-cell': {
      display: 'block',
      lineHeight: '52px',
    },
    '& .MuiDataGrid-cell--textCenter': {
      textAlign: 'center'
    },
    '& .MuiDataGrid-cell--editable': {
      bgcolor: 'rgb(245 245 245)'
    },
  },
  rightPane: {
    width: '600px',
    minWidth: '350px',
    ml: 1,
    p: 1,
    border: '1px solid rgba(224, 224, 224, 1)',
    borderRadius: 1,
    overflowY: 'auto',
  },
} as const;